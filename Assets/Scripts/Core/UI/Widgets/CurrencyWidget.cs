using TMPro;
using UnityEngine;

public class CurrencyWidget : MonoBehaviour
{
    private const string NULL_ID = "Null_ID";

    [SerializeField] private TMP_Text m_currencyText;
    [SerializeField] private string m_currencyKey = SaveKeys.Currency.Coins;

    private void Awake()
    {
        CurrencyManager.OnCurrencyUpdated += OnCurrencyUpdated;
    }

    private void OnDestroy()
    {
        CurrencyManager.OnCurrencyUpdated -= OnCurrencyUpdated;
    }

    private void Start()
    {
        if (string.IsNullOrEmpty(m_currencyKey))
        {
            Devlog.LogError($"Currency Key not set on the gameObject: {gameObject.name}.");
            m_currencyText.text = NULL_ID;
            return;
        }
        
        long currencyAmount = CurrencyManager.Instance.GetCurrency(m_currencyKey);
        m_currencyText.text = currencyAmount.ToString();
    }

    private void OnCurrencyUpdated(string currencyName, long newAmount)
    {
        if (m_currencyKey == currencyName)
        {
            m_currencyText.text = newAmount.ToString();
        }
    }
}
