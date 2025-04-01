using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public static int CurrentLevelIndex => Instance.m_currentLevelIndex;

    [Header("Debug")]
    [Tooltip("Only Works In Editor - Overrides the level loaded to selected index, -1 to ignore.")]
    [SerializeField] private int m_levelOverride = -1;
    [SerializeField] private LevelList m_levelList;

    private int m_currentLevelIndex = 0;

    private List<string> m_loadedLevelList;
    private string m_currentLoadedSceneName;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        SplashScreen.OnSplashScreenFinished += OnSplashScreenFinished;
    }

    private void Start()
    {
        m_loadedLevelList = m_levelList.GetSceneNames();

#if UNITY_EDITOR
        SetDebugLevelOverride();
#endif
        PlayBackgroundMusic();
        LoadCurrentLevel();
    }

    private void OnSplashScreenFinished()
    {
        SplashScreen.OnSplashScreenFinished -= OnSplashScreenFinished;
        ShowStartScreen();
    }

    /// -- Screen display --
    private void ShowStartScreen() => ScreenManager.Instance.ReplaceScreen(StartScreen.PATH);
    private void ShowGameScreen() => ScreenManager.Instance.ReplaceScreen(GameScreen.PATH);
    private void ShowLevelCompleteScreen() => ScreenManager.Instance.ReplaceScreen(LevelCompleteScreen.PATH);
    private void ShowLevelFailedScreen() => ScreenManager.Instance.ReplaceScreen(LevelFailedScreen.PATH);
    private void ShowGameCompleteScreen() => ScreenManager.Instance.ReplaceScreen(GameCompleteScreen.PATH);
    /// -----------------------------------------------------------------------------------------

    private void PlayBackgroundMusic()
    {
        AudioManager audioManager = AudioManager.Instance;
        audioManager.PlayMusicAudio(audioManager.AudioSoundList.music.backgroundMusic);
    }

    private void LoadCurrentLevel()
    {
        if (m_loadedLevelList == null || m_loadedLevelList.Count == 0)
        {
            Devlog.LogError("[GameManager]: No scenes found in the loaded level list.");
            return;
        }

        m_currentLevelIndex = SavePrefs.LoadInt(SaveKeys.Progression.CurrentLevelIndex);

        if (m_currentLevelIndex >= m_loadedLevelList.Count || m_currentLevelIndex < 0)
        {
            // Note: If you enter this check, liklihood is that the player quit the game after completing the last level, but before ResetGameProgressToBeginning() is called.
            //       resets game progress back to 0 for safety.
            Devlog.LogWarning($"[GameManager]: Level Index: ({m_currentLevelIndex}) exceeds the current level list length. Resetting game progress to 0.");
            m_currentLevelIndex = 0;
            SavePrefs.SaveInt(SaveKeys.Progression.CurrentLevelIndex, m_currentLevelIndex);
        }

        m_currentLoadedSceneName = m_loadedLevelList[m_currentLevelIndex];

        SceneManager.LoadScene(m_currentLoadedSceneName, LoadSceneMode.Additive);
    }

    private void UnloadCurrentLevel()
    {
        Scene sceneToUnload = SceneManager.GetSceneByName(m_currentLoadedSceneName);
        if (sceneToUnload.IsValid() && sceneToUnload.isLoaded)
        {
            SceneManager.UnloadSceneAsync(m_currentLoadedSceneName);
        }
        else
        {
            Devlog.LogWarning($"[GameManager]: Scene {m_currentLoadedSceneName} is not valid or not loaded. Cannot unload scene.");
        }
    }

    public void StartLevel()
    {
        GameplayEvents.TriggerLevelStart();
        ShowGameScreen();
    }

    public void CompleteLevel()
    {
        AudioManager audioManager = AudioManager.Instance;
        audioManager.PlaySFXAudio(audioManager.AudioSoundList.sfx.levelCompleteSFX);

        IncrementLevelIndex();
        SavePrefs.SaveInt(SaveKeys.Progression.CurrentLevelIndex, m_currentLevelIndex);

        ShowLevelCompleteScreen();
    }

    public void FailLevel()
    {
        ShowLevelFailedScreen();
    }

    private void IncrementLevelIndex()
    {
        m_currentLevelIndex++;

        // Check If Max Level Reached
        if (m_currentLevelIndex >= m_loadedLevelList.Count)
        {
            if (m_levelList.loopingLevels)
            {
                m_currentLevelIndex = Mathf.Clamp(m_levelList.elementToLoopBackTo, 0, m_loadedLevelList.Count);
            }
        }
    }

    public void LoadNextLevel()
    {
        PerformSceneTransition(CheckForGameCompletion);
    }

    private void CheckForGameCompletion()
    {
        if (m_currentLevelIndex >= m_loadedLevelList.Count)
        {
            ShowGameCompleteScreen();
        }
        else
        {
            ReloadCurrentScene();
        }
    }

    public void ResetCurrentLevel()
    {
        PerformSceneTransition(ReloadCurrentScene);
    }

    public void ResetGameProgressToBeginning()
    {
        m_currentLevelIndex = 0;
        SavePrefs.SaveInt(SaveKeys.Progression.CurrentLevelIndex, m_currentLevelIndex);
        PerformSceneTransition(ReloadCurrentScene);
    }

    private void PerformSceneTransition(Action onBlackScreenCallback)
    {
        FadeToBlackPopUp screenFade = ScreenManager.Instance.ShowPopUp<FadeToBlackPopUp>(FadeToBlackPopUp.PATH);
        screenFade.FullFade(0.5f, onBlackScreenCallback, 0.1f, 0.5f, null);
    }

    private void ReloadCurrentScene()
    {
        UnloadCurrentLevel();
        LoadCurrentLevel();
        ShowStartScreen();
    }

    public int GetNumberOfLevels()
    {
        return m_loadedLevelList.Count;
    }

#if UNITY_EDITOR
    private void SetDebugLevelOverride()
    {
        if (m_levelOverride > -1)
        {
            m_currentLevelIndex = m_levelOverride;
            SavePrefs.SaveInt(SaveKeys.Progression.CurrentLevelIndex, m_currentLevelIndex);
        }
    }
#endif
}
