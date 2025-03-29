#if UNITY_EDITOR || DEVLOG
using System;
using System.Collections.Generic;
using UnityEngine;

public static class DebugConsoleLogHandler
{
    public static Queue<string> LogQueue { get; private set; } = new Queue<string>();
    private const int MaxLogs = 1000;

    public static event Action OnLogReceived;

    static DebugConsoleLogHandler()
    {
        Application.logMessageReceived += HandleLog;
    }
    
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Init()
    {
    }

    private static void HandleLog(string logString, string stackTrace, LogType type)
    {
        string color = type switch
        {
            LogType.Error => "<color=red>",
            LogType.Warning => "<color=yellow>",
            _ => "<color=white>"
        };

        string formattedMessage = $"{color}{logString}\n{stackTrace}</color>";
        LogQueue.Enqueue(formattedMessage);

        if (LogQueue.Count > MaxLogs)
        {
            LogQueue.Dequeue();
        }

        OnLogReceived?.Invoke();
    }
}
#endif
