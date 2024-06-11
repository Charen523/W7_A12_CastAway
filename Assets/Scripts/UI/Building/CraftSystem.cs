using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UIElements;

public class CraftSystem : MonoBehaviour
{
    [System.Serializable]
    public class MaterialInfo
    {
        public string matName;
        public int matNumber;
    }

    [System.Serializable]
    public class Craft
    {
        [Header("Info")]
        public string tag;
        public string name;
        public bool isBuilt;

        [Header("Prefab")]
        public GameObject actualPrefab;
        public GameObject previewPrefab;

        [Header("Materials")]
        public string matInfo;
        public MaterialInfo[] needMaterials; // 필요한 재료들
    }
    public List<Craft> crafts;
    public TextMeshProUGUI promptText; // 프롬프트 텍스트
    public GameObject promptPanel; // 프롬프트 패널
    public GameObject oldFence;
    public GameObject oldShip;
    public Material fontMat;
    public Material transparentMat;
    public Material redMat; // 충돌 시 붉은 재질
    public LayerMask groundLayerMask;
    public GameObject InventoryUI;

    private GameObject currentPreview; // 현재 프리뷰 오브젝트
    private Material[] originalMaterials; // 원래 재질들
    private Transform playerTransform;
    public static CraftSystem instance;

    void Awake()
    {
        // 필요한 초기화 작업 수행
        playerTransform = CharacterManager.Instance.Player.transform;
    }

    void OnEnable()
    {
        if(instance != null)
        {
            Destroy(instance);
        }

        instance = this;

        // 플레이어의 트랜스폼 초기화
        playerTransform = CharacterManager.Instance.Player.transform;

        // 프롬프트 패널 비활성화
        promptPanel.SetActive(false);

        // 각 craft 초기화
        Initialize();

        // 각 craft의 actualPrefab 비활성화 및 isBuilt 초기화
        foreach (var craft in crafts)
        {
            if (craft.actualPrefab != null)
            {
                craft.actualPrefab.SetActive(false);
            }
            craft.isBuilt = false;
        }
    }

    public void Initialize()
    {
        foreach (var craft in crafts)
        {
            if (craft.previewPrefab != null)
            {
                craft.previewPrefab.SetActive(true);

                // previewPrefab의 모든 Renderer의 material을 fontMat으로 설정
                Renderer[] renderers = craft.previewPrefab.GetComponentsInChildren<Renderer>();
                foreach (Renderer renderer in renderers)
                {
                    renderer.material = fontMat; // material을 null로 설정하여 보이지 않게 만듦
                }
                if (renderers.Length > 0)
                {
                    for (int i = 0; i < renderers.Length; i++)
                    {
                        renderers[i].material = fontMat;
                    }
                }
            }
        }
    }

    // 설치 메서드
    public void Install(ItemData data)
    {
        InventoryUI.SetActive(false); // 인벤토리 창 비활성화
        CharacterManager.Instance.Player.controller.ToggleCursor();
        currentPreview = Instantiate(DataManager.Instance.itemPrefabDictionary[data.name]);
        Renderer[] renderers = currentPreview.GetComponentsInChildren<Renderer>();

        // 원래 재질 저장
        if (renderers.Length > 0)
        {
            originalMaterials = new Material[renderers.Length];
            for (int i = 0; i < renderers.Length; i++)
            {
                originalMaterials[i] = renderers[i].material;
            }
        }

        foreach (Renderer renderer in renderers)
        {
            renderer.material = transparentMat; // 프리뷰 오브젝트의 재질을 transparentMat으로 변경
        }

        StartCoroutine(FollowAndPlace(data)); // 프리뷰 오브젝트를 따라다니게 하고 배치할 코루틴 시작
    }

    // 설치전 프리뷰 오브젝트 메서드
    private IEnumerator FollowAndPlace(ItemData data)
    {
        while (!Input.GetKeyDown(KeyCode.E))
        {
            FollowCharacter(); // 캐릭터를 따라다니기

            Renderer[] renderers = currentPreview.GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers)
            {
                renderer.material = transparentMat; // transparentMat으로 표시
            }

            yield return null;
        }

        // 최종 위치에 오브젝트 설치
        PlaceFinalObject(data);
    }

    // 캐릭터 위치로 이동 메서드
    private void FollowCharacter()
    {
        Vector3 newPosition = playerTransform.position + playerTransform.forward * 2f;
        newPosition.y = 30f;
        LayerMask terrainLayerMask = LayerMask.GetMask("Ground");
        RaycastHit hit;
        if (Physics.Raycast(newPosition, Vector3.down, out hit, Mathf.Infinity, terrainLayerMask))
        {
            newPosition.y = hit.point.y; // y 위치를 지면에 맞추기
        }

        currentPreview.transform.position = newPosition;
    }

    private void PlaceFinalObject(ItemData data)
    {
        // 최종 위치에 실제 오브젝트 설치
        Vector3 finalPosition = currentPreview.transform.position;
        Quaternion finalRotation = currentPreview.transform.rotation;

        GameObject actualObject = Instantiate(DataManager.Instance.itemPrefabDictionary[data.name], finalPosition, finalRotation);
        Renderer[] finalRenderers = actualObject.GetComponentsInChildren<Renderer>();
        for (int i = 0; i < finalRenderers.Length; i++)
        {
            finalRenderers[i].material = originalMaterials[i];
        }

        // Inventory에서 아이템 제거
        UIInventory inventory = CraftManager.Instance.UIInventory;
        if (inventory != null)
        {
            inventory.RemoveItemByName(data.itemId);
        }
        Destroy(currentPreview); // 프리뷰 오브젝트 삭제
        currentPreview = null; // 현재 프리뷰 오브젝트 초기화
    }
}
