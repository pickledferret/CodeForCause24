using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeToBlack : MonoBehaviour
{
    [SerializeField] private Image m_image;

    private void Awake()
    {
        GameplayEvents.ScreenFade += OnFadeToBlack;
        m_image.color = new Color(0f, 0f, 0f, 0f);
        m_image.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        GameplayEvents.ScreenFade -= OnFadeToBlack;
    }

    private void OnFadeToBlack(float fadeOutTime, System.Action onBlackScreenCallback, float delayBeforeFadeBackIn, float fadeInTime, System.Action fadeCompleteCallback)
    {
        StartCoroutine(FadeSequence(fadeOutTime, onBlackScreenCallback, delayBeforeFadeBackIn, fadeInTime, fadeCompleteCallback));
    }

    private IEnumerator FadeSequence(float fadeOutTime, System.Action onBlackScreenCallback, float delayBeforeFadeBackIn, float fadeInTime, System.Action fadeCompleteCallback)
    {
        m_image.gameObject.SetActive(true);

        yield return StartCoroutine(Fade(1f, fadeOutTime));
        onBlackScreenCallback?.Invoke();

        yield return new WaitForSeconds(delayBeforeFadeBackIn);

        yield return StartCoroutine(Fade(0f, fadeInTime));
        fadeCompleteCallback?.Invoke();

        m_image.color = new Color(0f, 0f, 0f, 0f);
        m_image.gameObject.SetActive(false);
    }

    private IEnumerator Fade(float targetAlpha, float duration)
    {
        Color currentColor = m_image.color;
        float startAlpha = currentColor.a;

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            float normalizedTime = t / duration;

            float alpha = Mathf.Lerp(startAlpha, targetAlpha, normalizedTime);

            m_image.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);

            yield return null;
        }

        m_image.color = new Color(currentColor.r, currentColor.g, currentColor.b, targetAlpha);
    }
}