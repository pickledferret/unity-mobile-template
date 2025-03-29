using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DebugScreen : ScreenBase
{
    public const string PATH = "Prefabs/UI/Screens/DebugScreen";

    [SerializeField] private Button m_closeDebugScreenButton;

    [Header("Debug Console")]
    [SerializeField] private Button m_toggleConsoleLogButton;
    [SerializeField] private Button m_clearConsoleLogButton;
    [SerializeField] private DebugConsoleLog m_consoleLog;
    private bool m_consoleActive;

    [Header("General")]
    [SerializeField] private Button m_clearSavePrefs;

    [Header("Level Progress")]
    [SerializeField] private TMP_Text m_currentLevel;
    [SerializeField] private TMP_Text m_maxLevelIndex;
    [SerializeField] private TMP_InputField m_levelOverride;
    [SerializeField] private Button m_overrideLevelButton;

    [Header("Currency")]
    [SerializeField] private TMP_Dropdown m_currencyDropdown;
    [SerializeField] private Button m_addCurrencyButton;
    [SerializeField] private Button m_spendCurrencyButton;
    [SerializeField] private TMP_InputField m_amountInput;

    [Header("Haptics")]
    [SerializeField] private TMP_Dropdown m_hapticTypeDropdown;
    [SerializeField] private TMP_InputField m_hapticBurstDurationInput;
    [SerializeField] private Button m_triggerHapticButton;
    [SerializeField] private Button m_triggerHapticBurstButton;

    private void Start()
    {
        SetUpCoreDebug();
        SetUpGeneralDebug();
        SetUpLevelProgressDebug();
        SetUpCurrencyDebug();
        SetUpHapticsDebug();
    }

    private void SetUpCoreDebug()
    {
        // Close Button
        m_closeDebugScreenButton.onClick.AddListener(() => ScreenManager.Instance.PopScreen(true));

        // Console Log
        m_toggleConsoleLogButton.onClick.AddListener(ToggleConsoleLogVisibility);
        m_clearConsoleLogButton.onClick.AddListener(ClearConsoleLog);
        m_consoleLog.ShowConsoleLog(false);
    }

    private void ToggleConsoleLogVisibility()
    {
        m_consoleActive = !m_consoleActive;
        m_consoleLog.ShowConsoleLog(m_consoleActive);
    }

    private void ClearConsoleLog()
    {
        m_consoleLog.ClearLogs();
    }

    #region GENERAL
    private void SetUpGeneralDebug()
    {
        m_clearSavePrefs.onClick.AddListener(SavePrefs.ClearAllKeys);
    }
    #endregion


    #region LEVEL PROGRESS
    private void SetUpLevelProgressDebug()
    {
        m_currentLevel.text = $"Current Level Index: {GameManager.CurrentLevelIndex}";

        int maxLevelIndex = GameManager.Instance.GetNumberOfLevels() - 1;
        m_maxLevelIndex.text = $"Max Level Index: {maxLevelIndex}";

        m_overrideLevelButton.onClick.AddListener(OverrideLevel);
        m_levelOverride.onValueChanged.AddListener(LevelOverrideValueChanged);
    }

    private void LevelOverrideValueChanged(string val)
    {
        if (int.TryParse(m_levelOverride.text, out int levelOverride))
        {
            int maxLevelIndex = GameManager.Instance.GetNumberOfLevels() - 1;
            m_levelOverride.text = Mathf.Clamp(levelOverride, 0, maxLevelIndex).ToString();
        }
    }

    private void OverrideLevel()
    {
        if (int.TryParse(m_levelOverride.text, out int levelOverride))
        {
            SavePrefs.SaveInt(SaveKeys.Progression.CurrentLevelIndex, levelOverride);
            m_currentLevel.text = $"Current Level Index: {levelOverride}";
            GameManager.Instance.ResetCurrentLevel();
            ScreenManager.Instance.PopScreen();
        }
    }
    #endregion


    #region CURRENCY DEBUG
    private void SetUpCurrencyDebug()
    {
        m_currencyDropdown.ClearOptions();
        List<string> currencies = new List<string>(SaveKeys.Currency.AllCurrencyKeys);
        m_currencyDropdown.AddOptions(currencies);

        m_addCurrencyButton.onClick.AddListener(AddCurrency);
        m_spendCurrencyButton.onClick.AddListener(SpendCurrency);
    }

    private void AddCurrency()
    {
        string selectedCurrency = SaveKeys.Currency.AllCurrencyKeys[m_currencyDropdown.value];
        if (long.TryParse(m_amountInput.text, out long amount))
        {
            CurrencyManager.Instance.AddCurrency(selectedCurrency, amount);
        }
    }

    private void SpendCurrency()
    {
        string selectedCurrency = SaveKeys.Currency.AllCurrencyKeys[m_currencyDropdown.value];
        if (long.TryParse(m_amountInput.text, out long amount))
        {
            CurrencyManager.Instance.SpendCurrency(selectedCurrency, amount);
        }
    }
    #endregion


    #region HAPTICS
    private void SetUpHapticsDebug()
    {
        m_triggerHapticButton.onClick.AddListener(TriggerHaptic);
        m_triggerHapticBurstButton.onClick.AddListener(TriggerHapticBurst);

        m_hapticTypeDropdown.ClearOptions();
        List<string> hapticTypes = Enum.GetNames(typeof(Haptics.HapticType)).ToList();
        m_hapticTypeDropdown.AddOptions(hapticTypes);
    }

    private void TriggerHaptic()
    {
        Haptics.HapticType selectedHaptic = (Haptics.HapticType)m_hapticTypeDropdown.value;
        Haptics.PlayHaptics(selectedHaptic);
    }

    private void TriggerHapticBurst()
    {
        if (int.TryParse(m_hapticBurstDurationInput.text, out int durationMs))
        {
            Haptics.HapticType selectedHaptic = (Haptics.HapticType)m_hapticTypeDropdown.value;
            Haptics.PlayHapticsBurst(durationMs, selectedHaptic);
        }
    }
    #endregion
}