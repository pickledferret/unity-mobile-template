using UnityEngine;

public static class Devlog
{
    public const string DEVLOG_SYMBOL = "DEVLOG";

    [System.Diagnostics.Conditional(DEVLOG_SYMBOL)]
    public static void Log(object message)
    {
        Debug.Log(message);
    }

    [System.Diagnostics.Conditional(DEVLOG_SYMBOL)]
    public static void Log(object message, Object context)
    {
        Debug.Log(message, context);
    }

    [System.Diagnostics.Conditional(DEVLOG_SYMBOL)]
    public static void LogWarning(object message)
    {
        Debug.LogWarning(message);
    }

    [System.Diagnostics.Conditional(DEVLOG_SYMBOL)]
    public static void LogWarning(object message, Object context)
    {
        Debug.LogWarning(message, context);
    }

    [System.Diagnostics.Conditional(DEVLOG_SYMBOL)]
    public static void LogError(object message)
    {
        Debug.LogError(message);
    }

    [System.Diagnostics.Conditional(DEVLOG_SYMBOL)]
    public static void LogError(object message, Object context)
    {
        Debug.LogError(message, context);
    }

    [System.Diagnostics.Conditional(DEVLOG_SYMBOL)]
    public static void LogException(System.Exception exception)
    {
        Debug.LogException(exception);
    }

    [System.Diagnostics.Conditional(DEVLOG_SYMBOL)]
    public static void LogException(System.Exception exception, Object context)
    {
        Debug.LogException(exception, context);
    }
}