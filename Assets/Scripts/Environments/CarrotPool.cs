using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarrotPool : MonoBehaviour
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

    // 당근이 땅 위로 보일 부분의 높이
    public float carrotVisibleHeight = 0.025f;

    // 오브젝트들의 위치 리스트
    private List<Vector3> placedPositions;

    // 최소 거리 설정
    public float minDistance;

    void Start()
    {
        // 오브젝트 풀 초기화
        InitializePool();

        // 배치 오브젝트 위치 리스트 초기화
        placedPositions = new List<Vector3>();

        // 오브젝트 배치
        PlaceObjects();

        minDistance = Random.Range(0.1f, 0.3f);
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
        LayerMask terrainLayerMask = LayerMask.GetMask("Ground");

        foreach (GameObject obj in pool)
        {
            Vector3 randomPosition = Vector3.zero;

            // 랜덤 x, z 위치 설정
            float randomX = Random.Range(positionMin.x, positionMax.x);
            float randomZ = Random.Range(positionMin.z, positionMax.z);

            // Raycast를 시작할 위치 설정 (position.y에서 10f 위)
            float initialY = 10f;

            // 오브젝트의 scale 설정
            Vector3 randomScale = new Vector3(
                Random.Range(scaleMin.x, scaleMax.x),
                Random.Range(scaleMin.y, scaleMax.y),
                Random.Range(scaleMin.z, scaleMax.z)
            );

            obj.transform.localScale = randomScale;

            // Raycast로 지형의 높이 감지
            RaycastHit hit;
            if (Physics.Raycast(new Vector3(randomX, initialY, randomZ), Vector3.down, out hit, Mathf.Infinity, terrainLayerMask))
            {
                // 감지된 위치로 오브젝트 배치 (carrotVisibleHeight을 고려)
                float carrotHeight = randomScale.y;
                randomPosition = new Vector3(randomX + minDistance, hit.point.y + (carrotHeight * carrotVisibleHeight), randomZ + minDistance);
            }
            else
            {
                // Raycast가 실패할 경우 안전장치로 positionMin.y 사용
                randomPosition = new Vector3(randomX + minDistance, positionMin.y + (randomScale.y * carrotVisibleHeight), randomZ + minDistance);
            }

            // 유효한 위치가 찾아지면 오브젝트를 배치하고 위치를 리스트에 추가
            obj.transform.position = randomPosition;
            placedPositions.Add(randomPosition);

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
