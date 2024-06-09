using UnityEngine;

public class RightHandWeapon : EquipRightHand
{
    protected override void PerformAttack()
    {
        base.PerformAttack();

    }

    public void OnHit()
    {
        float cameraPlayerDistance = Vector3.Distance(_camera.transform.position, transform.position);

        Ray ray = _camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0)); // 스크린의 정 중앙으로 Ray를 쏨
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, attackRange+cameraPlayerDistance)) // 공격 범위 내의 객체 정보가 있다면
        {
            if (doesDealDamage && hit.collider.TryGetComponent(out IDamagable damagable)) // 장비가 공격용일때 + 콜라이더에 닿은 오브젝트가 데미지를 넣을 수 있는 오브젝트일때
            {
                damagable.TakeDamage(damage); // TakePhysicalDamage 실행
            }
        }
    }
}