using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player", menuName = "SO/New Player", order = 4)]
public class PlayerData : ScriptableObject
{
    [System.Serializable]
    public class Stat
    {
        public int startValue;
        public int maxValue;
        public int currentValue;
    }

    [System.Serializable]
    public class StaminaStat : Stat
    {
        public float regenRate; // 초당 회복되는 스태미나 양
    }

    public string characterName;
    public Stat health;
    public Stat thirst;
    public StaminaStat stamina;
    public Stat hunger;
}
