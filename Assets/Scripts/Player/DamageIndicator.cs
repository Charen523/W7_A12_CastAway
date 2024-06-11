using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DamageIndicator : MonoBehaviour
{
    public Image image;
    public float flashSpeed;

    private Coroutine coroutine;

    private void Start()
    {
        CharacterManager.Instance.Player.condition.onTakeDamage += Flash;
    }

    public void Flash()
    {
        if (coroutine == null)
        {
            image.enabled = true;
            image.color = new Color(1f, 105f / 255f, 105f / 255f);
            coroutine = StartCoroutine(FadeIn());
        }
    }

    private IEnumerator FadeIn()
    {
        image.enabled = true;
        float targetAlpha = 100 / 255f;
        float a = 0.0f;

        while (a < targetAlpha)
        {
            a += (targetAlpha / flashSpeed) * Time.deltaTime;
            image.color = new Color(1f, 105f / 255f, 105f / 255f, a);
            yield return null;
        }

        image.color = new Color(1f, 105f / 255f, 105f / 255f, targetAlpha);
        StartCoroutine(FadeAway()); 
    }

    private IEnumerator FadeAway()
    {
        float startAlpha = 100 / 255f;
        float a = startAlpha;

        while (a > 0.0f)
        {
            a -= (startAlpha / flashSpeed) * Time.deltaTime;
            image.color = new Color(1f, 105f / 255f, 105f / 255f, a);
            yield return null;
        }

        image.enabled = false;
        coroutine = null;
    }
}