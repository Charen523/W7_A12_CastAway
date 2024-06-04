using UnityEngine;

//IInteractable 인터페이스 필요.
//인벤토리 스크립트와 연결되게 수정 필요.
public class ItemObject : MonoBehaviour
{
    public ItemData data;

    public void OnObjectInteract()
    {
        // CharacterManager 인스턴스와 관련된 로직
        // CharacterManager.Instance.Player.itemData = data;
        // CharacterManager.Instance.Player.addItem?.Invoke();

        // 아이템 타입에 따라 다른 작업 수행
        switch (data.type)
        {
            case eItemType.CONSUME:
                HandleConsumable(data as ConsumableData);
                break;
            case eItemType.EQUIP:
                HandleEquip(data as EquipData);
                break;
            default:
                HandleMaterial(data);
                break;
        }

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
