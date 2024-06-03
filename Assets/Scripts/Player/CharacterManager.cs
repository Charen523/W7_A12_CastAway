using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : Singleton<CharacterManager>
{
    //private static CharacterManager _instance;
    //public static CharacterManager Instance // 캐릭터 매니저 싱글톤
    //{
    //    get
    //    {
    //        if (_instance == null) //인스턴스가 없다면 생성하는 방어코드
    //        {
    //            _instance = new GameObject("CharacerManager").AddComponent<CharacterManager>(); //AddComponent로 스크립트를 붙여줌 
    //        }
    //        return _instance;
    //    }
    //}

    private Player _player;

    public Player Player //Player 스크립트 호출
    {
        get { return _player; }
        set { _player = value; }
    }
    //private void Awake()
    //{
    //    if (_instance == null) // _instance가 없을 경우 나 자신을 넣어줌
    //    {
    //        _instance = this;
    //        DontDestroyOnLoad(gameObject); // 씬을 이동해도 파괴되지 않도록
    //    }
    //    else
    //    {
    //        if (_instance != this)
    //        {
    //            Destroy(gameObject); //인스턴스가 내 자신이 아니라면 파괴함
    //        }
    //    }
    //}
}
