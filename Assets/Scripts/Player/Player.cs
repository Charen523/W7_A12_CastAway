using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    /*events*/
    public Action addItem;
    public event Action NearWorkBench;
    public event Action FarWorkBench;
    public event Action NearFurnace;
    public event Action FarFurnace; 

    public PlayerController controller; //플레이어 컨트롤러 호출
    public PlayerCondition condition;
    public Equipment equip;

    public ItemData itemData; //아이템 데이터 생성시 추가

    public Transform dropPosition; //아이템을 드롭할 위치

    public int collisionMask;

    private void Awake()
    {
        CharacterManager.Instance.Player = this; // 캐릭터 매니저에 존재하는 Player에 자신을 넣어줌
        controller = GetComponent<PlayerController>(); //GetComponent로 PlayerController 넣어줌
        condition = GetComponent<PlayerCondition>();
        equip = GetComponent<Equipment>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("B0002"))
        {
            NearWorkBench?.Invoke();
        }

        //Trigger로 바뀔 수도 있음.
        if (collision.gameObject.CompareTag("Furnace"))
        {
            NearFurnace?.Invoke();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("B0002"))
        {
            FarWorkBench?.Invoke();
        }

        //Trigger로 바뀔 수도 있음.
        if (collision.gameObject.CompareTag("Furnace"))
        {
            FarFurnace?.Invoke();
        }
    }
}
