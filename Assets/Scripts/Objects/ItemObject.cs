using UnityEditor.EditorTools;
using UnityEngine;

//IInteractable 인터페이스 필요.
//인벤토리 스크립트와 연결되게 수정 필요.
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

        //아이템 오브젝트 파괴(당근, 버섯이면 풀로 반환)
        if (ResourcePoolManager.Instance.isCrop)
        {
            ResourcePoolManager.Instance.ReturnObjectToPool(gameObject);
        }
        else
            Destroy(gameObject);
    }
}
