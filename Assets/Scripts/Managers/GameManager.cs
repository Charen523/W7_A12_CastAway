using System;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    /*events*/
    public event Action OnPause;
    public event Action OnResume;
    public event Action BecomeMorning;

    [Header("Time")]
    [Range(0f, 1f)] public float time;
    public int day = 0;
    public float startTime = 0.25f; //오전 6시.
    public float fullDayLength = 300;
    private float timeRate;

    /*Player*/
    private PlayerCondition playerCondition;

    [Header("Game Status")]
    public bool isPause;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        OnPause += Pause;
        OnResume += Resume;
        BecomeMorning += TimeToMorning;
        
        timeRate = 1 / fullDayLength;
        time = startTime;
        Time.timeScale = 1f;

        playerCondition = CharacterManager.Instance.Player.condition;
        playerCondition.OnDeath += PlayerDeath;
    }

    private void Update()
    {
        float beforeTime = time;
        time = (time + timeRate * Time.deltaTime) % 1.0f;

        if (beforeTime > time)
            day++;
    }

    public void OnPauseEvent()
    {
        OnPause?.Invoke();
    }

    private void Pause()
    {
        Time.timeScale = 0f;
    }

    public void OnResumeEvent()
    {
        OnResume?.Invoke();
    }

    private void Resume()
    {
        Time.timeScale = 1f;
    }

    private void PlayerDeath()
    {
        Time.timeScale = 0f;
    }

    public void OnSleep()
    {
        BecomeMorning?.Invoke();
    }

    private void TimeToMorning()
    {
        time = 0.2499f;
        day++;
    }
 }
