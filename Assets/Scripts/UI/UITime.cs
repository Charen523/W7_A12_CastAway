using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UITime : MonoBehaviour
{
    private float time => GameManager.Instance.time;
    private int day => GameManager.Instance.day;

    private Image timeBoard;
    private TextMeshProUGUI dateTxt;

    private void Awake()
    {
        timeBoard = transform.Find("CurrentTime").GetComponent<Image>();
        dateTxt = transform.Find("Date").GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (day % 2 == 0)
        {
            timeBoard.fillClockwise = true;
            timeBoard.fillAmount = time;
        }
        else
        {
            timeBoard.fillClockwise = false;
            timeBoard.fillAmount = 1 - time;
        }

        dateTxt.text = $"Day {day}";
    }
}