#if UNITY_IOS && !UNITY_EDITOR
using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
#endif

using static Haptics;

public static class iOS
{
#if UNITY_IOS && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern bool _IsHapticsSupported();

    // A single native function that takes an integer parameter
    [DllImport("__Internal")]
    private static extern void _PlayHaptic(int hapticType);
#endif

    public static bool IsHapticsSupported()
    {
#if UNITY_IOS && !UNITY_EDITOR
        return _IsHapticsSupported();
#else
        return false;
#endif
    }

    public static void PlayHaptic(Haptics.HapticType hapticType)
    {
#if UNITY_IOS && !UNITY_EDITOR
    if (IsHapticsSupported())
    {
        _PlayHaptic((int)hapticType);
    }
#else
        Devlog.Log($"Haptics not supported on this device. Simulating [{hapticType.ToString()}] haptic.");
#endif
    }

    public static void PlayHapticBurst(long duration, Haptics.HapticType hapticType)
    {
#if UNITY_IOS && !UNITY_EDITOR
        if (IsHapticsSupported())
        {
            float totalDuration = duration / 1000f;
            float interval = 0.1f;
            int count = Mathf.CeilToInt(totalDuration / interval);

            GameManager.Instance.StartCoroutine(PlayHapticsRepeatedly(hapticType, interval, count));
        }
#else
        Devlog.Log($"Haptics not supported on this device. Simulating burst of [{hapticType.ToString()}] haptics for: {duration}ms.");
#endif
    }

#if UNITY_IOS && !UNITY_EDITOR
    private static IEnumerator PlayHapticsRepeatedly(Haptics.HapticType hapticType, float interval, int count)
    {
        for (int i = 0; i < count; i++)
        {
            _PlayHaptic((int)hapticType);
            yield return new WaitForSeconds(interval);
        }
    }
#endif
}