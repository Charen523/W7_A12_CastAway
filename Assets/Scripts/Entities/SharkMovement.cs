using System.Security.Cryptography;
using UnityEngine;

public class SharkMovement : MonoBehaviour
{
    public EntityData data;
    private Animator animator;
    private SkinnedMeshRenderer[] meshRenderers;
    private eAIState aiState;

    private float playerDistance;

    public float lastAttackTime = 0; // 최근 공격한 시간
    public float fieldOfView = 360f;
    public float moveSpeed = 20f;

    private Rigidbody rb;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        meshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        SetState(eAIState.IDLE);
    }

    private void Update()
    {
        playerDistance = Vector3.Distance(transform.position, CharacterManager.Instance.Player.transform.position);

        animator.SetBool("Moving", aiState != eAIState.IDLE);

        switch (aiState)
        {
            case eAIState.IDLE:
                IdleUpdate();
                break;
            case eAIState.ATTACKING:
                AttackingUpdate();
                break;
        }
    }

    private void SetState(eAIState state)
    {
        aiState = state;

        switch (aiState)
        {
            case eAIState.IDLE:
                rb.velocity = Vector3.zero;
                break;
            case eAIState.ATTACKING:
                break;
        }
    }

    void IdleUpdate()
    {
        if (playerDistance < data.detectDistance)
        {
            if(CharacterManager.Instance.Player.transform.position.y < 0)
            {
                SetState(eAIState.ATTACKING);
            }
        }
    }

    void AttackingUpdate()
    {
        Vector3 playerPosition = CharacterManager.Instance.Player.transform.position;
        if (playerDistance > data.attackDistance || !IsPlayerInFieldOfView())
        {
            MoveTowards(playerPosition);
        }
        else
        {
            rb.velocity = Vector3.zero;

            if (Time.time - lastAttackTime > data.attackRate)
            {
                lastAttackTime = Time.time;
                CharacterManager.Instance.Player.controller.GetComponent<IDamagable>().TakeDamage(data.damage);
                animator.speed = 1;
            }
        }

        // If player goes out of detect distance, go back to idle
        if (playerDistance > data.detectDistance)
        {
            SetState(eAIState.IDLE);
        }
    }

    void MoveTowards(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        rb.velocity = direction * moveSpeed;
        transform.LookAt(target);
    }

    bool IsPlayerInFieldOfView()
    {
        Vector3 directionToPlayer = CharacterManager.Instance.Player.transform.position - transform.position;
        float angle = Vector3.Angle(transform.forward, directionToPlayer);
        return angle < fieldOfView * 0.5f;
    }
}