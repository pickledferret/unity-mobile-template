using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Bootstrap : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField] private SceneAsset m_mainSceneToLoad;
#endif

    [SerializeField] [HideInInspector] private string m_mainSceneString;

    void Start()
    {
        if (string.IsNullOrEmpty(m_mainSceneString))
        {
            Devlog.LogError("[Main Scene] is not set in Bootstrap scene - Load Bootstrap scene, select Bootstrap object and set a Main Scene To Load.");
            return;
        }

        SceneManager.LoadScene(m_mainSceneString, LoadSceneMode.Additive);
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        m_mainSceneString = m_mainSceneToLoad == null ? string.Empty : m_mainSceneToLoad.name;
    }
#endif
}