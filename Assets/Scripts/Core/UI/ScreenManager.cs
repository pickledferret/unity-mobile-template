using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    public static ScreenManager Instance { get; private set; }

    private Stack<ScreenBase> m_screenStack = new Stack<ScreenBase>();
    private Stack<PopUpBase> m_popUpStack = new Stack<PopUpBase>();

    [SerializeField] private Canvas m_canvas;
    public Canvas Canvas => m_canvas;

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

    /// <summary>
    /// Push a new screen onto the stack
    /// </summary>
    /// <param name="newScreen"></param>
    /// <param name="disablePrevious"></param>
    public void PushScreen(string screenPath, bool disablePrevious = true)
    {
        ScreenBase loadedScreen = Resources.Load<ScreenBase>(screenPath);
        if (loadedScreen == null)
        {
            Devlog.LogError($"[ScreenManager] Screen prefab not found at path: {screenPath}");
            return;
        }

        if (m_screenStack.Count > 0 && disablePrevious)
        {
            m_screenStack.Peek().gameObject.SetActive(false);
        }

        ScreenBase newScreen = Instantiate(loadedScreen, m_canvas.transform);
        newScreen.gameObject.SetActive(true);
        m_screenStack.Push(newScreen);
    }

    /// <summary>
    /// Pop the top screen off the stack
    /// </summary>
    public void PopScreen(bool enablePrevious = true)
    {
        if (m_screenStack.Count == 0)
        {
            return;
        }

        ScreenBase topScreen = m_screenStack.Pop();
        Destroy(topScreen.gameObject);

        if (enablePrevious && m_screenStack.Count > 0)
        {
            m_screenStack.Peek().gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Replace the top screen in the stack with a new one
    /// </summary>
    /// <param name="newScreen"></param>
    public void ReplaceScreen(string newScreen)
    {
        if (m_screenStack.Count > 0)
        {
            PopScreen();
        }

        PushScreen(newScreen);
    }

    /// <summary>
    /// Show a PopUp by loading it from Resources.Load(popUpPath);
    /// </summary>
    /// <param name="popUpPath"></param>
    public T ShowPopUp<T>(string popUpPath) where T : PopUpBase
    {
        if (string.IsNullOrEmpty(popUpPath))
        {
            Devlog.LogError("PopUp path is empty.");
            return null;
        }

        PopUpBase popUpPrefab = Resources.Load<PopUpBase>(popUpPath);
        if (popUpPrefab == null)
        {
            Devlog.LogError($"PopUp not found at path: {popUpPath}");
            return null;
        }

        return Instantiate(popUpPrefab, m_canvas.transform) as T;
    }
}