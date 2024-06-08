using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;



public class CraftSystem : MonoBehaviour, IInteractable
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

        [Header("Materials")]
        public string matInfo;
    }

    public List<Craft> crafts;
    public TextMeshProUGUI promptText;
    public GameObject promptPanel;
    

    private GameObject currentPreview; // 현재 프리뷰 오브젝트
    private Material previewMaterial; // 재질

    void Start()
    {
        promptPanel.SetActive(false);
        foreach (var craft in crafts)
        {
            craft.isBuilt = false;
        }
    }

    void Update()
    {
        //if(currentPreview != null)
        //{
            //// 마우스를 따라 이동
            //FollowMouse();

            //// 건축 가능 여부 확인 및 색상 변경
            //CheckInstallableColor();
        //}

        //// 초록색일 때 실제 오브젝트 생성
        //if (previewMaterial.color == Color.green)
        //{
        //    PlaceObject();
        //}

    }

    public void GetInteractPrompt()
    {
        foreach (var craft in crafts)
        {   // 아직 지어지지 않았으면,
            if (!craft.isBuilt)
            {
                CreatePreviewObject(); // 가까이 다가가면 초록색 프리뷰 띄우기
                DisplayPrompt(); // 프롬프트 표시
            }
        }
    }

    // 프리뷰 오브젝트 생성
    void CreatePreviewObject()
    {
        foreach(var craft in crafts)
        {
            if(craft.tag == gameObject.tag)
            {
                currentPreview = Instantiate(craft.actualPrefab);
                previewMaterial = currentPreview.GetComponent<Renderer>().material;
                previewMaterial.color = Color.green; // 초기 색상 초록색
            }
        }
    }

    public void DisplayPrompt()
    {
        foreach (var craft in crafts)
        {
            if (craft.tag == gameObject.tag)
            {
                promptPanel.SetActive(true); // 판넬 setActive true
                string str = $"[E] 키를 눌러 {craft.name} 건설하기\n재료: {craft.matInfo}";// (가까이 다가갔을 때, 화면에 띄울 프롬프트)
                promptText.text = str; // 오브젝트 설명 표시
            }
        }
    }

    public void ClosePrompt()
    {   //상호작용 완료시, 판넬 setActive false
        promptPanel.SetActive(false); 
        promptText.text = "";
    }

    public void OnInteract()
    {
        // 재료가 모두 있으면,
        PlaceObject();
    }

    // 실제 오브젝트 생성
    void PlaceObject()
    {
        foreach (var craft in crafts)
        {
            if (craft.tag == gameObject.tag)
            {
                Instantiate(craft.actualPrefab, currentPreview.transform.position, Quaternion.identity);
                craft.isBuilt = true;
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
