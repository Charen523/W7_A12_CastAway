using UnityEngine;

public class EquipRightHand : Equip
{
    [Header("Usage Stats")]
    public float attackRange;
    public float attackDelay;
    private bool IsAttacking;

    [Header("Damage")]
    public bool doesDealDamage;
    public int damage;

    private Animator animator;
    protected Camera _camera;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        _camera = Camera.main;
    }

    public override void OnEquip()
    {
        //장비 장착
    }

    public override void OnUnequip()
    {
        //장착 해제
    }

    public virtual void OnAttackInput()
    {
        if (!IsAttacking)
        {
            IsAttacking = true;
            Invoke("OnCanAttack", attackDelay); //공격 딜레이.
            PerformAttack();
        }
    }

    void OnCanAttack()
    {
        IsAttacking = false;
    }

    protected virtual void PerformAttack()
    {
        // 공격 로직 구현
        // 공격 애니메이션 재생?
    }
}
