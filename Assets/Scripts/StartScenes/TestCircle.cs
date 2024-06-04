using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestCircle : MonoBehaviour
{
    public int maxValue;
    public Image fill;

    private int currentValue;


    
    void Start()
    {
        currentValue = maxValue;
        fill.fillAmount = 0.5f;

    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
            Add(10);

        if (Input.GetKeyDown(KeyCode.D))
            Deduct(3);
    }

    public void Add(int i)
    {
        currentValue += i;

        if (currentValue > maxValue)
        {
            currentValue = maxValue;
        }
        fill.fillAmount = (float)currentValue/maxValue;
    }

    public void Deduct(int i)
    {
        currentValue -= i;

        if (currentValue < 0)
        {
            currentValue = 0;
        }
        fill.fillAmount = (float)currentValue / maxValue;
    }

}
