using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathUIController : MonoBehaviour
{
    public CanvasGroup deathUICanvasGroup;
    public float fadeDuration = 1.0f;


    void Start()
    {
        deathUICanvasGroup.alpha = 0f;
    }

    public void ShowDeathUI()
    {
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            deathUICanvasGroup.alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            yield return null;
        }
        deathUICanvasGroup.alpha = 1.0f;
    }
}
