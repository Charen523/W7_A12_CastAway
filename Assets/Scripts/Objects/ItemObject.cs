using UnityEngine;

//IInteractable 인터페이스 필요.
//인벤토리 스크립트와 연결되게 수정 필요.
public class ItemObject : MonoBehaviour, IInteractable
{
    public ItemData data;

    public void GetInteractPrompt()
    {
        throw new System.NotImplementedException();
    }

    public void ClosePrompt()
    {
        throw new System.NotImplementedException();
    }

    public void OnInteract()
    {
        //inventory에 아이템 넣기.
        CharacterManager.Instance.Player.addItem?.Invoke();

        // 아이템 오브젝트 파괴
        Destroy(gameObject);
    }

    private void HandleConsumable(ConsumableData consumableData)
    {

    }

    private void HandleEquip(EquipData equipData)
    {

    }

    private void HandleMaterial(ItemData itemData)
    {

    }
}
