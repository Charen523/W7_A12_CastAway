using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : Singleton<CharacterManager>
{

    private Player _player;

    public Player Player //Player 스크립트 호출
    {
        get { return _player; }
        set { _player = value; }
    }
}
