
//규칙1. enum 이름 앞에 소문자e 붙이기.
//규칙2. enum의 원소들은 대문자로 만들기.
public enum eItemType
{
    MATERIAL,
    CONSUME,
    EQUIP
}

public enum eConditionType
{
    HUNGER,
    THIRST,
    HEALTH,
    STAMINA,
    TEMPERATURE
}

public enum eEquipType
{
    DEFAULT,
    HELMET,
    RIGHT_HAND,
    PANTS,
    ARMOR,
    LEFT_HAND
}

public enum eEquipStatType
{
    ATK,
    DEF
}

public enum eAIState
{
    IDLE, //기본
    WANDERING, //자동으로 이동
    ATTACKING, // 공격
    FLEEING // 도망
}