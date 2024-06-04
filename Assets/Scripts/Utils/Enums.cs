
//규칙1. enum 이름 앞에 소문자e 붙이기.
//규칙2. enum의 원소들은 대문자로 만들기.
public enum eItemType
{
    MATERIAL,
    CONSUME,
    EQUIP
}

public enum eConsumableType
{
    HUNGER,
    THIRST,
    HEALTH,
    STAMINA,
    TEMPERATURE
}

public enum eAIState
{
    IDLE, //기본
    WANDERING, //자동으로 이동
    ATTACKING, // 공격
    FLEEING // 도망
}