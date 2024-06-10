using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Condition : MonoBehaviour
{
    [Header("Condition Values")]
    public eConditionType conditionType;
    public float startValue;
    public float minValue = 0;
    public float maxValue;
    public float curValue;
    public float deltaRate;

    protected Image uiBar;

    private void Awake()
    {
        uiBar = GetComponent<Image>();  
    }

    private void Start()
    {
        curValue = startValue;
    }

    protected virtual void Update()
    {
        uiBar.fillAmount = GetPertentage();
    }

    public virtual void ChangeValue(float amount)
    {
        curValue = Mathf.Clamp(curValue + amount, minValue, maxValue);
    }

    public float GetPertentage()
    {
        return curValue / maxValue;
    }

}
