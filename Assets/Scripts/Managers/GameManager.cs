using System;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    /*events*/
    public event Action OnPause;
    public event Action OnResume;

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
        
        timeRate = 1 / fullDayLength;
        time = startTime;
        Time.timeScale = 1f;

        playerCondition = CharacterManager.Instance.Player.condition;
        playerCondition.OnDeath += PlayerDeath;
    }

    private void Update()
    {
        time = (time + timeRate * Time.deltaTime) % 1.0f;
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
}
