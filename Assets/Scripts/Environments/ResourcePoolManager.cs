using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.UIElements;


public class ResourcePoolManager : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;

        [Header("DropInfo")]
        public string itemName;

        [Header("Position")]
        public Vector3 positionMin;
        public Vector3 positionMax;

        [Header("Scale")]
        public Vector3 scaleMin;
        public Vector3 scaleMax;
    }

    public List<Pool> Pools;
    public Dictionary<string, Queue<GameObject>> PoolDictionary;
    public bool isCrop = false;

    private Transform parentFolder;

    // 오브젝트 리턴 대기 시간
    public float respawnDelay = 10f;

    // 오브젝트 위치를 저장할 딕셔너리
    private Dictionary<GameObject, Vector3> originalPositions = new Dictionary<GameObject, Vector3>();

    private static ResourcePoolManager _instance; // 유일한 인스턴스를 저장할 정적 변수
    public static ResourcePoolManager Instance // 유일한 인스턴스를 반환하는 정적 프로퍼티
    {
        get
        {   // 인스턴스가 없으면 새 게임 오브젝트를 만들어 인스턴스를 추가(방어코드)
            if (_instance == null)
            {
                _instance = new GameObject("ResourcePoolManager").AddComponent<ResourcePoolManager>();
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {   // 인스턴스 초기화
            _instance = this;
        }
        else
        {
            if (_instance == this)
            {   // 중복 인스턴스 제거
                Destroy(gameObject);
            }
        }

        parentFolder = GetComponent<Transform>();
        // 오브젝트 풀 초기화
        InitializePool();
    }

    void Start()
    {
        // 오브젝트 배치
        PlaceObjects();
    }

    // 오브젝트 풀 초기화 메서드
    void InitializePool()
    {
        PoolDictionary = new Dictionary<string, Queue<GameObject>>();
        foreach (var pool in Pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab, parentFolder);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            PoolDictionary.Add(pool.tag, objectPool);
        }
    }

    public GameObject SpawnFromPool(string tag)
    {
        // 애초에 Pool이 존재하지 않는 경우
        if (!PoolDictionary.ContainsKey(tag))
            return null;

        // 제일 오래된 객체를 재활용
        GameObject obj = PoolDictionary[tag].Dequeue();
        PoolDictionary[tag].Enqueue(obj);
        obj.SetActive(true);
        return obj;
    }

    // 오브젝트 배치 메서드
    void PlaceObjects()
    {
        LayerMask terrainLayerMask = LayerMask.GetMask("Ground");

        foreach (var pool in Pools)
        {
            for (int i = 0; i < pool.size; i++)
            {
                Vector3 offset = new Vector3(i * 0.01f, 0, i * 0.001f);
                Vector3 position = new Vector3(
                    Random.Range(pool.positionMin.x, pool.positionMax.x),
                    30f,
                    Random.Range(pool.positionMin.z, pool.positionMax.z)
                );

                GameObject obj = SpawnFromPool(pool.tag);
                if (obj != null)
                {
                    RaycastHit hit;
                    if (Physics.Raycast(position, Vector3.down, out hit, Mathf.Infinity, terrainLayerMask))
                    {
                        // y 위치를 지면에 맞추기
                        position.y = hit.point.y;
                    }
                    else
                    {
                        // 레이캐스트 실패 시 기본 y 위치 사용
                        position.y = pool.positionMin.y;
                    }

                    obj.transform.position = position + offset; // 객체 위치 설정(오프셋 더함)

                    obj.transform.localScale = new Vector3(
                        Random.Range(pool.scaleMin.x, pool.scaleMax.x),
                        Random.Range(pool.scaleMin.y, pool.scaleMax.y),
                        Random.Range(pool.scaleMin.z, pool.scaleMax.z)
                    );

                    originalPositions[obj] = position; // 원래 위치 저장
                }
            }
        }
    }
    // 오브젝트 풀로 반환, 재생성 코루틴 호출하는 메서드
    public void ReturnObjectToPool(GameObject obj)
    {
        obj.SetActive(false);
        isCrop = false;
        StartCoroutine(RespawnObject(obj, respawnDelay));
    }

    // 아이템 드랍 및 재생성 코루틴
    private IEnumerator RespawnObject(GameObject obj, float delay)
    {
        Debug.Log("10초 후 생성");
        yield return new WaitForSeconds(delay);
        obj.SetActive(true);
    }
}
