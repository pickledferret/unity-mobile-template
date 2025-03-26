using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; }

    public delegate void CurrencyAmountDelegate(string currencyName, long newAmount);
    public static event CurrencyAmountDelegate OnCurrencyUpdated;

    private Dictionary<string, long> m_currencies = new();
    private Dictionary<string, Coroutine> m_currencyValueLerpCoroutines = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        LoadAllCurrencies();
    }

    private void LoadAllCurrencies()
    {
        foreach (string currencyKey in SaveKeys.Currency.AllCurrencyKeys)
        {
            RegisterCurrency(currencyKey);
        }
    }

    private bool IsCurrencyRegistered(string currencyName)
    {
        return Instance.m_currencies.ContainsKey(currencyName);
    }

    /// <summary>
    /// Register a new currency.
    /// </summary>
    /// <param name="currencyName">CurrencyName should be pre-defined in SaveKeys.cs</param>
    /// <param name="initialAmount"></param>
    public void RegisterCurrency(string currencyName, long initialAmount = 0)
    {
        if (IsCurrencyRegistered(currencyName))
        {
            Devlog.LogWarning($"{name}: Currency: {currencyName} already registered.");
        }
        else
        {
            long savedAmount = SavePrefs.LoadLong(currencyName, initialAmount);
            m_currencies[currencyName] = savedAmount;
            Devlog.Log($"{name}: Loaded currency {currencyName} :: {initialAmount}");
        }
    }


    /// <summary>
    /// Add an amount to a currency.
    /// </summary>
    /// <param name="currencyName">CurrencyName should be pre-defined in SaveKeys.cs</param>
    /// <param name="amount">Amount to add to the currency.</param>
    public void AddCurrency(string currencyName, long amount)
    {
        if (IsCurrencyRegistered(currencyName))
        {
            long oldCurrencyAmount = m_currencies[currencyName];
            long newCurrencyAmount = Math.Max(0, oldCurrencyAmount + amount);

            m_currencies[currencyName] = newCurrencyAmount;
            SavePrefs.SaveLong(currencyName, newCurrencyAmount);

            LerpCurrencyValue(currencyName, oldCurrencyAmount, newCurrencyAmount);

        }
    }


    /// <summary>
    /// Subtract amount from a currency if
    /// </summary>
    /// <param name="currencyName">CurrencyName should be pre-defined in SaveKeys.cs</param>
    /// <param name="amount"></param
    public void SpendCurrency(string currencyName, long amount)
    {
        if (IsCurrencyRegistered(currencyName))
        {
            if (CanAfford(currencyName, amount))
            {
                long oldCurrencyAmount = m_currencies[currencyName];
                long newCurrencyAmount = oldCurrencyAmount - amount;

                m_currencies[currencyName] -= amount;
                SavePrefs.SaveLong(currencyName, m_currencies[currencyName]);

                LerpCurrencyValue(currencyName, oldCurrencyAmount, newCurrencyAmount);
            }
        }
    }


    /// <summary>
    /// Check if user can afford an amount of a currency.
    /// </summary>
    /// <param name="currencyName">CurrencyName should be pre-defined in SaveKeys.cs</param>
    /// <param name="amount"></param>
    /// <returns></returns>
    public bool CanAfford(string currencyName, long amount)
    {
        if (IsCurrencyRegistered(currencyName))
        {
            if (amount >= 0f && m_currencies[currencyName] >= amount)
            {
                return true;
            }
            else
            {
                Devlog.LogWarning($"{name}: Not enough {currencyName} to spend.");
                return false;
            }
        }

        return false;
    }


    /// <summary>
    /// Return the current balance of a registered currency.
    /// </summary>
    /// <param name="currencyName">CurrencyName should be pre-defined in SaveKeys.cs</param>
    /// <returns></returns>
    public long GetCurrency(string currencyName)
    {
        if (IsCurrencyRegistered(currencyName))
        {
            return m_currencies[currencyName];
        }
        Devlog.LogError($"{currencyName} is not a registered currency.");
        return -1;
    }


    /// <summary>
    /// Overrides the currency amount - hard-setting its value.
    /// </summary>
    /// <param name="currencyName">CurrencyName should be pre-defined in SaveKeys.cs</param>
    /// <param name="amount"></param>
    public void OverrideCurrencyAmount(string currencyName, long amount)
    {
        if (IsCurrencyRegistered(currencyName))
        {
            Devlog.Log($"{name}: Currency {currencyName} value overridden from {m_currencies[currencyName]} to {amount}.");
            m_currencies[currencyName] = amount;
            SavePrefs.SaveLong(currencyName, amount);
            OnCurrencyUpdated?.Invoke(currencyName, amount);
        }
    }

    private void LerpCurrencyValue(string currencyName, long startValue, long endValue)
    {
        if (m_currencyValueLerpCoroutines.ContainsKey(currencyName) && m_currencyValueLerpCoroutines[currencyName] != null)
        {
            StopCoroutine(m_currencyValueLerpCoroutines[currencyName]);
        }
        m_currencyValueLerpCoroutines[currencyName] = StartCoroutine(LerpCurrencyRoutine(currencyName, startValue, endValue));
    }

    private IEnumerator LerpCurrencyRoutine(string currencyName, long startValue, long endValue)
    {
        float duration = 0.5f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            long lerpedAmount = (long)Mathf.Lerp(startValue, endValue, t);

            OnCurrencyUpdated?.Invoke(currencyName, lerpedAmount);

            yield return null;
        }

        m_currencies[currencyName] = endValue;
        OnCurrencyUpdated?.Invoke(currencyName, endValue);
        m_currencyValueLerpCoroutines[currencyName] = null;
    }
}