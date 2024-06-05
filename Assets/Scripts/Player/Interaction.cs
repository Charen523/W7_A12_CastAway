using UnityEngine;
using UnityEngine.InputSystem;

public interface IInteractable
{
    public void GetInteractPrompt(); //화면에 띄울 프롬프트 

    public void ClosePrompt();

    public void OnInteract(); // Interact 되었을 시 발동되는 효과 정하기
}

public class Interaction : MonoBehaviour
{
    [Header("Interact Ray")]
    public float checkRate = 0.05f;
    private float lastCheckTime; // 최근 체크한 시간
    public float maxCheckDistance; // 체크할 최대 거리
    public LayerMask layerMask; // 어떤 레이어가 달린 게임 오브젝트를 추출할 것인지 정하기
    private Vector3 rayPos;

    [Header("Ray Object")]
    public GameObject curInteractGameObject; // 검출할 오브젝트
    private IInteractable curInteractable; // IInteractable 캐싱

    private Camera _camera;

    private void Start()
    {
        _camera = Camera.main; //메인 카메라 호출
        rayPos = new Vector3(Screen.width / 2, Screen.height / 2);
    }

    private void Update()
    {
        if (Time.time - lastCheckTime > checkRate) //레이 빈도.
        {
            lastCheckTime = Time.time;

            Ray ray = _camera.ScreenPointToRay(rayPos);
            Debug.DrawRay(ray.origin, rayPos * maxCheckDistance, Color.red); //레이 그리기.
            
            if (Physics.Raycast(ray, out RaycastHit hit, maxCheckDistance, layerMask))
            {
                if (hit.collider.gameObject != curInteractGameObject)
                {
                    curInteractGameObject = hit.collider.gameObject;
                    
                    if (hit.collider.gameObject.TryGetComponent(out IInteractable interactable))
                    {
                        curInteractable = interactable;
                        curInteractable.GetInteractPrompt();
                    }
                }
            }
            else
            {
                curInteractGameObject = null; //부딪히지 않았다면 초기화
                curInteractable = null;
            }
        }
    }

    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && curInteractable != null) 
        {
            curInteractable.ClosePrompt();
            curInteractable.OnInteract();

            /*초기화*/
            curInteractGameObject = null;
            curInteractable = null;
        }
    }
}