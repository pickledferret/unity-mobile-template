using UnityEngine;
using System;
using System.Collections.Generic;

public static class SavePrefs
{
    [Serializable]
    private class SerializableList<T>
    {
        public List<T> List;
        public SerializableList(List<T> list)
        {
            List = list;
        }
    }

    public static void SaveInt(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
        PlayerPrefs.Save();
    }

    public static int LoadInt(string key, int defaultValue = 0)
    {
        return PlayerPrefs.GetInt(key, defaultValue);
    }

    public static void SaveFloat(string key, float value)
    {
        PlayerPrefs.SetFloat(key, value);
        PlayerPrefs.Save();
    }

    public static float LoadFloat(string key, float defaultValue = 0f)
    {
        return PlayerPrefs.GetFloat(key, defaultValue);
    }

    public static void SaveString(string key, string value)
    {
        PlayerPrefs.SetString(key, value);
        PlayerPrefs.Save();
    }

    public static string LoadString(string key, string defaultValue = "")
    {
        return PlayerPrefs.GetString(key, defaultValue);
    }

    public static void SaveLong(string key, long value)
    {
        SaveString(key, value.ToString());
    }

    public static long LoadLong(string key, long defaultValue = 0L)
    {
        string value = LoadString(key);
        return long.TryParse(value, out long result) ? result : defaultValue;
    }

    public static void SaveBool(string key, bool value)
    {
        SaveInt(key, value ? 1 : 0);
    }

    public static bool LoadBool(string key, bool defaultValue = false)
    {
        return LoadInt(key, defaultValue ? 1 : 0) == 1;
    }

    public static void SaveList<T>(string key, List<T> value)
    {
        string json = JsonUtility.ToJson(new SerializableList<T>(value));
        SaveString(key, json);
    }

    public static List<T> LoadList<T>(string key)
    {
        string json = LoadString(key);
        if (string.IsNullOrEmpty(json))
        {
            return new List<T>();
        }

        SerializableList<T> data = JsonUtility.FromJson<SerializableList<T>>(json);
        return data?.List ?? new List<T>();
    }

    public static bool HasKey(string key)
    {
        return PlayerPrefs.HasKey(key);
    }

    public static void DeleteKey(string key)
    {
        PlayerPrefs.DeleteKey(key);
    }

    public static void ClearAllKeys()
    {
        PlayerPrefs.DeleteAll();
    }
}
