using UnityEngine;

public class ItemObject : MonoBehaviour, IInteractable
{
    public ItemData data;

    public void GetInteractPrompt()
    {
        //추후 주울 수 있는 아이템 옆에 [data.displayName] 띄워주기.
    }

    public void ClosePrompt()
    {
        //판넬 setactive false
    }

    public void OnInteract()
    {
        //inventory에 아이템 넣기.
        CharacterManager.Instance.Player.itemData = data;
        CharacterManager.Instance.Player.addItem?.Invoke();

        Destroy(gameObject);
    }
}
