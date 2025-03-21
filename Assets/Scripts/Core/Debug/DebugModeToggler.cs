using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

public static class DebugModeToggler
{
    static readonly NamedBuildTarget[] s_buildTargets = new NamedBuildTarget[]
    {
            NamedBuildTarget.Standalone,
            NamedBuildTarget.Android,
            NamedBuildTarget.iOS,
            NamedBuildTarget.WebGL
    };

    [MenuItem("Template/Toggle Debug Mode")]
    public static void EnableDebugMode()
    {
        ToggleDevLogSymbol(true);
    }

    [MenuItem("Template/Toggle Shipping Mode")]
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
                    Devlog.Log($"Added {Devlog.DEVLOG_SYMBOL} to {buildTarget.TargetName}");
                    defines = string.IsNullOrEmpty(defines) ? Devlog.DEVLOG_SYMBOL : $"{defines};{Devlog.DEVLOG_SYMBOL}";
                    Devlog.Log($"Added {defines}");
                    PlayerSettings.SetScriptingDefineSymbols(buildTarget, defines);
                }
            }
            else
            {
                if (defines.Contains(Devlog.DEVLOG_SYMBOL))
                {
                    Devlog.Log($"Removed {Devlog.DEVLOG_SYMBOL} from {buildTarget}");
                    defines = defines.Replace(Devlog.DEVLOG_SYMBOL, string.Empty).Replace(";;", ";").Trim(';');
                    PlayerSettings.SetScriptingDefineSymbols(buildTarget, defines);
                }
            }
        }
    }
}