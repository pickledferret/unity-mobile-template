using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DebugScreen : MonoBehaviour
{
    [SerializeField] private CanvasGroup m_canvasGroup;
    [SerializeField] private Transform m_scrollViewContent;
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

#if DEVLOG
    public static event Action OnOpenDebugScreen;
    public static void TriggerOpenDebugScreen()
    {
        OnOpenDebugScreen?.Invoke();
    }
#endif


    private void Awake()
    {
#if DEVLOG
        OnOpenDebugScreen += OpenDebugScreen;
        RegisterListeners();
        ToggleContent(false);
#else
        gameObject.SetActive(false);
#endif
    }

    private void OnDestroy()
    {
#if DEVLOG
        OnOpenDebugScreen -= OpenDebugScreen;
#endif
    }


#if DEVLOG
    private void RegisterListeners()
    {
        RegisterCoreDebugListeners();
        RegisterGeneralDebuglisteners();
        RegisterLevelProgressDebugListeners();
        RegisterCurrencyDebugListeners();
        RegisterHapticsDebugListeners();
    }

    private void ToggleContent(bool show)
    {
        for (int i = 0; i < m_scrollViewContent.childCount; i++)
        {
            m_scrollViewContent.GetChild(i).gameObject.SetActive(show);
        }
    }

    private void Setup()
    {
        transform.SetAsLastSibling();

        SetUpCoreDebug();
        SetUpGeneralDebug();
        SetUpLevelProgressDebug();
        SetUpCurrencyDebug();
        SetUpHapticsDebug();

        ToggleContent(true);
    }

    #region CORE DEBUG SETUP
    private void RegisterCoreDebugListeners()
    {
        m_closeDebugScreenButton.onClick.AddListener(CloseDebugScreen);
        m_toggleConsoleLogButton.onClick.AddListener(ToggleConsoleLogVisibility);
        m_clearConsoleLogButton.onClick.AddListener(ClearConsoleLog);
    }

    private void SetUpCoreDebug()
    {
        m_consoleActive = false;
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

    private void OpenDebugScreen()
    {
        Setup();
        m_canvasGroup.alpha = 1f;
        m_canvasGroup.interactable = true;
        m_canvasGroup.blocksRaycasts = true;
    }

    private void CloseDebugScreen()
    {
        m_canvasGroup.alpha = 0f;
        m_canvasGroup.interactable = false;
        m_canvasGroup.blocksRaycasts = false;
    }
    #endregion


    #region GENERAL
    private void RegisterGeneralDebuglisteners()
    {
        m_clearSavePrefs.onClick.AddListener(SavePrefs.ClearAllKeys);
    }

    private void SetUpGeneralDebug()
    {
        // ..
    }
    #endregion


    #region LEVEL PROGRESS
    private void RegisterLevelProgressDebugListeners()
    {
        m_overrideLevelButton.onClick.AddListener(OverrideLevel);
        m_levelOverride.onValueChanged.AddListener(LevelOverrideValueChanged);
    }

    private void SetUpLevelProgressDebug()
    {
        m_currentLevel.text = $"Current Level Index: {GameManager.CurrentLevelIndex}";

        int maxLevelIndex = GameManager.Instance.GetNumberOfLevels() - 1;
        m_maxLevelIndex.text = $"Max Level Index: {maxLevelIndex}";
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
    private void RegisterCurrencyDebugListeners()
    {
        m_addCurrencyButton.onClick.AddListener(AddCurrency);
        m_spendCurrencyButton.onClick.AddListener(SpendCurrency);
    }

    private void SetUpCurrencyDebug()
    {
        m_currencyDropdown.ClearOptions();
        List<string> currencies = new List<string>(SaveKeys.Currency.AllCurrencyKeys);
        m_currencyDropdown.AddOptions(currencies);
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
    private void RegisterHapticsDebugListeners()
    {
        m_triggerHapticButton.onClick.AddListener(TriggerHaptic);
        m_triggerHapticBurstButton.onClick.AddListener(TriggerHapticBurst);
    }

    private void SetUpHapticsDebug()
    {
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


#endif
}