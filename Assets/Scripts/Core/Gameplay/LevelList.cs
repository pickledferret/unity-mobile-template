using UnityEngine;
using System.Collections.Generic;
using Unity.Collections;


#if UNITY_EDITOR
using UnityEditor;
using System.Linq;
using System.IO;
#endif

[CreateAssetMenu(fileName = "LevelList", menuName = "Level List", order = 1)]
public class LevelList : ScriptableObject
{
#if UNITY_EDITOR
    [Header("Drag Scenes To Load In Order.")]
    public List<SceneAsset> scenes = new();
#endif

    [Space(20)]

    [HideInInspector]
    [SerializeField] private List<string> m_savedSceneNameOrder = new();

    public List<string> GetSceneNames()
    {
        return m_savedSceneNameOrder;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        UpdateSceneNames();
    }

    public void UpdateSceneNames()
    {
        m_savedSceneNameOrder.Clear();

        foreach (SceneAsset scene in scenes)
        {
            string scenePath = AssetDatabase.GetAssetOrScenePath(scene);
            string sceneName = Path.GetFileNameWithoutExtension(scenePath);
            m_savedSceneNameOrder.Add(sceneName);
        }

        EditorUtility.SetDirty(this);
    }

    [Button("Add Scenes to Build Settings")]
    void AddScenesListToBuildSettings()
    {
        EditorBuildSettingsScene[] original = EditorBuildSettings.scenes;
        List<EditorBuildSettingsScene> newSettings = new List<EditorBuildSettingsScene>(original);

        bool newScenesAddedToBuildSettings = false;

        foreach (SceneAsset scene in scenes)
        {
            string scenePath = AssetDatabase.GetAssetOrScenePath(scene);

            if (!newSettings.Any(s => s.path == scenePath))
            {
                EditorBuildSettingsScene sceneToAdd = new EditorBuildSettingsScene(scenePath, true);
                newSettings.Add(sceneToAdd);
                newScenesAddedToBuildSettings = true;
                Devlog.Log($"Added {scene.name} to Build Settings > Scene List.");
            }
        }

        if (!newScenesAddedToBuildSettings)
        {
            Devlog.Log("All scenes already added to Build Settings > Scene List.");
        }

        EditorBuildSettings.scenes = newSettings.ToArray();
    }
#endif

}