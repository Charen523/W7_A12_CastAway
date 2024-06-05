using System;
using Unity.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")] //변수 위에 구분점으로 타이틀 생성 , 이동에 필요한 값을 생성
    public float moveSpeed;
    private Vector2 curMovementInput;
    public float jumptForce;
    public LayerMask groundLayerMask;
    public float runSpeed;
    public float runStamina;

    [Header("Look")] // 카메라 화면 회전에 필요한 값들을 입력
    public Transform cameraContainer; //카메라를 담을 변수
    public float distance; // 타겟으로부터의 기본 거리
    public float minDistance = 1.0f; // 타겟으로부터의 최소 거리
    public float maxDistance = 5.0f; // 타겟으로부터의 최대 거리
    public float minXLook;
    public float maxXLook; //X축 최소,최대 이동범위
    private float camCurXRot; // Input Action 마우스의 델타값을 저장
    private float camCurYRot;
    public float lookSensitivity; // 회전 민감도

    private Vector2 mouseDelta; //마우스의 델타값

    [HideInInspector]
    public bool canLook = true; // 인벤토리 나올시 화면 고정

    public Action inventory; // 델리게이트 (인벤토리) 생성

    private Rigidbody rigidbody;
    private Animator animator;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>(); //Rigidbody 받아오기
        animator = GetComponentInChildren<Animator>();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // 마우스 커서를 보이지 않게 하기
    }

    //Update보다 자주 호출됨, 프레임 속도가 높다면 프레임마다 여러번 호출,
    //독립된 업데이트이므로 동일한 주기로 업데이트 영향을 받음(Time.Deltatime 필요없음)
    private void FixedUpdate()
    {
        if (animator.GetBool("IsWalk") == true)
        {
            transform.eulerAngles = new Vector3(0, camCurYRot, 0);
            AnimatorAim();
        }
        if (IsGrounded())
        {
            Move(); //물리적 이동
        }
    }

    //프레임마다 한번씩 호출, Update 계산이 끝나면 그 뒤에 실행
    //특정 행동이 완전히 끝난 뒤 실행하고 싶다면 적용 ex)3인칭 카메라가 캐릭터를 따라 움직일시
    private void LateUpdate()
    {
        if (canLook) //canlook이 true일때만 카메라 회전
        {
            CameraLook(); // 카메라 룩은 LateUpdate에 저장
        }
    }

    public void OnLookInput(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>(); // 마우스 델타에 Vector2값을 읽어옴
    }

    public void OnMoveInput(InputAction.CallbackContext context) //이동 
    {
        if (context.phase == InputActionPhase.Performed) //분기점 인풋액션이 시작되었을 때 Start를 쓰지 않는 이유 - 입력값을 받았을 때만 행동하기 때문
        {
            curMovementInput = context.ReadValue<Vector2>(); //값을 읽어옴 (Vectro2)
            animator.SetBool("IsWalk", true);
        }
        else if (context.phase == InputActionPhase.Canceled) //키가 떨어졌을 때 (취소되었을때)
        {
            curMovementInput = Vector2.zero; // Vector를 초기화
            if (animator.GetBool("IsRun") == true)
            {
                animator.SetBool("IsRun", false);
                CancelInvoke("SubtractStamina");
            }
            animator.SetBool("IsWalk", false);
        }
    }

    public void OnJumpInput(InputAction.CallbackContext context) //점프 받아오기
    {
        if (context.phase == InputActionPhase.Started && IsGrounded()) //버튼이 눌렸을 때 , 땅바닥에 있을 때 두 가지 모두 만족하는 경우 점프 액션이 동작하도록 설정
        {
            rigidbody.AddForce(Vector2.up * jumptForce, ForceMode.Impulse); //순간적으로 힘을 받아야 하기 때문에 Impulse를 사용
            animator.SetTrigger("IsJump");
        }
    }

    public void OnRunInput(InputAction.CallbackContext context)
    {
        if (animator.GetBool("IsRun") == true)
        {
            animator.SetBool("IsRun", false);
            CancelInvoke("SubtractStamina");
        }
        else if (animator.GetBool("IsRun") == false)
        {
            animator.SetBool("IsRun", true);
            InvokeRepeating("SubtractStamina", 0, 0.1f);
        }
    }

    public void OnInventoryInput(InputAction.CallbackContext context) //인벤토리 버튼 동작
    {
        if (context.phase == InputActionPhase.Started) // 버튼이 눌렸다면
        {
            inventory?.Invoke(); //UI인벤토리에 있는 기능 사용
            ToggleCursor();
        }
    }

    public void OnSettingInput(InputAction.CallbackContext context)
    {

    }
    private void Move() //실제로 이동을 시키는 로직
    {
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x; // 입력된 벡터값의 방향 X,y값 설정
        if (animator.GetBool("IsRun") == true)
        {
            dir *= runSpeed; //달리기 속도 곱하기
        }
        else
        {
            dir *= moveSpeed; //이동속도를 곱하기
        }
        dir.y = rigidbody.velocity.y; // 점프했을 때만 Y축 영향을 받아야 하기 때문에 velocity 값으로 고정

        rigidbody.velocity = dir; // 세팅값을 Velociy에 입력
    }

    void CameraLook() //카메라 회전을 시키는 로직
    {
        camCurXRot -= mouseDelta.y * lookSensitivity; //돌려줄 델타값을 민감도와 곱하여 저장 -Y값을 X에 더하는 이유 X축을 돌리려면 마우스의 Y값이 필요함
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook); // 최대값 최소값을 지정해주는 함수 Mathf.Clamp
        camCurYRot += mouseDelta.x * lookSensitivity;

        Vector3 CamtoPlayer = new Vector3 (transform.position.x,transform.position.y+1,transform.position.z); //카메라가 캐릭터를 관찰하는 위치 지정

        // 회전 각도에 따라 카메라 위치 계산
        Quaternion rotation = Quaternion.Euler(camCurXRot, camCurYRot, 0);
        Vector3 desiredPosition = CamtoPlayer - (rotation * Vector3.forward * distance);

        // 카메라와 타겟 사이의 방향 벡터 계산
        Vector3 direction = (desiredPosition - transform.position).normalized;
        //Debug.Log(desiredPosition);
        //Debug.Log(direction);
        // 타겟에서 카메라 방향으로의 레이캐스트
        RaycastHit hit;

        if (Physics.Raycast(CamtoPlayer, direction, out hit, maxDistance+2, groundLayerMask))
        {
            // 레이캐스트가 지면과 충돌하면 거리 조정
            distance = Mathf.Clamp(hit.distance, minDistance, maxDistance);
            desiredPosition = CamtoPlayer - (rotation * Vector3.forward * distance);
        }
        else
        {
            // 충돌이 없으면 최대 거리 유지
            distance = maxDistance;
            desiredPosition = CamtoPlayer - (rotation * Vector3.forward * distance);
        }

        // 카메라 위치와 회전 적용
        cameraContainer.transform.position = desiredPosition;
        cameraContainer.transform.LookAt(CamtoPlayer);
    }

    bool IsGrounded() //땅에 있는지 알아내기 위해 Raycast 사용
    {
        Ray[] rays = new Ray[4] // x축과 z축 +-값을 받아오기 위해 4개의 배열 사용
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down), //살짝 앞,위로 조정 - Ground위치에서 쏘게 되어 Ground를 인식하지 못하게 되는 문제를 방지하기 위함 
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down), // 그리고 Vector3를 아래방향으로 쏘아 지면이 있는지 체크
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) +(transform.up * 0.01f), Vector3.down)
        };

        for (int i = 0; i < rays.Length; i++) // for문으로 4가지 케이스를 돌려 GroundLayerMask가 존재한다면 true를 반환
        {
            if (Physics.Raycast(rays[i], 0.1f, groundLayerMask))
            {
                return true;
            }
        }

        return false;
    }

    void ToggleCursor()
    {
        bool toggle = Cursor.lockState == CursorLockMode.Locked; //커서가 잠겨있는 상태를 true로 정의
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked; //true이면 풀고 false라면 잠그기
        canLook = !toggle; // canlook의 bool값 뒤집기
    }

    void SubtractStamina()
    {
        CharacterManager.Instance.Player.condition.UseStamina(runStamina);
    }


    //모션 테스트용
    public void OnTestInput(InputAction.CallbackContext context)
    {
        animator.SetBool("Next", true);
        Invoke("SetNext", 0.1f);
    }

    void SetNext()
    {
        animator.SetBool("Next", false);
    }

    void AnimatorAim()
    {
        float cameraYRotation = cameraContainer.transform.localEulerAngles.y;

        // 이동 방향 벡터의 Y축 회전값 계산
        float moveDirectionYRotation = Mathf.Atan2(curMovementInput.x, curMovementInput.y) * Mathf.Rad2Deg; 

        // 두 각도 사이의 차이 계산
        float angleDifference = Mathf.DeltaAngle(cameraYRotation, moveDirectionYRotation);

        animator.gameObject.transform.localPosition = Vector3.zero;
        animator.gameObject.transform.localRotation = Quaternion.Euler(0f, angleDifference, 0f);
    }
}
