using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatInfoToggle : MonoBehaviour
{
    private enum eInfoTxts
    {
        HealthTxt,
        StaminaTxt,
        HTTxt = 3,
        TemTxt,
        PauseTxt
    }

    private PlayerController controller;
    private UICondition conditions;
    private float health;
    private float stamina;
    private float hunger;
    private float thirst;
    private float temperature;

    private List<TextMeshProUGUI> infoTxts = new List<TextMeshProUGUI>();
    private Coroutine coroutine;
    

    private void Awake()
    {
        foreach (Transform child in transform)
        {
            infoTxts.Add(child.GetComponent<TextMeshProUGUI>());
        }
    }

    private IEnumerator Start()
    {
        controller = CharacterManager.Instance.Player.controller;
        controller.AltInfoOn += ShowInfo;
        controller.AltInfoOff += HideInfo;

        yield return null;
        conditions = CharacterManager.Instance.Player.condition.uiCondition;
        GetConditionInfo();
        SetConditionInfo();

        gameObject.SetActive(false);
    }

    private void GetConditionInfo()
    {
        health = Mathf.Round(conditions.health.GetPertentage() * 100);
        stamina = Mathf.Round(conditions.stamina.GetPertentage() * 100);
        hunger = Mathf.Round(conditions.hunger.GetPertentage() * 100);
        thirst = Mathf.Round(conditions.thirst.GetPertentage() * 100);
        temperature = Mathf.Round(conditions.temperature.GetPertentage() * 100);
    }

    private void SetConditionInfo()
    {
        infoTxts[(int)eInfoTxts.HealthTxt].text = $"체력\n{health}/{conditions.health.maxValue}";
        infoTxts[(int)eInfoTxts.StaminaTxt].text = $"스테미나\n{stamina}%";
        infoTxts[(int)eInfoTxts.HTTxt].text = $"{hunger}%\n{thirst}%";
        infoTxts[(int)eInfoTxts.TemTxt].text = $"온도 {temperature}%";
    }

    private void ShowInfo()
    {
        gameObject.SetActive(true);
        coroutine = StartCoroutine(UpdateUICoroutine());   
    }

    private void HideInfo()
    {
        gameObject.SetActive(false);
        if (coroutine != null) { StopCoroutine(coroutine); }
    }

    private IEnumerator UpdateUICoroutine()
    {
        while (true)
        {
            GetConditionInfo();
            SetConditionInfo();
            yield return null;
        }
    }
}
