using UnityEngine;

public class RandomRain : MonoBehaviour
{
    public GameObject rainObject; // 비가 포함된 게임 오브젝트
    public float rainInterval = 30f; // 비가 랜덤하게 발생하는 간격
    public float rainDuration = 10f; // 비가 내리는 지속 시간

    private float nextRainTime;
    private bool isRaining = false;

    void Start()
    {
        ScheduleNextRain();
    }

    void Update()
    {
        if (Time.time >= nextRainTime)
        {
            StartRain();
            ScheduleNextRain();
        }
    }
    public void EnableRain()
    {
        isRaining = true;
    }

    public void DisableRain()
    {
        isRaining = false;
        StopRain();
    }
    void StartRain()
    {
        rainObject.SetActive(true);
        Invoke("StopRain", rainDuration);
    }

    void StopRain()
    {
        rainObject.SetActive(false);
    }

    public void ScheduleNextRain()
    {
        nextRainTime = Time.time + Random.Range(rainInterval, rainInterval * 2);
    }
}
