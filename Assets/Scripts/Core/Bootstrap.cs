using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrap : MonoBehaviour
{
    void Start()
    {
        SceneManager.LoadScene("MainScene", LoadSceneMode.Additive);
    }
}