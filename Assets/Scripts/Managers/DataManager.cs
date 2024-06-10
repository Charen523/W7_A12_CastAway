using System.Collections.Generic;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    public Dictionary<string, ItemData> itemDataDictionary;
    public Dictionary<string, GameObject> itemPrefabDictionary;
    public Dictionary<string, GameObject> EquipPrefabDictionary;
    public Dictionary<string, QuestData> QuestDataDictionary;

    protected override void Awake()
    {
        base.Awake();
        itemDataDictionary = new Dictionary<string, ItemData>();
        itemPrefabDictionary = new Dictionary<string, GameObject>();
        EquipPrefabDictionary = new Dictionary<string, GameObject>();
        QuestDataDictionary = new Dictionary<string, QuestData>();


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
        foreach (var data in Resources.LoadAll<QuestData>("Quest_SO"))
        {
            QuestDataDictionary.Add(data.name, data);
        }

        Debug.Log("QuestDataDictionary 초기화 완료:");
        foreach (var quest in QuestDataDictionary.Values)
        {
            Debug.Log($"Quest Name: {quest.QuestName}, BoolClear: {quest.BoolClear}");
        }
    }

    public QuestData GetQuest()
    {
        foreach (var quest in QuestDataDictionary.Values)
        {
            if (quest.BoolClear == false)
            {
                return quest;
            }
        }
        return null;
    }
}
