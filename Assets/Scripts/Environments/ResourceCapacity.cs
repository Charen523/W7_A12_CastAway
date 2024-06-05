using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceCapacity : MonoBehaviour
{
    // 자원 용량 설정
    public int maxHits = 5;
    private int currentHits;

    private ResourcePoolManager poolManager;

    void Start()
    {
        currentHits = 0;
        poolManager = FindObjectOfType<ResourcePoolManager>();
    }

    // 도끼로 쳤을 때 호출되는 메서드
    public void Hit(GameObject prefab)
    {
        currentHits++;
        if (currentHits >= maxHits)
        {
            poolManager.ReturnObjectToPool(prefab);
            currentHits = 0;
        }
    }

    // 플레이어와 부딪히면 풀로 반환, 5초 후 재생성 메서드
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // 충돌한 프리팹을 풀로 반환하고 재생성 코루틴 시작
            GameObject prefab = gameObject;
            Debug.Log("반환");
            poolManager.ReturnObjectToPool(prefab);
        }
    }
}
