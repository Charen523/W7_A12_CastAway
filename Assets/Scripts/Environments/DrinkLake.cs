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
    private CraftSystem craftSystem;
    private Vector3 dropLocation = new Vector3(282.5f, 12.069f, 658.64f); // 드랍 위치 설정

    void Start()
    {
        currentDrink = 0;
        lakeLocation = GetComponent<Transform>();
        craftSystem = FindObjectOfType<CraftSystem>();
    }

    public void GetInteractPrompt()
    {
        if(currentDrink < maxDrink)
        {
            // 물을 마실 수 있는 횟수(가까이 다가갔을 때, 화면에 띄울 프롬프트) ex) 7/7
            craftSystem.promptPanel.SetActive(true);
            string str = $"이용 가능 횟수: {maxDrink - currentDrink}/{maxDrink}\n[E]를 눌러 갈증 해소하기";
            craftSystem.promptText.text = str;

            // 3초 뒤에 ClosePrompt 실행
            StartCoroutine(ClosePromptAfterDelay(3.0f));
        }
    }

    public void ClosePrompt()
    {
        //상호작용 완료시, 판넬 setactive false
        craftSystem.promptPanel.SetActive(false);
    }

    private IEnumerator ClosePromptAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ClosePrompt();
    }

    public void OnInteract()
    {
        ReduceLakeAmount();
    }

    public void ReduceLakeAmount()
    {
        float maxValue = CharacterManager.Instance.Player.condition.GetThirstMaxValue();

        currentDrink++;
        if (currentDrink < maxDrink)
        {
            // 갈증 해소
            CharacterManager.Instance.Player.condition.Drink(maxValue);

            // 호수 물 줄어듬
            lakeLocation.position = new Vector3(transform.position.x, transform.position.y - (0.1f * 7 / maxDrink), transform.position.z);
        }
        else
        {
            // 호수 삭제
            Destroy(gameObject);

            // 메탈 아이템(I0011) 드랍
            GameObject dropItemPrefab;
            if (DataManager.Instance.itemPrefabDictionary.TryGetValue("I0011", out dropItemPrefab))
            {
                for (int i = 0; i < 3; i++)
                {
                    Vector3 offset = new Vector3(i * 0.5f, 0, 0); // 위치 오프셋을 설정하여 아이템이 겹치지 않도록 함
                    Instantiate(dropItemPrefab, dropLocation + offset, Quaternion.identity);
                }
            }
        }
            
    }
}
