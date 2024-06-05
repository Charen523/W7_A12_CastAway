using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NeutralEntities : MonoBehaviour, IDamagable
{
    private NavMeshAgent agent; //NavMeshAgent 불러오기
    private Animator animator;  // animator 호출
    private SkinnedMeshRenderer[] meshRenderers; //SkinnedMeshRenderer 불러오기 (오브젝트가 갖고있는 MeshRenderer)
    private eAIState aiState;
    private EntityData data;

    private float playerDistance; //플레이어와의 거리

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        meshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
    }

    private void Start()
    {
        SetState(eAIState.WANDERING); //시작 Ai 상태 지정 
    }

    private void Update()
    {
        playerDistance = Vector3.Distance(transform.position, CharacterManager.Instance.Player.transform.position);
        //업데이트에선 플레이어와의 거리를 지속적으로 측정

        animator.SetBool("Moving", aiState != eAIState.IDLE); // ai의 상태가 기본 상태가 아니라면 Moving을 true로

        switch (aiState)
        {
            case eAIState.IDLE: //idle과 wandering일경우 PassiveUpdate
                PassiveUpdate();
                break;
            case eAIState.WANDERING:
                PassiveUpdate();
                break;
            case eAIState.FLEEING: //도망칠 경우 FleeingUpdate
                FleeingUpdate();
                break;
        }
    }

    private void SetState(eAIState state) // AI의 상태 세팅
    {
        aiState = state;

        switch (aiState) //switch 문으로 지정 
        {
            case eAIState.IDLE: // Idle일 경우
                agent.speed = data.walkSpeed; //이동속도 = 걷기속도
                agent.isStopped = true; //멈춰있음
                break;
            case eAIState.WANDERING: // 이동 상태일때
                agent.speed = data.walkSpeed;
                agent.isStopped = false; //멈춰있지 않음
                break;
            case eAIState.FLEEING: //도망갈 때
                agent.speed = data.runSpeed; //달려가는 속도
                agent.isStopped = false; // 멈춰있지 않음
                break;
        }

        animator.speed = agent.speed / data.walkSpeed; // animator의 속도 조절 (NPC의 현재 속도/기본 걷기 속도)
    }

    void PassiveUpdate()
    {
        if (aiState == eAIState.WANDERING && agent.remainingDistance < 0.1f) //목표지점 남은 거리가 0.1f보다 작을 경우
        {
            SetState(eAIState.IDLE); //잠시 멈춤
            Invoke("WanderToNewLocation", Random.Range(data.minWanderWaitTime, data.maxWanderWaitTime)); //다음 지점으로 목표를 지정하는 함수를 호출 (최소,최대값 적용)
        }
    }

    void FleeingUpdate()
    {
        if (agent.remainingDistance < 0.1f)
        {
            agent.SetDestination(GetFleeLocation());
        }
        else
        {
            SetState(eAIState.WANDERING);
        }
    }

    void WanderToNewLocation() //랜덤한 좌표를 찍고 이동
    {
        if (aiState != eAIState.IDLE) // idle이 아닐때 적용X (방어코드)
        {
            return;
        }
        SetState(eAIState.WANDERING); //상태를 Wandering 으로 변경
        agent.SetDestination(GetWanderLocation()); // 목표지점을 변경하는 함수 호출
    }

    Vector3 GetFleeLocation()
    {
        NavMeshHit hit;

        NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * data.safeDistance), out hit, data.maxWanderDistance, NavMesh.AllAreas);

        int i = 0;
        while (GetDestinationAngle(hit.position) > 90 || playerDistance < data.safeDistance)
        {

            NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * data.safeDistance), out hit, data.maxWanderDistance, NavMesh.AllAreas);
            i++;
            if (i == 30)
                break;
        }

        return hit.position;
    }

    Vector3 GetWanderLocation() // 이동할 지점 찍는 함수
    {
        NavMeshHit hit;
        // NavMesh - 이동경로중 차단 경로

        NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * Random.Range(data.minWanderDistance, data.maxWanderDistance)), out hit, data.maxWanderDistance, NavMesh.AllAreas);
        // SamplePosition - 일정한 영역 지정 / 이동할 수 있는 경로 중 차단된 경로 / 최고 거리 / layer 필터링
        // onUnitSphere 반지름이 1인 구체영역
        int i = 0;
        while (Vector3.Distance(transform.position, hit.position) < data.detectDistance) // 이동 거리가 감지 거리보다 작을때 계속해서 트라이
        {
            NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * Random.Range(data.minWanderDistance, data.maxWanderDistance)), out hit, data.maxWanderDistance, NavMesh.AllAreas);
            i++;
            if (i == 30)
                break;
        }

        return hit.position;
    }

    float GetDestinationAngle(Vector3 targetPos)
    {
        return Vector3.Angle(transform.position - CharacterManager.Instance.Player.transform.position, transform.position + targetPos);
    }
    public void TakePhysicalDamage(int damageAmount) // 데미지를 받는 로직
    {
        data.health -= damageAmount; //체력 - 데미지
        if (data.health <= 0) //0보다 작거나 같아지면 죽음
            Die();

        StartCoroutine(DamageFlash()); //아니라면 데미지를 받음 (코루틴)
    }

    void Die()
    {
        for (int x = 0; x < data.dropOnDeath.Length; x++) //dropOnDeath 에 있는 드롭 프리펩을 떨어트림(아이템 드랍)
        {
            //Instantiate(data.dropOnDeath[x].dropPrefab, transform.position + Vector3.up * 2, Quaternion.identity); //드롭 위치 지정
        }

        Destroy(gameObject); //몬스터 삭제
    }

    IEnumerator DamageFlash()
    {
        for (int x = 0; x < meshRenderers.Length; x++) //meshRenderers 의 모든 색깔을 변경
            meshRenderers[x].material.color = new Color(1.0f, 0.6f, 0.6f);

        yield return new WaitForSeconds(0.1f); //색을 0.1초만큼 기다렸다가 변경
        for (int x = 0; x < meshRenderers.Length; x++) // 하얀색으로 섬광이 일어남
            meshRenderers[x].material.color = Color.white;
    }
}
