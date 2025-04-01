using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
using System.IO;
#endif

[CreateAssetMenu(fileName = "LevelList", menuName = "Level List", order = 1)]
public class LevelList : ScriptableObject
{
#if UNITY_EDITOR
    // SceneAsset references are Editor only, so scene asset names are saved to a serialized list in OnValidate()
    public List<SceneAsset> scenes = new();
#endif

    public bool loopingLevels;
    public int elementToLoopBackTo = 0;

    [HideInInspector] [SerializeField] private List<string> m_savedSceneNameOrder = new();

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
#endif

}