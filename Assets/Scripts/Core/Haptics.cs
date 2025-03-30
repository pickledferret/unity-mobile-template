public static class Haptics
{
    public enum HapticType
    {
        Light = 0,
        Medium = 1,
        Heavy = 2,
        Success = 3,
        Warning = 4,
        Error = 5
    }

    /// <summary>
    /// Play haptic feedback of the specified type.
    /// </summary>
    /// <param name="type"></param>
    public static void PlayHaptics(HapticType type)
    {
#if UNITY_IOS
        iOS.PlayHaptic(type);
#elif UNITY_ANDROID
        Android.PlayHaptic(type);
#endif
    }

    /// <summary>
    /// Play haptic-burst feedback for duration in milliseconds.
    /// </summary>
    /// <param name="duration">in milliseconds</param>
    /// <param name="type">Haptic Type (iOS Only).</param>
    public static void PlayHapticsBurst(long duration, HapticType type = HapticType.Light)
    {
#if UNITY_IOS
        iOS.PlayHapticBurst(duration, type);
#elif UNITY_ANDROID
        Android.PlayHapticBurst(duration);
#endif
    }

}