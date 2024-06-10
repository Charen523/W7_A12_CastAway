using UnityEngine;

public class RightHandTool : EquipRightHand
{
    [Header("Resource Gathering")]
    public bool doesGatherResources; //tag, 레이어 등으로 바꾸어 나무는 캐지고 돌은 안캐지는 식의 작업 필요.
    public int gatherAmount; //자원을 얻는 양.
    public float gatherRate; //자원이 나오는 빈도.

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
        if (Physics.Raycast(ray, out hit, attackRange))
        {
            ResourceCapacity resourceCapacity = hit.collider.GetComponent<ResourceCapacity>();
            if (resourceCapacity != null)
            {
                resourceCapacity.Hit();
            }
        }
    }
}
