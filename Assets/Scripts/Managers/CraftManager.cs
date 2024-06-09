using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftManager : Singleton<CraftManager>
{
    private CraftSystem _craftSystem;
    private UIInventory _inventory;

    public CraftSystem CraftSystem //CraftSystem 스크립트 호출
    {
        get { return _craftSystem; }
        set { _craftSystem = value; }
    }

    public UIInventory UIInventory //CraftSystem 스크립트 호출
    {
        get { return _inventory; }
        set { _inventory = value; }
    }

    void Start()
    {
        _craftSystem = FindObjectOfType<CraftSystem>();
        _inventory = FindObjectOfType<UIInventory>();
        if (_craftSystem == null)
        {
            Debug.LogError("CraftSystem not found!");
        }
    }

}
