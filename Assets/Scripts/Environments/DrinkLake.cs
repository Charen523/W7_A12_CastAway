using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.EditorTools;
using UnityEngine;

public class DrinkLake : MonoBehaviour, IInteractable
{
    // 자원 용량 설정
    public int maxDrink = 7;
    private int currentDrink;
    private Transform lakeLocation;

    void Start()
    {
        currentDrink = 0;
        lakeLocation = GetComponent<Transform>();
    }

    public void GetInteractPrompt()
    {
        // 물을 마실 수 있는 횟수(가까이 다가갔을 때, 화면에 띄울 프롬프트) ex) 7/7
    }

    public void ClosePrompt()
    {
        //상호작용 완료시, 판넬 setactive false
    }

    public void OnInteract()
    {
        ReduceLakeAmount();
    }

    public void ReduceLakeAmount()
    {
        float maxValue = CharacterManager.Instance.Player.condition.GetThirstMaxValue();
        if (currentDrink < maxDrink)
        {
            currentDrink++;

            // 갈증 해소
            CharacterManager.Instance.Player.condition.Drink(maxValue);

            // 호수 물 줄어듬
            lakeLocation.position = new Vector3(transform.position.x, transform.position.y - (0.1f * 7 / maxDrink), transform.position.z);
        }
        else
            Destroy(gameObject); // 호수 삭제
    }
}
