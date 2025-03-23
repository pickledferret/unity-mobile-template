using UnityEngine;
using UnityEditor;
using System.Reflection;

[CustomEditor(typeof(Object), true)]
public class ButtonDrawer : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MethodInfo[] methods = target.GetType().GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        foreach (MethodInfo method in methods)
        {
            object[] attributes = method.GetCustomAttributes(typeof(ButtonAttribute), true);

            if (attributes.Length > 0)
            {
                ButtonAttribute buttonAttribute = (ButtonAttribute)attributes[0];
                string buttonText = !string.IsNullOrEmpty(buttonAttribute.buttonText) ? buttonAttribute.buttonText : method.Name;

                GUILayout.Space(20);
                if (GUILayout.Button(buttonText))
                {
                    method.Invoke(target, null);
                }
                GUILayout.Space(20);
            }
        }
    }
}
