using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.EditorTools;
using UnityEngine;

public class DrinkWell : MonoBehaviour, IInteractable
{
    public ItemData data;
    private CraftSystem craftSystem;


    void Start()
    {
        craftSystem = FindObjectOfType<CraftSystem>();
    }

    public void GetInteractPrompt()
    {
        // 가까이 다가갔을 때, 화면에 띄울 프롬프트
        craftSystem.promptPanel.SetActive(true);
        string str = $"갈증을 해소할 수 있습니다.\n[E]를 눌러 물통 획득하기";
        craftSystem.promptText.text = str;

        // 3초 뒤에 ClosePrompt 실행
        StartCoroutine(ClosePromptAfterDelay(3.0f));
    }

    public void ClosePrompt()
    {   //상호작용 완료시, 판넬 setactive false
        craftSystem.promptPanel.SetActive(false);
    }

    private IEnumerator ClosePromptAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ClosePrompt();
    }

    public void OnInteract()
    {
        //inventory에 아이템 넣기.
        CharacterManager.Instance.Player.itemData = data;
        CharacterManager.Instance.Player.addItem?.Invoke();
    }
}
