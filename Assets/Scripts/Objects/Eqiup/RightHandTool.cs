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

    }

    public void GatherResources()
    {
        float cameraPlayerDistance = Vector3.Distance(_camera.transform.position, transform.position);

        Ray ray = _camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0)); // 스크린의 정 중앙으로 Ray를 쏨
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, attackRange+ cameraPlayerDistance)) // 공격 범위 내의 객체 정보가 있다면
        {
            if (doesGatherResources && hit.collider.TryGetComponent(out ResourceCapacity resource)) // 장비가 자원채취용일때 + 콜라이더에 닿은 오브젝트가 자원일때
            {
                //resource.Gather(hit.point, hit.normal);  // Gather 실행
            }
        }
    }
}
