using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Condition : MonoBehaviour
{
    [Header("Condition Values")]
    public float startValue;
    public float maxValue;
    public float curValue;
    public float deltaRate;

    private Image uiBar;

    private void Awake()
    {
        
    }

    private void Start()
    {
        curValue = startValue;
    }

    private void Update()
    {
        uiBar.fillAmount = GetPertentage();
    }

    public void ChangeValue(float amount)
    {
        curValue = Mathf.Clamp(curValue + amount, 0, float.MaxValue);
    }

    public void Add(float amount)
    {
        curValue = Mathf.Min(curValue + amount, maxValue);
    }

    public void Subtract(float amount)
    {
        curValue = Mathf.Max(curValue - amount, 0.0f);
    }

    public float GetPertentage()
    {
        return curValue / maxValue;
    }

}
