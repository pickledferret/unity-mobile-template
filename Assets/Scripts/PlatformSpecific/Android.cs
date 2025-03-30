#if UNITY_ANDROID && !UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
#endif

public static class Android
{
#if UNITY_ANDROID && !UNITY_EDITOR
    public static AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
    public static AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
    public static AndroidJavaObject vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
#endif

#if UNITY_ANDROID && !UNITY_EDITOR
    private static long MapHapticTypeToDuration(Haptics.HapticType hapticType)
    {
        return hapticType switch
        {
            Haptics.HapticType.Light => 50,
            Haptics.HapticType.Medium => 100,
            Haptics.HapticType.Heavy => 200,
            Haptics.HapticType.Success => 150,
            Haptics.HapticType.Warning => 300,
            Haptics.HapticType.Error => 400,
            _ => 0,
        };
    }
#endif
    public static void PlayHaptic(Haptics.HapticType hapticType)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        long vibrationDuration = MapHapticTypeToDuration(hapticType);
        vibrator.Call("vibrate", vibrationDuration);
#else
        Devlog.Log($"Haptics not supported on this device. Simulating [{hapticType.ToString()}] haptic.");
#endif
    }

    public static void PlayHapticBurst(long duration = 100)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        vibrator.Call("vibrate", duration);
#else
        Devlog.Log($"Haptics not supported on this device. Simulating burst of {duration}ms.");
#endif
    }
}