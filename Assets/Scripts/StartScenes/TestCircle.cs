using UnityEngine;
using UnityEngine.UI;


public class TestCircle : MonoBehaviour
{
    public int maxValue = 100;
    public Image fill;

    private int currentValue;

    void Start()
    {
        currentValue = maxValue / 2; // 초기 fillAmount가 0.5가 되도록 설정
        fill.fillAmount = 0.5f; // 초기 fillAmount를 0.5로 설정
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
        UpdateFillAmount();
    }

    public void Deduct(int i)
    {
        currentValue -= i;

        if (currentValue < 0)
        {
            currentValue = 0;
        }
        UpdateFillAmount();
    }

    private void UpdateFillAmount()
    {
        fill.fillAmount = 0.5f * ((float)currentValue / (maxValue / 2)); // 비율 조정
    }
}
