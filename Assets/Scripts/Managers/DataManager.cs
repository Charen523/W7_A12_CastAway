using System.Collections.Generic;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    public Dictionary<string, ItemData> itemDataDictionary;
    public Dictionary<string, GameObject> itemPrefabDictionary;
    public Dictionary<string, GameObject> EquipPrefabDictionary;

    protected override void Awake()
    {
        base.Awake();
        itemDataDictionary = new Dictionary<string, ItemData>();
        itemPrefabDictionary = new Dictionary<string, GameObject>();
        EquipPrefabDictionary = new Dictionary<string, GameObject>();


        foreach (var data in Resources.LoadAll<ItemData>("Item_SOs"))
        {
            itemDataDictionary.Add(data.name, data);
        }
        foreach (var data in Resources.LoadAll<GameObject>("Item_Prefabs"))
        {
            itemPrefabDictionary.Add(data.name, data);
        }
        foreach (var data in Resources.LoadAll<GameObject>("Equip_Prefabs"))
        {
            EquipPrefabDictionary.Add(data.name, data);
        }
    }
}
