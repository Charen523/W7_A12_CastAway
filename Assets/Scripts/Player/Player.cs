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
    public event Action InHome;
    public event Action OutHome;
    public event Action NearBed;
    public event Action FarBed; 

    public PlayerController controller; //플레이어 컨트롤러 호출
    public PlayerCondition condition;
    public Equipment equip;
    public UIInventory inventory;
    public ItemData itemData; //아이템 데이터 생성시 추가
    public ItemSlot[] invenSlots = new ItemSlot[16];

    public Transform dropPosition; //아이템을 드롭할 위치

    public int collisionMask;

    private void Awake()
    {
        CharacterManager.Instance.Player = this; // 캐릭터 매니저에 존재하는 Player에 자신을 넣어줌
        controller = GetComponent<PlayerController>(); //GetComponent로 PlayerController 넣어줌
        condition = GetComponent<PlayerCondition>();
        equip = GetComponent<Equipment>();
    }

    private void Start()
    {
        inventory.InventoryRefresh += OnInvenRefresh;
        invenSlots = inventory.slots;
    }

    private void OnInvenRefresh()
    {
        invenSlots = inventory.slots;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("B0002"))
        {
            NearWorkBench?.Invoke();
        }

        if (collision.gameObject.CompareTag("B0004"))
        {
            NearBed?.Invoke();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("B0002"))
        {
            FarWorkBench?.Invoke();
        }

        if (collision.gameObject.CompareTag("B0004"))
        {
            FarBed?.Invoke();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("B0001") || other.gameObject.CompareTag("B0003"))
        {
            condition.WarmToggle(true);
        }

        if (other.gameObject.CompareTag("B0006"))
        {
            InHome?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("B0001") || other.gameObject.CompareTag("B0003"))
        {
            condition.WarmToggle(false);
        }

        if (other.gameObject.CompareTag("B0006"))
        {
            OutHome?.Invoke();
        }
    }
}
