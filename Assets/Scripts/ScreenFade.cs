using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenFade : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 1.2f;

    void Start()
    {
        StartCoroutine(FadeFromBlack());
    }

    IEnumerator FadeFromBlack()
    {
        float t = 0f;
        Color c = fadeImage.color;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            c.a = 1f - (t / fadeDuration);
            fadeImage.color = c;
            yield return null;
        }

        c.a = 0f;
        fadeImage.color = c;
    }
}
