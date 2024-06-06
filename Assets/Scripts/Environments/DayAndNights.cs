using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class DayAndNight : MonoBehaviour
{
    [Range(0f, 1f)] public float time;
    public float fullDayLength;
    public float startTime = 0.4f; //0.4f = 9시 36분
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

    private RandomRain randomRain;
    private Temperature temperature;

    private void Start()
    {
        timeRate = 0.005f; //(fullDayLength * 2);
        time = startTime;


        randomRain = FindObjectOfType<RandomRain>();
        if (randomRain == null)
        {
            //Debug.LogError("비가없음");
        }

        temperature = FindObjectOfType<Temperature>();
        if (temperature == null)
        {
            Debug.LogError("Temperature 없음");
        }

    }

    private void Update()
    {
        time = (time + timeRate * Time.deltaTime) % 1.0f;
        UpdateLighting(sun, sunColor, sunIntensity);
        UpdateLighting(moon, moonColor, moonIntensity);

        RenderSettings.ambientIntensity = lightingIntensityMultiplier.Evaluate(time);
        RenderSettings.reflectionIntensity = reflectionIntensityMultiplier.Evaluate(time);
        UpdateTemperature();

        if (randomRain != null)
        {

            if (!sun.gameObject.activeInHierarchy)
            {
                randomRain.EnableRain();
            }
            else
            {
                randomRain.DisableRain();
            }
        }

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

    private void UpdateTemperature()
    {
        if (temperature != null)
        {
            float targetTemperature = 0.0f;

            // 정오(12시)에 온도가 90이 되도록 설정
            if (time >= 0.5f && time <= 0.65f)
            {
                targetTemperature = 90.0f;
            }
            // 밤 시간대 (0.85 <= time < 1.0 또는 0 <= time < 0.25)에 온도가 20 이하가 되도록 설정
            else if (time >= 0.9f || time < 0.25f)
            {               
                targetTemperature = 15.0f;
            }
            // 그 외 시간대에는 온도가 40에서 60 사이에서 변화하도록 설정
            else //if (time > 0.25f && time < 0.89f)
            {
                targetTemperature = Mathf.Lerp(40.0f, 60.0f, Mathf.PingPong((time - 0.25f) * 4.0f, 1.0f));
            }

            // 온도가 목표 온도에 점진적으로 도달하도록 변경 속도를 조절.
            float temperatureChangeRate = 0.05f; // 온도가 천천히 변화하는 속도
            float currentTemperature = temperature.GetCurrentValue();
            float newTemperature = Mathf.Lerp(currentTemperature, targetTemperature, temperatureChangeRate * Time.deltaTime);

            temperature.ChangeValue(newTemperature - currentTemperature);
        }
    }

}

