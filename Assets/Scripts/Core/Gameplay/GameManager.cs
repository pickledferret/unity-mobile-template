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

        m_currentLoadedSceneName = m_loadedLevelList[m_currentLevelIndex];

        SceneManager.LoadScene(m_currentLoadedSceneName, LoadSceneMode.Additive);

        ScreenManager.Instance.ReplaceScreen(StartScreen.PATH);
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

    public void LevelStarted()
    {
        GameplayEvents.TriggerLevelStart();
        ScreenManager.Instance.ReplaceScreen(GameScreen.PATH);
    }

    public void LevelCompleted()
    {
        AudioManager audioManager = AudioManager.Instance;
        audioManager.PlaySFXAudio(audioManager.AudioSoundList.sfx.levelCompleteSFX);

        m_currentLevelIndex++;

        if (m_currentLevelIndex == m_loadedLevelList.Count)
        {
            // Max Level Reached.. Go back to the start.
            m_currentLevelIndex = 0;
        }

        SavePrefs.SaveInt(SaveKeys.Progression.CurrentLevelIndex, m_currentLevelIndex);

        ScreenManager.Instance.ReplaceScreen(LevelCompleteScreen.PATH);
    }

    public void LevelFailed()
    {
        ScreenManager.Instance.ReplaceScreen(LevelFailedScreen.PATH);
    }

    public void GoToNextLevel()
    {
        FadeOutAndLoadCurrentLevel();
    }

    public void ResetCurrentLevel()
    {
        FadeOutAndLoadCurrentLevel();
    }

    private void FadeOutAndLoadCurrentLevel()
    {
        FadeToBlackPopUp screenFade = ScreenManager.Instance.ShowPopUp<FadeToBlackPopUp>(FadeToBlackPopUp.PATH);
        screenFade.FullFade(0.5f, ReloadLevelScene, 0.1f, 0.5f, null);
    }

    public void ResetGameToBeginning()
    {
        m_currentLevelIndex = 0;
        SavePrefs.SaveInt(SaveKeys.Progression.CurrentLevelIndex, m_currentLevelIndex);

        FadeToBlackPopUp screenFade = ScreenManager.Instance.ShowPopUp<FadeToBlackPopUp>(FadeToBlackPopUp.PATH);
        screenFade.FullFade(0.5f, ReloadLevelScene, 0.1f, 0.5f, null);
    }

    private void ReloadLevelScene()
    {
        UnloadCurrentLevel();
        LoadCurrentLevel();
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