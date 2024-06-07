using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    public Dictionary<string, ItemData> itemDataDictionary;
    public Dictionary<string, GameObject> itemPrefabDictionary;

    protected override void Awake()
    {
        base.Awake();
        itemDataDictionary = new Dictionary<string, ItemData>();
        itemPrefabDictionary = new Dictionary<string, GameObject>();
        
        foreach (var data in Resources.LoadAll<ItemData>("Item_SOs")) 
        {
            itemDataDictionary.Add(data.name, data);
        }
        
        foreach (var data in Resources.LoadAll<GameObject>("Item_Prefabs"))
        {
            itemPrefabDictionary.Add(data.name, data);
        }
    }
}
