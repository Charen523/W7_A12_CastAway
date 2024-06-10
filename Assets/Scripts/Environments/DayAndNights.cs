using UnityEngine;

public class DayAndNight : MonoBehaviour
{
    private float time => GameManager.Instance.time;
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

    private Temperature temperature;
    private float targetTemperature = 50;
    private int timeFlag = 0;

    private void Start()
    {
        temperature = FindObjectOfType<Temperature>();
        if (temperature == null)
        {
            Debug.LogError("Temperature 없음");
        }
    }

    private void Update()
    {
        UpdateLighting(sun, sunColor, sunIntensity);
        UpdateLighting(moon, moonColor, moonIntensity);

        RenderSettings.ambientIntensity = lightingIntensityMultiplier.Evaluate(time);
        RenderSettings.reflectionIntensity = reflectionIntensityMultiplier.Evaluate(time);
        
        UpdateTemperature();
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
            if (time > 0.99f)
                timeFlag = 0;

            if (time > 0.75f && timeFlag == 5) // 저녁: 쌀쌀함.
            {
                targetTemperature = Random.Range(25, 50);
                timeFlag++;
            }
            else if (time > 0.55f && timeFlag == 4) // 오후: 따뜻함.
            {
                targetTemperature = Random.Range(50, 75);
                timeFlag++;
            }
            else if (time > 0.49f && timeFlag == 3) // 정오: 무더위.
            {
                targetTemperature = Random.Range(75, 100);
                timeFlag++;
            }
            else if (time > 0.25f && timeFlag == 2) // 아침: 따뜻함.
            {
                targetTemperature = Random.Range(50, 75);
                timeFlag++;
            }
            else if (time > 0.05f && timeFlag == 1) //새벽: 쌀쌀함.
            {
                targetTemperature = Random.Range(25, 50);
                timeFlag++;
            }
            else if (time > 0f && timeFlag == 0) // 밤: 혹한.
            {
                targetTemperature = Random.Range(0, 25);
                timeFlag++;
            }

            // 온도가 목표 온도에 점진적으로 도달하도록 변경 속도를 조절.
            //인게임 1시간동안 체온 변화가 이루어짐.
            float temperatureChangeRate = 0.5f; 
            float currentTemperature = temperature.GetCurrentValue();
            float newTemperature = Mathf.Lerp(currentTemperature, targetTemperature, temperatureChangeRate * Time.deltaTime);

            temperature.ChangeValue(newTemperature - currentTemperature);
        }
    }

}

