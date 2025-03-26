#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;

public static class TemplateMenu
{

    static readonly NamedBuildTarget[] s_buildTargets = new NamedBuildTarget[]
    {
            NamedBuildTarget.Standalone,
            NamedBuildTarget.Android,
            NamedBuildTarget.iOS,
            NamedBuildTarget.WebGL
    };

    [MenuItem("Template/Clear Player Prefs")]
    public static void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        Devlog.Log("Cleared all player prefs.");
    }

    [MenuItem("Template/Debug/Toggle Debug Mode")]
    public static void EnableDebugMode()
    {
        ToggleDevLogSymbol(true);
    }

    [MenuItem("Template/Debug/Toggle Shipping Mode")]
    public static void DisableDebugMode()
    {
        ToggleDevLogSymbol(false);
    }

    private static void ToggleDevLogSymbol(bool enable)
    {
        foreach (NamedBuildTarget buildTarget in s_buildTargets)
        {
            string defines = PlayerSettings.GetScriptingDefineSymbols(buildTarget);

            if (enable)
            {
                if (!defines.Contains(Devlog.DEVLOG_SYMBOL))
                {
                    defines = string.IsNullOrEmpty(defines) ? Devlog.DEVLOG_SYMBOL : $"{defines};{Devlog.DEVLOG_SYMBOL}";
                    PlayerSettings.SetScriptingDefineSymbols(buildTarget, defines);
                    Debug.Log($"Added {Devlog.DEVLOG_SYMBOL} to: {buildTarget.TargetName}");
                }
            }
            else
            {
                if (defines.Contains(Devlog.DEVLOG_SYMBOL))
                {
                    Devlog.Log($"Removed {Devlog.DEVLOG_SYMBOL} from: {buildTarget.TargetName}");
                    defines = defines.Replace(Devlog.DEVLOG_SYMBOL, string.Empty).Replace(";;", ";").Trim(';');
                    PlayerSettings.SetScriptingDefineSymbols(buildTarget, defines);
                }
            }
        }
    }
}
#endif