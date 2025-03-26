using System;
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
    public void FullFade(float fadeOutTime, Action onBlackScreenCallback, float delayBeforeFadeIn, float fadeInTime, Action fadeCompleteCallback, Color color = default)
    {
        StartCoroutine(FadeSequence(fadeOutTime, onBlackScreenCallback, delayBeforeFadeIn, fadeInTime, fadeCompleteCallback, color));
    }

    /// <summary>
    /// Fades the screen from transparent to black. Destroys self after completion.
    /// </summary>
    public void FadeOut(float duration, Action onComplete = null, bool closePopUpAfterCompletion = true, Color color = default)
    {
        StartCoroutine(Fade(0f, 1f, duration, onComplete, closePopUpAfterCompletion, color));
    }

    /// <summary>
    /// Fades the screen from black to transparent. Destroys self after completion.
    /// </summary>
    public void FadeIn(float duration, Action onComplete = null, bool closePopUpAfterCompletion = true, Color color = default)
    {
        Color startColor;
        startColor = (color == default) ? m_image.color : color;
        startColor.a = 1f;
        m_image.color = startColor;

        StartCoroutine(Fade(1f, 0f, duration, onComplete, closePopUpAfterCompletion, color));
    }

    private IEnumerator Fade(float startAlpha, float targetAlpha, float duration, Action onComplete, bool closePopUpAfterCompletion = true, Color color = default)
    {
        Color startColor = m_image.color;
        startColor = (color == default) ? m_image.color : color;
        startColor.a = startAlpha;

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / duration);
            m_image.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            transform.SetAsLastSibling();
            yield return null;
        }

        m_image.color = new Color(m_image.color.r, m_image.color.g, m_image.color.b, targetAlpha);

        onComplete?.Invoke();

        if (closePopUpAfterCompletion)
        {
            ClosePopUp();
        }
    }

    private IEnumerator FadeSequence(float fadeOutTime, Action onBlackScreenCallback, float delayBeforeFadeIn, float fadeInTime, Action fadeCompleteCallback, Color color = default)
    {
        yield return Fade(0f, 1f, fadeOutTime, onBlackScreenCallback, false, color);

        float time = 0f;
        while (time < delayBeforeFadeIn)
        {
            time += Time.deltaTime;
            transform.SetAsLastSibling();
            yield return null;
        }

        yield return Fade(1f, 0f, fadeInTime, fadeCompleteCallback, true, color);
    }
}
