using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Condition : MonoBehaviour
{
    public float curValue;
    public float maxValue;
    public float startValue;
    public float regenRate;
    public Image uiBar;

    private void Start()
    {
        curValue = startValue;

    }

    private void Update()
    {
        uiBar.fillAmount = GetPerventage();
    }

    public void Add(float amount)
    {
        curValue = Mathf.Min(curValue + amount, maxValue);
    }

    public void Subtract(float amount)
    {
        curValue = Mathf.Max(curValue - amount, 0.0f);
    }

    public float GetPerventage()
    {
        return curValue / maxValue;
    }

}
