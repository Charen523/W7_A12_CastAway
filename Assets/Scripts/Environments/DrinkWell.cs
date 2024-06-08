using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrinkWell : MonoBehaviour, IInteractable
{
    public ItemData data;

    public void GetInteractPrompt()
    {
        // 갈증을 해소할 수 있습니다. (가까이 다가갔을 때, 화면에 띄울 프롬프트)
    }

    public void ClosePrompt()
    {
        //상호작용 완료시, 판넬 setactive false
    }

    public void OnInteract()
    {
        //inventory에 아이템 넣기.
        CharacterManager.Instance.Player.itemData = data;
        CharacterManager.Instance.Player.addItem?.Invoke();
    }
}
