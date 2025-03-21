using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private const string LEVEL = "Level";

    public static GameManager Instance { get; private set; }

    [SerializeField] private int m_maxLevelSize = 12;

    [Header("Debug")]
    [Tooltip("Only Works In Editor - Overrides the level loaded to selected index, -1 to ignore.")]
    [SerializeField] private int m_levelOverride = -1;

    private int m_currentLevel = 0;
    public int CurrentLevel => m_currentLevel;

    private StringBuilder m_currentLoadedScene;

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
        m_currentLevel = SavePrefs.LoadInt(SaveKeys.CurrentLevel);

        if (m_currentLevel >= m_maxLevelSize)
        {
            // Max Level Reached..
            m_currentLevel--;
        }

        m_currentLoadedScene = new StringBuilder();
        m_currentLoadedScene.Append(LEVEL).Append(m_currentLevel);
        SceneManager.LoadScene(m_currentLoadedScene.ToString(), LoadSceneMode.Additive);

        ScreenManager.Instance.ReplaceScreen(StartScreen.PATH);
    }

    private void UnloadCurrentLevel()
    {
        Scene sceneToUnload = SceneManager.GetSceneByName(m_currentLoadedScene.ToString());
        if (sceneToUnload.IsValid() && sceneToUnload.isLoaded)
        {
            SceneManager.UnloadSceneAsync(m_currentLoadedScene.ToString());
            m_currentLoadedScene.Clear();
        }
        else
        {
            Devlog.LogWarning($"Scene {m_currentLoadedScene.ToString()} is not valid or not loaded. Cannot unload scene.");
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

        m_currentLevel++;

        if (m_currentLevel == m_maxLevelSize)
        {
            // Max Level Reached..
            m_currentLevel--;
        }

        SavePrefs.SaveInt(SaveKeys.CurrentLevel, m_currentLevel);

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
        m_currentLevel = 0;
        SavePrefs.SaveInt(SaveKeys.CurrentLevel, m_currentLevel);

        FadeToBlackPopUp screenFade = ScreenManager.Instance.ShowPopUp<FadeToBlackPopUp>(FadeToBlackPopUp.PATH);
        screenFade.FullFade(0.5f, ReloadLevelScene, 0.1f, 0.5f, null);
    }

    private void ReloadLevelScene()
    {
        UnloadCurrentLevel();
        LoadCurrentLevel();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            LevelCompleted();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            ResetGameToBeginning();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            LevelFailed();
        }
    }

#if UNITY_EDITOR
    private void SetDebugLevelOverride()
    {
        if (m_levelOverride > -1)
        {
            m_currentLevel = m_levelOverride;
            SavePrefs.SaveInt(SaveKeys.CurrentLevel, m_currentLevel);
        }
    }
#endif
}