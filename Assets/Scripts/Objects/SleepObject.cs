using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SleepObject : MonoBehaviour, IInteractable
{
    private GameObject sleepPrompt;

    private float time;
    private bool sleepable = false;

    private void Awake()
    {
        if (sleepPrompt == null)
        {
            sleepPrompt = GameObject.Find("Sleep");
            
            if (sleepPrompt == null)
            {
                Debug.LogError("SleepPrompt 오브젝트를 찾을 수 없습니다.");
            }
        }
    }

    private void Start()
    {
        CharacterManager.Instance.Player.NearBed += CheckSleepable;

        sleepPrompt.transform.GetChild(0).gameObject.SetActive(false);
    }

    public void CheckSleepable()
    {
        time = GameManager.Instance.time;
        if (time > 0.75f || time < 0.25f)
        {
            sleepable = true;
        }
        else
        {
            sleepable = false;
        }
    }

    public void OnInteract()
    {
        if (sleepable)
        {
            GameManager.Instance.OnSleep();
            sleepable = false;
        }
    }

    public void GetInteractPrompt()
    {
        sleepPrompt.transform.GetChild(0).gameObject.SetActive(true);
    }

    public void ClosePrompt()
    {
        sleepPrompt.transform.GetChild(0).gameObject.SetActive(false);
    }
}