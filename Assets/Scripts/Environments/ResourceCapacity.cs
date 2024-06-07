using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

    // 플레이어와 충돌한 자원이 당근, 버섯인지 확인
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameObject prefab = gameObject;
            if (prefab.tag == "B1010"|| prefab.tag == "B1007" || prefab.tag == "B1008" || prefab.tag == "B1009")
            {
                poolManager.isCrop = true;
            }
        }
    }
}
