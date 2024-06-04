using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NeutralEntities : MonoBehaviour , IDamagable
{
    private NavMeshAgent agent; //NavMeshAgent 불러오기
    private Animator animator;  // animator 호출
    private SkinnedMeshRenderer[] meshRenderers; //SkinnedMeshRenderer 불러오기 (오브젝트가 갖고있는 MeshRenderer)

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        meshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
    }

    private void Start()
    {
        SetState(AIState.Wandering); //시작 Ai 상태 지정 
    }

    private void Update()
    {
        playerDistance = Vector3.Distance(transform.position, CharacterManager.Instance.Player.transform.position);
        //업데이트에선 플레이어와의 거리를 지속적으로 측정

        animator.SetBool("Moving", aiState != AIState.Idle); // ai의 상태가 기본 상태가 아니라면 Moving을 true로

        switch (aiState)
        {
            case AIState.Idle: //idle과 wandering일경우 PassiveUpdate
                PassiveUpdate();
                break;
            case AIState.Wandering:
                PassiveUpdate();
                break;
        }
    }

    public void TakePhysicalDamage(int damageAmount)
    {
        throw new System.NotImplementedException();
    }
}
