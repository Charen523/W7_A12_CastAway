using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerController controller; //플레이어 컨트롤러 호출
    public PlayerCondition condition;

    public ItemData itemData; //아이템 데이터 생성시 추가
    public Action addItem;

    public Transform dropPosition; //아이템을 드롭할 위치

    public int collisionMask;

    private void Awake()
    {
        CharacterManager.Instance.Player = this; // 캐릭터 매니저에 존재하는 Player에 자신을 넣어줌
        controller = GetComponent<PlayerController>(); //GetComponent로 PlayerController 넣어줌
        condition = GetComponent<PlayerCondition>();
    }
}
