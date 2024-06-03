using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayAndNight : MonoBehaviour
{
    [Range(0f, 1f)] public float time;
    public float fullDayLength;
    public float startTime = 0.4f; //0.4f = 9½Ã 36ºÐ.
    private float timeRate;
    public Vector3 noon;

    [Header("Sun")]
    public Light sun;
    public Gradient sunColor;
    public AnimationCurve sunIntensity;

    [Header("Moon")]
    public Light moon;
    public Gradient moonColor;
    public AnimationCurve moonIntensity;

    [Header("Other Lighting")]
    public AnimationCurve lightingIntensityMultiplier;
    public AnimationCurve reflectionIntensityMultiplier;

    private void Start()
    {
        timeRate = 1.0f / fullDayLength;
        time = startTime;
    }

    private void Update()
    {
        time = (time + timeRate * Time.deltaTime) % 1.0f;
        UpdateLighting(sun, sunColor, sunIntensity);
        UpdateLighting(moon, moonColor, moonIntensity);

        RenderSettings.ambientIntensity = lightingIntensityMultiplier.Evaluate(time);
        RenderSettings.reflectionIntensity = reflectionIntensityMultiplier.Evaluate(time);
    }

    public void UpdateLighting(Light lightSource, Gradient gradient, AnimationCurve curve)
    {
        float intensity = curve.Evaluate(time);

        lightSource.transform.eulerAngles = (time - (lightSource == sun ? 0.25f : 0.75f)) * noon * 4.0f;
        lightSource.color = gradient.Evaluate(time);
        lightSource.intensity = intensity;

        GameObject obj = lightSource.gameObject;
        if (lightSource.intensity == 0 && obj.activeInHierarchy)
            obj.SetActive(false);
        else if (lightSource.intensity > 0 && !obj.activeInHierarchy)
            obj.SetActive(true);
    }
}
