using UnityEngine;
using TMPro;
using System.Text;

public class DebugConsoleLog : MonoBehaviour
{
    [SerializeField] private TMP_Text m_logText;
    [SerializeField] private CanvasGroup m_canvasGroup;

    public void ShowConsoleLog(bool show)
    {
#if DEVLOG
        m_canvasGroup.alpha = show ? 1 : 0;
        m_canvasGroup.blocksRaycasts = show;

        if (show)
        {
            DebugConsoleLogHandler.OnLogReceived += UpdateLogText;
            UpdateLogText();
        }
        else
        {
            DebugConsoleLogHandler.OnLogReceived -= UpdateLogText;
        }
#endif
    }

    private void UpdateLogText()
    {
#if DEVLOG
        StringBuilder logBuilder = new();

        foreach (string log in DebugConsoleLogHandler.LogQueue)
        {
            logBuilder.AppendLine(log);
        }

        m_logText.text = logBuilder.ToString();
#endif
    }

    public void ClearLogs()
    {
#if DEVLOG
        DebugConsoleLogHandler.LogQueue.Clear();
        m_logText.text = "";
#endif
    }
}