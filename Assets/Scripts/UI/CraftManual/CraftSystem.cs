using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftSystem : MonoBehaviour
{
    [System.Serializable]
    public class Craft
    {
        public string tag;
        public GameObject previewPrefab; // 프리뷰 프리팹
    }

    public List<Craft> crafts;

    private GameObject currentPreview; // 현재 프리뷰 오브젝트
    private Material previewMaterial; // 재질

    void Update()
    {
        if(currentPreview != null)
        {
            // 마우스를 따라 이동
            FollowMouse();

            // 건축 가능 여부 확인 및 색상 변경
            //CheckInstallableColor();
        }

        // 좌클릭 및 초록색일 때 실제 오브젝트 생성
        if (Input.GetMouseButtonDown(0) && previewMaterial.color == Color.green)
        {
            PlaceObject();
        }

    }

    // 프리뷰 오브젝트 생성(아이콘 클릭 시)
    void CreatePreviewObject(string selectedTag)
    {
        foreach(var craft in crafts)
        {
            if(craft.tag == selectedTag)
            {
                currentPreview = Instantiate(craft.previewPrefab);
                previewMaterial = currentPreview.GetComponent<Renderer>().material;
                previewMaterial.color = Color.green; // 초기 색상 초록색
            }
        }
    }

    // 프리뷰 오브젝트가 마우스를 따라다니게 함
    void FollowMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            currentPreview.transform.position = hit.point;
        }
    }

    // 건축 가능 여부 확인 및 색상 변경
    void UpdatePreviewColor()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.CompareTag("Ground") && !IsColliding())
            {
                previewMaterial.color = Color.green; // 건축 가능
            }
            else
            {
                previewMaterial.color = Color.red; // 건축 불가능
            }
        }
    }

    // 다른 오브젝트와 충돌 여부 확인
    bool IsColliding()
    {
        Collider[] colliders = Physics.OverlapBox(currentPreview.transform.position, currentPreview.transform.localScale / 2);
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject != currentPreview)
            {
                return true;
            }
        }
        return false;
    }

    // 실제 오브젝트 생성
    void PlaceObject()
    {
        //Instantiate(previewPrefab, currentPreview.transform.position, Quaternion.identity);
    }
}
