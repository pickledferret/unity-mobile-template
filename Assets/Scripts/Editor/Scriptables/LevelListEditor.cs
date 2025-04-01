using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;

[CustomEditor(typeof(LevelList))]
public class LevelListEditor : Editor
{
    private SerializedProperty m_scenes;
    private SerializedProperty m_loopingLevels;
    private SerializedProperty m_elementToLoopBackTo;

    private void OnEnable()
    {
        m_scenes = serializedObject.FindProperty("scenes");
        m_loopingLevels = serializedObject.FindProperty("loopingLevels");
        m_elementToLoopBackTo = serializedObject.FindProperty("elementToLoopBackTo");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.LabelField("Level Configuration", EditorStyles.boldLabel);
        EditorGUILayout.HelpBox("Assign your level scenes to be played in order.", MessageType.Info);
        EditorGUILayout.PropertyField(m_scenes, new GUIContent("Level List"));

        EditorGUILayout.Space(20);

        EditorGUILayout.LabelField("Looping Options", EditorStyles.boldLabel);
        EditorGUILayout.HelpBox("Set to True to restart the level sequence from a specified point after the final level is completed. (For endless gameplay)\n\n" +
                                "Set to False to display a final [Game Complete] screen instead, resetting the game state & stats to the start.", MessageType.Info);
        EditorGUILayout.PropertyField(m_loopingLevels);

        if (m_loopingLevels.boolValue)
        {
            EditorGUILayout.Space(20);

            EditorGUILayout.LabelField("What element in the level list above do you want to loop back to?", EditorStyles.label);

            EditorGUILayout.PropertyField(m_elementToLoopBackTo, new GUIContent("Element (Index):"));

            if (m_elementToLoopBackTo.intValue >= 0 && m_elementToLoopBackTo.intValue < m_scenes.arraySize)
            {
                SerializedProperty sceneElement = m_scenes.GetArrayElementAtIndex(m_elementToLoopBackTo.intValue);
                SceneAsset sceneAsset = sceneElement.objectReferenceValue as SceneAsset;

                if (sceneAsset != null)
                {
                    EditorGUILayout.HelpBox($"The game will loop back to **{sceneAsset.name}** (Element {m_elementToLoopBackTo.intValue}) in the Level List after the final level.", MessageType.Info);
                }
                else
                {
                    EditorGUILayout.HelpBox($"Warning: Unknown Scene at Element: {m_elementToLoopBackTo.intValue}. Remove this element or assign a Scene.", MessageType.Error);
                }
            }
            else
            {
                EditorGUILayout.HelpBox($"Invalid Index! Please ensure the index is between 0 and {m_scenes.arraySize - 1}.", MessageType.Error);
            }
        }
        else
        {
            EditorGUILayout.Space(20);

            EditorGUILayout.HelpBox("The game will end after the final level. Displaying a 'Game Complete' screen.", MessageType.Info);
        }

        EditorGUILayout.Space(40);

        GUI.backgroundColor = Color.green;
        if (GUILayout.Button("Add Scenes to Build Settings"))
        {
            AddScenesListToBuildSettings();
        }

        EditorGUILayout.Space(10);

        GUI.backgroundColor = Color.yellow;
        if (GUILayout.Button("Open Build Settings"))
        {
            OpenBuildSettings();
        }

        serializedObject.ApplyModifiedProperties();
    }

    void AddScenesListToBuildSettings()
    {
        EditorBuildSettingsScene[] original = EditorBuildSettings.scenes;
        List<EditorBuildSettingsScene> newSettings = new List<EditorBuildSettingsScene>(original);

        bool newScenesAddedToBuildSettings = false;

        for (int i = 0; i < m_scenes.arraySize; i++)
        {
            SerializedProperty sceneElement = m_scenes.GetArrayElementAtIndex(i);
            SceneAsset scene = sceneElement.objectReferenceValue as SceneAsset;

            if (scene != null)
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
        }

        if (!newScenesAddedToBuildSettings)
        {
            Devlog.Log("All scenes already added to Build Settings > Scene List.");
        }

        EditorBuildSettings.scenes = newSettings.ToArray();
    }

    void OpenBuildSettings()
    {
        EditorApplication.ExecuteMenuItem("File/Build Profiles");
    }

}
