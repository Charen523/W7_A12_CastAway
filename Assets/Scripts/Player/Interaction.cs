using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.UI.Image;

public interface IInteractable
{
    public void GetInteractPrompt(); //화면에 띄울 프롬프트 

    public void ClosePrompt();

    public void OnInteract(); // Interact 되었을 시 발동되는 효과 정하기
}

public class Interaction : MonoBehaviour
{
    public float checkRate = 0.05f; // 검출할 빈도 (얼마나 자주 체크할지)
    private float lastCheckTime; // 최근 체크한 시간
    public float maxCheckDistance; // 체크할 최대 거리
    public LayerMask layerMask; // 어떤 레이어가 달린 게임 오브젝트를 추출할 것인지 정하기

    public GameObject curInteractGameObject; // 검출할 오브젝트
    private IInteractable curInteractable; // IInteractable 캐싱


    public Camera camera;

    void Start()
    {
        //camera = Camera.main; //메인 카메라 호출
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - lastCheckTime > checkRate) // 검출 빈도만큼 대기 걸어두기
        {
            lastCheckTime = Time.time; //가장 마지막 체크 타임을 기록

            Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2)); //카메라 기준으로 Ray 발사 스크린 너비 높이 절반(정 중앙으로 발사)
            RaycastHit hit;  // 부딪힌 오브젝트의 정보를 넣을 공간
            Debug.DrawRay(camera.transform.position, new Vector3(Screen.width / 2, Screen.height / 2), UnityEngine.Color.red);

            if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask)) // Ray, 부딪힌 물체, 거리, 레이어마스크
            {
                if (hit.collider.gameObject != curInteractGameObject) // 충돌이 됐을 때 현재 상호작용중인 오브젝트와 다를 경우
                {
                    curInteractGameObject = hit.collider.gameObject; // Ray와 충돌한 오브젝트를 넣어줌
                    curInteractable = hit.collider.GetComponent<IInteractable>(); // 충돌한 오브젝트의 IInteractable 컴포넌트 호출
                    curInteractable.GetInteractPrompt();
                }
            }
            else
            {
                curInteractGameObject = null; //부딪히지 않았다면 초기화
                curInteractable = null;
            }
        }
    }

    public void OnInteractInput(InputAction.CallbackContext context) //Action에 넣을 동작
    {
        if (context.phase == InputActionPhase.Started && curInteractable != null) //인풋액션이 활성화 되었을때, curInteractable이 Null이 아닐 경우
        {
            curInteractable.ClosePrompt();
            curInteractable.OnInteract(); //OnInteract 호출
            curInteractGameObject = null; // 상호작용중인 오브젝트 비우기
            curInteractable = null; // curInteractable이 비우기
        }
    }
}