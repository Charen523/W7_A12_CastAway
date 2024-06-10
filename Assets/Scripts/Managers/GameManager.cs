using System;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    private void Start()
    {
        if (SceneManager.GetActiveScene().Equals(0))
        {
            Debug.LogError("StartScene으로 게임을 시작할 때는 아직 게임매니저가 존재하면 안되는데!");
        }
        else
        {
            OnPause += Pause;
            OnResume += Resume;
            BecomeMorning += TimeToMorning;

            playerCondition = CharacterManager.Instance.Player.condition;
            playerCondition.OnDeath += PlayerDeath;
        }
    }
    void OnEnable()
    {
        // 씬 로드 이벤트에 대한 리스너 추가
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        timeRate = 1 / fullDayLength;
        time = startTime;
        Time.timeScale = 1f;
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
