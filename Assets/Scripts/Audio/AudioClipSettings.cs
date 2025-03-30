using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AudioClipSettings
{
    public AudioClip audioClip;
    public float volume = 1f;
}

public static class AudioClipSettingsExtensions
{
    public static AudioClipSettings GetRandomClip(this List<AudioClipSettings> clips, AudioClipSettings excludedClip = null)
    {
        if (clips == null || clips.Count == 0)
        {
            Devlog.LogWarning("No audio clips available.");
            return null;
        }

        if (clips.Count == 1 && clips.Contains(excludedClip))
        {
            Devlog.LogWarning("Only one clip available, and it's excluded.");
            return null;
        }

        AudioClipSettings randomTrack;
        do
        {
            randomTrack = clips[Random.Range(0, clips.Count)];
        }
        while (randomTrack == excludedClip);

        return randomTrack;
    }
}
