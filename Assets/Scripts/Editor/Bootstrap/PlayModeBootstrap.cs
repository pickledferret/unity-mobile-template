#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
public class PlayModeBootstrap
{
    private static string BOOTSTRAP_SCENE = "Assets/Scenes/Bootstrap.unity";
    private static readonly string LOADED_SCENES_KEY = "Loaded_Scenes";

    private static List<string> m_savedScenes = new();

    [InitializeOnLoadMethod]
    private static void Initialize()
    {
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    private static void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        switch (state)
        {
            case PlayModeStateChange.EnteredEditMode:
                // After Play Mode exits, restore previously opened scenes
                OnExitPlayMode();
                break;
            case PlayModeStateChange.ExitingEditMode:
                // Before entering Play Mode, save open scenes and load Bootstrap scene
                OnEnterPlayMode();
                break;
        }
    }

    private static void OnEnterPlayMode()
    {
        if (!EditorApplication.isPlayingOrWillChangePlaymode)
            return;

        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();

        m_savedScenes.Clear();

        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene.IsValid())
            {
                m_savedScenes.Add(scene.path);
            }
        }

        EditorPrefs.SetString(LOADED_SCENES_KEY, JsonUtility.ToJson(new SceneListWrapper(m_savedScenes)));

        for (int i = m_savedScenes.Count - 1; i >= 0; i--)
        {
            EditorSceneManager.CloseScene(SceneManager.GetSceneByPath(m_savedScenes[i]), true);
        }

        EditorSceneManager.OpenScene(BOOTSTRAP_SCENE, OpenSceneMode.Single);
    }

    private static void OnExitPlayMode()
    {
        EditorApplication.delayCall += RestoreScenes;
    }

    private static void RestoreScenes()
    {
        if (!EditorApplication.isPlaying)
        {
            string jsonData = EditorPrefs.GetString(LOADED_SCENES_KEY, "");
            if (!string.IsNullOrEmpty(jsonData))
            {
                SceneListWrapper sceneWrapper = JsonUtility.FromJson<SceneListWrapper>(jsonData);
                m_savedScenes = sceneWrapper.scenes;
            }

            foreach (string scenePath in m_savedScenes)
            {
                if (!string.IsNullOrEmpty(scenePath))
                {
                    EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Additive);
                }
            }

            if (!m_savedScenes.Contains(BOOTSTRAP_SCENE))
            {
                EditorSceneManager.CloseScene(SceneManager.GetSceneAt(0), true);
            }
        }
    }

    [System.Serializable]
    private class SceneListWrapper
    {
        public List<string> scenes;

        public SceneListWrapper(List<string> scenes)
        {
            this.scenes = scenes;
        }
    }
}
#endif