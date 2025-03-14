using System.Collections;
using System.Collections.Generic;
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

    private string m_currentLoadedScene;

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
        LoadCurrentLevel();
    }

    private void LoadCurrentLevel()
    {
        m_currentLoadedScene = LEVEL + m_currentLevel;
        SceneManager.LoadScene(m_currentLoadedScene, LoadSceneMode.Additive);

        ScreenManager.Instance.ReplaceScreen(StartScreen.PATH);
    }

    public void StartLevel()
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
        screenFade.FullFade(0.5f, () =>
        {
            SceneManager.UnloadSceneAsync(m_currentLoadedScene);
            LoadCurrentLevel();
        }, 0.1f, 0.5f, null);
    }

    public void ResetGameToBeginning()
    {
        m_currentLevel = 0;

        FadeToBlackPopUp screenFade = ScreenManager.Instance.ShowPopUp<FadeToBlackPopUp>(FadeToBlackPopUp.PATH);
        screenFade.FullFade(0.5f, () =>
        {
            LoadCurrentLevel();
        }, 0.1f, 0.5f, null);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            LevelCompleted();
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
        }
    }
#endif
}