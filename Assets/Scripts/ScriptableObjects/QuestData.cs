using UnityEngine;


[CreateAssetMenu(fileName = "Quest", menuName = "SO/New Quest", order = 6)]
public class QuestData : ScriptableObject
{
    [Header("Info")]
    public string QuestName;
    public string QuestDescription;
    public string CurrentCount;
    public string MaxCount;
}
