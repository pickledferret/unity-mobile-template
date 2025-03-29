using UnityEngine;
using TMPro;
using System.Text;

public class DebugConsoleLog : MonoBehaviour
{
    [SerializeField] private TMP_Text m_logText;
    [SerializeField] private CanvasGroup m_canvasGroup;

    public void ShowConsoleLog(bool show)
    {
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
    }

    private void UpdateLogText()
    {
        StringBuilder logBuilder = new();

        foreach (string log in DebugConsoleLogHandler.LogQueue)
        {
            logBuilder.AppendLine(log);
        }

        m_logText.text = logBuilder.ToString();
    }

    public void ClearLogs()
    {
        DebugConsoleLogHandler.LogQueue.Clear();
        m_logText.text = "";

    }
}