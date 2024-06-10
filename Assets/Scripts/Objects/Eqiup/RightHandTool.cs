using UnityEngine;

public class RightHandTool : EquipRightHand
{
    [Header("Resource Gathering")]
    public bool doesGatherResources; //tag, 레이어 등으로 바꾸어 나무는 캐지고 돌은 안캐지는 식의 작업 필요.
    public string tag;

    protected override void PerformAttack()
    {
        base.PerformAttack();
        if (doesGatherResources)
        {
            GatherResources();
        }

    }

    void GatherResources()
    {
        // 자원 수집 로직 구현
        Ray ray = _camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;
        int interactableLayer = LayerMask.GetMask("Interactable"); // Interactable 레이어 마스크 생성
        if (Physics.Raycast(ray, out hit, attackRange, interactableLayer))
        {
            ResourceCapacity resourceCapacity = hit.collider.GetComponent<ResourceCapacity>();
            if (resourceCapacity != null)
            {
                resourceCapacity.Hit();
                Debug.Log("Resource hit!");
            }
        }
        else
        {
            Debug.Log("No resource hit.");
        }
    }
}
