using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    /*Events*/
    public event Action WorkshopInput;
    public event Action SettingInput;
    public event Action AltInfoOn;
    public event Action AltInfoOff;

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
    [HideInInspector] public Animator animator;
    public AudioSource footstepaudio;
    public AudioSource runningstep;
    public AudioSource Jumpaudio;
    public float footstepInterval = 0.5f; // 발자국 소리가 재생되는 간격
    private float lastFootstepTime;

    /*player Controllable Status*/
    public bool canLook = true;
    [HideInInspector] public bool canRun = true;
    

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
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
        if (context.phase == InputActionPhase.Performed)
        {
            moveInput = context.ReadValue<Vector2>();
            footstepaudio.Play();
            animator.SetBool("IsWalk", true);
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            moveInput = Vector2.zero;
            animator.SetBool("IsWalk", false);
            footstepaudio.Stop();
            if (animator.GetBool("IsRun") == true)
            {
                animator.SetBool("IsRun", false);
                CancelInvoke("SubtractStamina"); //Refactor 대상.
            }
        }
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && IsGrounded())
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode.Impulse);
            animator.SetTrigger("IsJump");
            Jumpaudio.Play();
        }
        if(animator.GetBool("IsJump") == false)
        {
            Jumpaudio.Stop();
        }
    }

    public void OnLookInput(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnRunInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            animator.SetBool("IsRun", true);
            runningstep.Play();
            footstepaudio.Stop();
        }

        if (context.phase == InputActionPhase.Canceled)
        {
            animator.SetBool("IsRun", false);
            canRun = true;
            footstepaudio.Play();
            runningstep.Stop();
        }
    }

    public void OnInventoryInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            WorkshopInput?.Invoke();
            ToggleCursor();
        }
    }

    public void OnSettingInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            SettingInput?.Invoke();
        }
    }

    public void OnAimOffInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            ToggleCursor();
            AltInfoOn?.Invoke();
        }

        if (context.phase == InputActionPhase.Canceled)
        {
            ToggleCursor();
            AltInfoOff?.Invoke();
        }
    }

    public void OnSitInput(InputAction.CallbackContext context)
    {
        //Animation이 없어서 미구현.
    }

    private void Move()
    {
        Vector3 direction = transform.forward * moveInput.y + transform.right * moveInput.x; //방향
        
        if (canRun)
        {
            direction *= animator.GetBool("IsRun") ? (baseSpeed * runSpeedRatio) : baseSpeed; //속도
        }
        else if (animator.GetBool("IsRun"))
        {
            direction *= baseSpeed;
            animator.SetBool("IsRun", false);
        }
        
        direction.y = rb.velocity.y; //y축 동기화
        rb.velocity = direction; //적용
    }

    private bool IsGrounded()
    {
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position - (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position - (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down)
        };

        for (int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 0.1f, groundLayerMask))
            {
                return true;
            }
        }

        return false;
    }

    private void CameraLook()
    {
        /*mouseDelta 회전각*/
        camCurXRot -= mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        camCurYRot += mouseDelta.x * lookSensitivity;

        //Player 머리 위치 계산.
        Vector3 CamToPlayer = new Vector3 (transform.position.x, transform.position.y + 1, transform.position.z);

        //현재 회전각에 따른 카메라 위치 계산
        Quaternion rotation = Quaternion.Euler(camCurXRot, camCurYRot, 0);
        Vector3 desiredPosition = CamToPlayer - (rotation * Vector3.forward * distance);

        //카메라와 타겟 사이의 방향 계산
        Vector3 direction = (desiredPosition - transform.position).normalized;

        //레이와 지면 충돌여부 계산.
        if (Physics.Raycast(CamToPlayer, direction, out RaycastHit hit, maxDistance+2, groundLayerMask))
        { 
            distance = Mathf.Clamp(hit.distance, minDistance, maxDistance);
            desiredPosition = CamToPlayer - (rotation * Vector3.forward * distance);
        }
        else
        {
            distance = maxDistance;
            desiredPosition = CamToPlayer - (rotation * Vector3.forward * distance);
        }

        //카메라 위치와 회전 적용
        cameraContainer.transform.position = desiredPosition;
        cameraContainer.transform.LookAt(CamToPlayer);
    }

    public void ToggleCursor()
    {
        bool toggle = Cursor.lockState == CursorLockMode.Locked;
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        canLook = !toggle;
    }

    private void AnimatorAim()
    {
        float cameraYRotation = cameraContainer.transform.localEulerAngles.y;
        float moveDirectionYRotation = Mathf.Atan2(moveInput.x, moveInput.y) * Mathf.Rad2Deg; 

        float angleDifference = Mathf.DeltaAngle(cameraYRotation, moveDirectionYRotation);

        animator.gameObject.transform.localPosition = Vector3.zero;
        animator.gameObject.transform.localRotation = Quaternion.Euler(0f, angleDifference, 0f);
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

}
