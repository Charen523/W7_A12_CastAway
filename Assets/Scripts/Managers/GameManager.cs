using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [Header("Time")]
    [Range(0f, 1f)] public float time;
    public int day = 0;
    public float startTime = 0.25f; //오전 6시.
    public float fullDayLength = 300;
    private float timeRate;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        timeRate = 1 / fullDayLength;
        time = startTime;
    }

    private void Update()
    {
        time = (time + timeRate * Time.deltaTime) % 1.0f;
    }
}
