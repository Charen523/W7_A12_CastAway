using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static CraftSystem;



public class CraftSystem : MonoBehaviour
{
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

        [HideInInspector]
        public Vector3 initialLocation;
    }
    public List<Craft> crafts;
    public TextMeshProUGUI promptText;
    public GameObject promptPanel;
    public GameObject oldFence;
    public Material fontMat;
    public Material plantMat;

    void Start()
    {
        promptPanel.SetActive(false);
        Initialize();
        foreach (var craft in crafts)
        {
            craft.actualPrefab.SetActive(false);
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

                // previewPrefab의 모든 Renderer의 material을 null로 설정
                Renderer[] renderers = craft.previewPrefab.GetComponentsInChildren<Renderer>();
                foreach (Renderer renderer in renderers)
                {
                    renderer.material = fontMat; // material을 null로 설정하여 보이지 않게 만듦
                }
            }
        }
    }

    // 프리뷰 오브젝트가 마우스를 따라다니게 함
    //void FollowMouse()
    //{
    //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //    RaycastHit hit;
    //    if (Physics.Raycast(ray, out hit))
    //    {
    //        currentPreview.transform.position = hit.point;
    //    }
    //}

    // 건축 가능 여부 확인 및 색상 변경
    //void UpdatePreviewColor()
    //{
    //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //    RaycastHit hit;
    //    if (Physics.Raycast(ray, out hit))
    //    {
    //        if (hit.collider.gameObject.CompareTag("Ground") && !IsColliding())
    //        {
    //            previewMaterial.color = Color.green; // 건축 가능
    //        }
    //        else
    //        {
    //            previewMaterial.color = Color.red; // 건축 불가능
    //        }
    //    }
    //}

    // 다른 오브젝트와 충돌 여부 확인
    //bool IsColliding()
    //{
    //    Collider[] colliders = Physics.OverlapBox(currentPreview.transform.position, currentPreview.transform.localScale / 2);
    //    foreach (Collider collider in colliders)
    //    {
    //        if (collider.gameObject != currentPreview)
    //        {
    //            return true;
    //        }
    //    }
    //    return false;
    //}
}
