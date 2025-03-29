using UnityEngine;
using TMPro;
using System.Text;

public class DebugConsoleLog : MonoBehaviour
{
    [SerializeField] private TMP_Text m_logText;

    public void ShowLogs(bool show)
    {
        if (show)
        {
            DebugLogHandler.OnLogReceived += UpdateLogText;
            UpdateLogText();
        }
        else
        {
            DebugLogHandler.OnLogReceived -= UpdateLogText;
        }
        
        gameObject.SetActive(show);
    }

    private void UpdateLogText()
    {
        StringBuilder logBuilder = new();

        foreach (string log in DebugLogHandler.LogQueue)
        {
            logBuilder.AppendLine(log);
        }

        m_logText.text = logBuilder.ToString();
    }

    public void ClearLogs()
    {
        DebugLogHandler.LogQueue.Clear();
        m_logText.text = "";

    }
}