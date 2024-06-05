using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcePoolManager : MonoBehaviour
{
    // 오브젝트 풀에 사용할 프리팹
    public GameObject prefab;

    // 풀의 크기 설정
    public int poolSize = 10;

    // 위치 및 크기 범위 설정
    [Header("Position")]
    public Vector3 positionMin;
    public Vector3 positionMax;

    [Header("Scale")]
    public Vector3 scaleMin;
    public Vector3 scaleMax;

    // 오브젝트 풀 리스트
    private List<GameObject> pool;

    // 오브젝트를 배치할 부모 오브젝트
    public Transform parentFolder;

    void Start()
    {
        // 오브젝트 풀 초기화
        InitializePool();

        // 오브젝트 배치
        PlaceObjects();
    }

    // 오브젝트 풀 초기화 메서드
    void InitializePool()
    {
        pool = new List<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(prefab, parentFolder);
            obj.SetActive(false);
            pool.Add(obj);
        }
    }

    // 오브젝트 배치 메서드
    void PlaceObjects()
    {
        foreach (GameObject obj in pool)
        {
            obj.transform.position = new Vector3(
                Random.Range(positionMin.x, positionMax.x),
                positionMax.y,
                Random.Range(positionMin.z, positionMax.z)
            );


            obj.transform.localScale = new Vector3(
                Random.Range(scaleMin.x, scaleMax.x),
                Random.Range(scaleMin.y, scaleMax.y),
                Random.Range(scaleMin.z, scaleMax.z)
            );

            obj.SetActive(true);
        }
    }

    // 오브젝트 풀로 반환하는 메서드
    public void ReturnObjectToPool(GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.SetParent(parentFolder);
        pool.Add(obj);
    }
}
