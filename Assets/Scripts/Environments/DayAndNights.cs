using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class DayAndNight : MonoBehaviour
{
    private float time => GameManager.Instance.time;
    private float fullDayLength;
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
        fullDayLength = GameManager.Instance.fullDayLength;

        temperature = FindObjectOfType<Temperature>();
        if (temperature == null)
        {
            Debug.LogError("Temperature 없음");
        }

        //randomRain = FindObjectOfType<RandomRain>();
        //if (randomRain == null)
        //{
        //    Debug.LogError("비가없음");
        //}
    }

    private void Update()
    {
        UpdateLighting(sun, sunColor, sunIntensity);
        UpdateLighting(moon, moonColor, moonIntensity);

        RenderSettings.ambientIntensity = lightingIntensityMultiplier.Evaluate(time);
        RenderSettings.reflectionIntensity = reflectionIntensityMultiplier.Evaluate(time);
        
        UpdateTemperature();

        //if (randomRain != null)
        //{

        //    if (!sun.gameObject.activeInHierarchy)
        //    {
        //        randomRain.EnableRain();
        //    }
        //    else
        //    {
        //        randomRain.DisableRain();
        //    }
        //}
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
            float targetTemperature = 0;

            if (time > 0f && time <= 0.05f) // 밤: 혹한.
            {
                targetTemperature = Random.Range(0, 15);
            }
            else if (time > 0.05f && time <= 0.25f)
            {
                targetTemperature = Random.Range(15, 50);
            }
            else if (time > 0.25f && time <= 0.5f) // 아침: 따뜻함.
            {
                targetTemperature = Random.Range(50, 85);
            }
            else if (time > 0.5f && time <= 0.55f) // 정오: 무더위.
            {
                targetTemperature = Random.Range(85, 100);
            }
            else if (time > 0.55f && time <= 0.75f) // 오후: 따뜻함.
            {
                targetTemperature = Random.Range(50, 85);
            }
            else if (time > 0.75f && time <= 1f) // 저녁: 쌀쌀함.
            {
                targetTemperature = Random.Range(15, 50);
            }

            // 온도가 목표 온도에 점진적으로 도달하도록 변경 속도를 조절.
            //인게임 1시간동안 체온 변화가 이루어짐.
            float temperatureChangeRate = Mathf.Log(0.01f) / (1 / fullDayLength * Time.deltaTime); 
            float currentTemperature = temperature.GetCurrentValue();
            float newTemperature = Mathf.Lerp(currentTemperature, targetTemperature, temperatureChangeRate * Time.deltaTime);

            temperature.ChangeValue(newTemperature - currentTemperature);
        }
    }

}

