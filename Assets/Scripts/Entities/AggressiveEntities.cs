using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.AI;

public class AggressiveEntities : MonoBehaviour, IDamagable
{
    public EntityData data;
    private NavMeshAgent agent; //NavMeshAgent 불러오기
    private Animator animator;  // animator 호출
    private SkinnedMeshRenderer[] meshRenderers; //SkinnedMeshRenderer 불러오기 (오브젝트가 갖고있는 MeshRenderer)
    private eAIState aiState;

    private int CurrentHealth;

    private float playerDistance; //플레이어와의 거리

    private EntitiesPoolManager poolManager;


    public float lastAttackTime = 0; //최근 공격한 시간
    public float fieldOfView = 120f; //시야각 (120도)

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        meshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        poolManager = EntitiesPoolManager.Instance;
    }

    private void Start()
    {
        SetState(eAIState.WANDERING); //시작 Ai 상태 지정 

        CurrentHealth = data.MaxHealth;
    }

    private void Update()
    {
        playerDistance = Vector3.Distance(transform.position, CharacterManager.Instance.Player.transform.position);
        //업데이트에선 플레이어와의 거리를 지속적으로 측정

        animator.SetBool("Moving", aiState != eAIState.IDLE); // ai의 상태가 기본 상태가 아니라면 Moving을 true로

        if (CurrentHealth < data.MaxHealth / 10) //체력이 소진되면 도망치게 만듬(테스트 필요)
        {
            SetState(eAIState.FLEEING);
        }

        switch (aiState)
        {
            case eAIState.IDLE: //idle과 wandering일경우 PassiveUpdate
                PassiveUpdate();
                break;
            case eAIState.WANDERING:
                PassiveUpdate();
                break;
            case eAIState.ATTACKING: // 공격중일 경우 
                AttackingUpdate();
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
            case eAIState.ATTACKING: //공격 중일떄
                agent.speed = data.runSpeed; // 달리기 속도
                agent.isStopped = false; // 멈춰있지 않음
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

        if (playerDistance < data.detectDistance) //플레이어가 감지 거리안에서 발견되면
        {
            SetState(eAIState.ATTACKING); //공격함
        }
    }

    void AttackingUpdate()
    {
        if (playerDistance > data.attackDistance || !IsPlayerInFieldOfView()) //플레이어가 공격거리보다 멀거나 시야에 있지 않다면
        {
            agent.isStopped = false; //멈추는것을 중단
            NavMeshPath path = new NavMeshPath();
            if (agent.CalculatePath(CharacterManager.Instance.Player.transform.position, path)) //경로 안에 플레이어가 존재한다면
            {
                agent.SetDestination(CharacterManager.Instance.Player.transform.position); //플레이어를 쫓아감 
            }
            else
            {
                SetState(eAIState.FLEEING);
            }
        }
        else
        {
            agent.isStopped = true; //멈춰서
            if (Time.time - lastAttackTime > data.attackRate) //공격 대기시간을 계산하여
            {
                lastAttackTime = Time.time;
                StartCoroutine(ChargeAndReturn());
                CharacterManager.Instance.Player.controller.GetComponent<IDamagable>().TakeDamage(data.damage); //IDamagable의 물리 데미지 주는 함수를 호출
                animator.speed = 1; //애니메이터의 속도를 1로 고정
            }
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

    bool IsPlayerInFieldOfView()
    {
        Vector3 directionToPlayer = CharacterManager.Instance.Player.transform.position - transform.position; //목표지점에서 내 방향을 뺌
        float angle = Vector3.Angle(transform.forward, directionToPlayer); // 플레이어와 NPC의 각도
        return angle < fieldOfView * 0.5f; //
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
    public void TakeDamage(int damageAmount) // 데미지를 받는 로직
    {
        CurrentHealth -= damageAmount; //체력 - 데미지
        if (CurrentHealth <= 0) //0보다 작거나 같아지면 죽음
            Die();
        else
        StartCoroutine(DamageFlash()); //아니라면 데미지를 받음 (코루틴)
    }

    void Die()
    {
        for (int x = 0; x < data.dropOnDeath.Length; x++) //dropOnDeath 에 있는 드롭 프리펩을 떨어트림(아이템 드랍)
        {
            Instantiate(data.dropOnDeath[x], transform.position + Vector3.up * 2, Quaternion.identity); //드롭 위치 지정
        }

        poolManager.ReturnObjectToPool(this.gameObject);
        CurrentHealth = data.MaxHealth;
    }

    IEnumerator DamageFlash()
    {
        for (int x = 0; x < meshRenderers.Length; x++) //meshRenderers 의 모든 색깔을 변경
            meshRenderers[x].material.color = new Color(1.0f, 0.6f, 0.6f);

        yield return new WaitForSeconds(0.1f); //색을 0.1초만큼 기다렸다가 변경
        for (int x = 0; x < meshRenderers.Length; x++) // 하얀색으로 섬광이 일어남
            meshRenderers[x].material.color = Color.white;
    }

    IEnumerator ChargeAndReturn()
    {
        Vector3 originalPosition = transform.position;
        data.isCharging = true;

        // 캐릭터의 방향으로 돌진
        Vector3 chargeDirection = (CharacterManager.Instance.Player.transform.position - transform.position).normalized;
        Vector3 chargeTarget = CharacterManager.Instance.Player.transform.position;
        float chargeElapsedTime = 0f;

        while (chargeElapsedTime < data.chargeDuration)
        {
            transform.position = Vector3.Lerp(transform.position, chargeTarget, chargeElapsedTime / data.chargeDuration);
            chargeElapsedTime += Time.deltaTime;
            yield return null;
        }

        // 정확히 캐릭터의 위치로 이동
        transform.position = chargeTarget;

        // 일정 시간 후에 원래 위치로 돌아옴
        float returnElapsedTime = 0f;
        while (returnElapsedTime < data.returnDuration)
        {
            transform.position = Vector3.Lerp(transform.position, originalPosition, returnElapsedTime / data.returnDuration);
            returnElapsedTime += Time.deltaTime;
            yield return null;
        }

        // 정확히 원래 위치로 이동
        transform.position = originalPosition;
        data.isCharging = false;
    }
}
