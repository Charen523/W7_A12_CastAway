using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ResourcePoolManager : MonoBehaviour
{
    // 오브젝트 풀에 사용할 프리팹
    public GameObject prefab;

    // 풀의 크기 설정
    public int poolSize = 5;

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

    // 오브젝트 리턴 대기 시간
    public float respawnDelay = 10f;

    // 오브젝트 위치를 저장할 딕셔너리
    private Dictionary<GameObject, Vector3> originalPositions = new Dictionary<GameObject, Vector3>();

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
            Vector3 position = new Vector3(
                Random.Range(positionMin.x, positionMax.x),
                positionMax.y,
                Random.Range(positionMin.z, positionMax.z)
            );

            obj.transform.position = position;

            obj.transform.localScale = new Vector3(
                Random.Range(scaleMin.x, scaleMax.x),
                Random.Range(scaleMin.y, scaleMax.y),
                Random.Range(scaleMin.z, scaleMax.z)
            );

            originalPositions[obj] = position; // 위치 저장
            obj.SetActive(true);
        }
    }

    // 오브젝트 풀로 반환하는 메서드
    public void ReturnObjectToPool(GameObject obj)
    {
        obj.SetActive(false);
        //obj.transform.SetParent(parentFolder);
        pool.Add(obj);
        StartCoroutine(ReSpawnObject(obj, respawnDelay));
    }

    // 일정 시간이 지난 후 오브젝트를 재생성하는 코루틴
    private IEnumerator ReSpawnObject(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);

        obj.transform.position = originalPositions[obj]; // 저장된 위치로 복원

        obj.SetActive(true);
        pool.Remove(obj);
    }
}
