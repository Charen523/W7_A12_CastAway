using UnityEngine;

public class RightHandWeapon : EquipRightHand
{
    protected override void PerformAttack()
    {
        base.PerformAttack();
        OnHit();
    }

    public void OnHit()
    {
        float cameraPlayerDistance = Vector3.Distance(_camera.transform.position, transform.position);

        Ray ray = _camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0)); // 스크린의 정 중앙으로 Ray를 쏨
        RaycastHit hit;

        int playerLayer = LayerMask.NameToLayer("Player"); //플레이어 레이어 마스크 등록

        int layerMask = ~(1<< playerLayer); //Player를 제외한 레이어 마스크 생성

        if (Physics.Raycast(ray, out hit, attackRange+cameraPlayerDistance, layerMask)) // 공격 범위 내의 객체 정보가 있다면
        {
            if (doesDealDamage && hit.collider.TryGetComponent(out IDamagable damagable)) // 장비가 공격용일때 + 콜라이더에 닿은 오브젝트가 데미지를 넣을 수 있는 오브젝트일때
            {
                damagable.TakeDamage(damage); // TakePhysicalDamage 실행
            }
        }
    }
}