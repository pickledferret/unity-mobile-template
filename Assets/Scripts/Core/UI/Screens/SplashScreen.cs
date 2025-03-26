using System;
using System.Collections;
using UnityEngine;

public class SplashScreen : MonoBehaviour
{
    [SerializeField] private Animator m_animator;

    public static event Action OnSplashScreenFinished;

    private void Start()
    {
        m_animator.Play("SplashScreen");
        StartCoroutine(DestroyAfterPlay());
    }

    private IEnumerator DestroyAfterPlay()
    {
        float animationLength = m_animator.GetCurrentAnimatorStateInfo(0).length;
        float time = 0f;
        do
        {
            time += Time.deltaTime;
            transform.SetAsLastSibling();
            yield return null;
        }
        while (time < animationLength);

        OnSplashScreenFinished?.Invoke();

        Destroy(gameObject);
    }
}
