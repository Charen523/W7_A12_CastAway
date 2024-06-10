using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Entity", menuName = "SO/New Entity", order = 3)]
public class EntityData : ScriptableObject
{
    [Header("Stats")]
    public string Name; //이름
    public int MaxHealth;
    public int CureentHealth; //체력
    public float walkSpeed; //걷기, 달리기 속도
    public float runSpeed;
    public GameObject[] dropOnDeath; //아이템 드롭

    [Header("AI")]
    public eAIState aiState; // Ai 상태 저장
    public float detectDistance; //감지 거리
    public float safeDistance; // 안전 거리

    [Header("Wandering")]
    public float minWanderDistance; // 최소,최대 이동거리
    public float maxWanderDistance;
    public float minWanderWaitTime; // 새로운 목표지점 까지 대기시간 (최소,최대)
    public float maxWanderWaitTime;

    [Header("Combat")]
    public int damage; //데미지
    public float attackRate; // 공격 대기시간
    public float attackDistance; // 공격 거리
    public float chargeDuration = 0.5f; // 캐릭터에게 돌진하는 시간
    public float returnDuration = 0.5f; // 원래 위치로 돌아오는 시간
    public bool isCharging = false;
}

