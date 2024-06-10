using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Equipment : MonoBehaviour
{
    public EquipRightHand curEquip; //현재 장착중인 장비
    public Transform equipParent; //장비를 달아줄 transform (캐릭터의 손을 기준점으로 잡아야 할 듯 한데..)

    private PlayerController controller; //플레이어 컨트롤러
    private PlayerCondition condition; // 컨디선 캐싱
    private Vector3 equipPosition;
    private Quaternion equipRotation;

    void Start()
    {
        controller = CharacterManager.Instance.Player.controller;
        condition = CharacterManager.Instance.Player.condition;
    }

    private void Update()
    {

    }

    public void OnAttackInput(InputAction.CallbackContext context) //공격 인풋 세팅
    {
        if (context.phase == InputActionPhase.Performed && curEquip != null && controller.canLook) // 버튼이 눌렸다면 + 장착된 장비가 있다면 + 컨트롤러가 정지된 상태가 아니라면 (인벤토리 등)
        {
            curEquip.OnAttackInput(); //애니메이션이 동작하도록
        }
    }

    public void EquipNew(EquipData data) //장착하기
    {
        UnEquip(); //현재 장착한 장비를 해제
        equipPosition = data.equipPrefab.transform.position; //기존 설정해 둔 position과 rotation값 저장
        equipRotation = data.equipPrefab.transform.rotation;
        curEquip = Instantiate(data.equipPrefab, equipParent).GetComponent<EquipRightHand>();// 장착을 할 장비를 curEquip에 넣어줌 
        curEquip.transform.localPosition = equipPosition; // 설정해두었던 값 적용
        curEquip.transform.localRotation = equipRotation;
    }

    public void UnEquip()
    {
        if (curEquip != null) // 장착한 장비가 있다면 (null이 아니라면)
        {
            Destroy(curEquip.gameObject); // 게임오브젝트를 파괴하고
            curEquip = null; // curEquip을 비움
        }
    }
}