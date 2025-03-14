using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeToBlackPopUp : PopUpBase
{
    public const string PATH = "Prefabs/UI/PopUps/PopUpFadeToBlack";

    [SerializeField] private Image m_image;

    /// <summary>
    /// Fades the screen to black, holds for a duration, then fades back in. Destroys self after completion.
    /// </summary>
    public void FullFade(float fadeOutTime, System.Action onBlackScreenCallback, float delayBeforeFadeIn, float fadeInTime, System.Action fadeCompleteCallback)
    {
        StartCoroutine(FadeSequence(fadeOutTime, onBlackScreenCallback, delayBeforeFadeIn, fadeInTime, () => {
            fadeCompleteCallback?.Invoke();
            ClosePopUp();
        }));
    }

    /// <summary>
    /// Fades the screen from transparent to black. Destroys self after completion.
    /// </summary>
    public void FadeOut(float duration, System.Action onComplete = null)
    {
        StartCoroutine(Fade(0f, 1f, duration, () => {
            onComplete?.Invoke();
            ClosePopUp();
        }));
    }

    /// <summary>
    /// Fades the screen from black to transparent. Destroys self after completion.
    /// </summary>
    public void FadeIn(float duration, System.Action onComplete = null)
    {
        Color startColor = m_image.color;
        startColor.a = 1f;
        m_image.color = startColor;
        StartCoroutine(Fade(1f, 0f, duration, () => {
            onComplete?.Invoke();
            ClosePopUp();
        }));
    }


    private IEnumerator Fade(float startAlpha, float targetAlpha, float duration, System.Action onComplete)
    {
        Color startColor = m_image.color;
        startColor.a = startAlpha;

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / duration);
            m_image.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            yield return null;
        }

        m_image.color = new Color(m_image.color.r, m_image.color.g, m_image.color.b, targetAlpha);

        onComplete?.Invoke();
    }


    private IEnumerator FadeSequence(float fadeOutTime, System.Action onBlackScreenCallback, float delayBeforeFadeIn, float fadeInTime, System.Action fadeCompleteCallback)
    {
        yield return Fade(0f, 1f, fadeOutTime, onBlackScreenCallback);
        yield return new WaitForSeconds(delayBeforeFadeIn);
        yield return Fade(1f, 0f, fadeInTime, fadeCompleteCallback);
    }
}
