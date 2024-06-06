using System;
using Unity.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    /*Events*/
    public event Action WorkshopInput;

    [Header("Movement")]
    public float baseSpeed = 5;
    public float runSpeedRatio = 2;
    public float runStaminaDeltaValue = 10;
    public float jumpForce = 80;
    public LayerMask groundLayerMask;

    [Header("Look")]
    public float lookSensitivity = 0.05f;
    public float minXLook = -50;
    public float maxXLook = 60;

    [Header("Camera")]
    public Transform cameraContainer;
    public float distance = 3; // 타겟으로부터의 기본 거리
    public float minDistance = 1; // 타겟으로부터의 최소 거리
    public float maxDistance = 5; // 타겟으로부터의 최대 거리

    /*Player Input*/
    private Vector2 moveInput;
    private Vector2 mouseDelta;
    private float camCurXRot;
    private float camCurYRot;

    /*Components*/
    private Rigidbody rb;
    private Animator animator;

    /*player Controllable Status*/
    private bool canLook = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
    }

    void Start()
    {
        //default Cursor: Locked
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        if (animator.GetBool("IsWalk"))
        {
            transform.eulerAngles = new Vector3(0, camCurYRot, 0);
            AnimatorAim();
        }

        if (IsGrounded())
        {
            Move();
        }
    }

    private void LateUpdate()
    {
        if (canLook)
        {
            CameraLook();
        }
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed) //분기점 인풋액션이 시작되었을 때 Start를 쓰지 않는 이유 - 입력값을 받았을 때만 행동하기 때문
        {
            moveInput = context.ReadValue<Vector2>(); //값을 읽어옴 (Vectro2)
            animator.SetBool("IsWalk", true);
        }
        else if (context.phase == InputActionPhase.Canceled) //키가 떨어졌을 때 (취소되었을때)
        {
            moveInput = Vector2.zero; // Vector를 초기화
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
            rb.AddForce(Vector2.up * jumpForce, ForceMode.Impulse); //순간적으로 힘을 받아야 하기 때문에 Impulse를 사용
            animator.SetTrigger("IsJump");
        }
    }

    public void OnLookInput(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
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
            WorkshopInput?.Invoke(); //UI인벤토리에 있는 기능 사용
            ToggleCursor();
        }
    }

    public void OnSettingInput(InputAction.CallbackContext context)
    {

    }

    public void OnAimOffInput(InputAction.CallbackContext context)
    {

    }

    public void OnSitInput(InputAction.CallbackContext context)
    {
        //Animation이 없어서 미구현.
    }

    private void Move() //실제로 이동을 시키는 로직
    {
        Vector3 dir = transform.forward * moveInput.y + transform.right * moveInput.x; // 입력된 벡터값의 방향 X,y값 설정

        dir *= animator.GetBool("IsRun") ? (baseSpeed * runSpeedRatio) : baseSpeed;
        
        dir.y = rb.velocity.y; // 점프했을 때만 Y축 영향을 받아야 하기 때문에 velocity 값으로 고정

        rb.velocity = dir; // 세팅값을 Velociy에 입력
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
        CharacterManager.Instance.Player.condition.UseStamina(runStaminaDeltaValue);
    } //요기 있으면 안됨!!


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
        float moveDirectionYRotation = Mathf.Atan2(moveInput.x, moveInput.y) * Mathf.Rad2Deg; 

        // 두 각도 사이의 차이 계산
        float angleDifference = Mathf.DeltaAngle(cameraYRotation, moveDirectionYRotation);

        animator.gameObject.transform.localPosition = Vector3.zero;
        animator.gameObject.transform.localRotation = Quaternion.Euler(0f, angleDifference, 0f);
    }
}
