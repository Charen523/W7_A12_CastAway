using UnityEngine;

public class UICondition : MonoBehaviour
{
    public Condition hunger;
    public Condition thirst;
    public Condition health;
    public Condition stamina;
    public Temperature temperature;

    private void Start()
    {
        Condition[] conditions = transform.Find("CurrentValues").GetComponentsInChildren<Condition>();

        foreach (var condition in conditions)
        {
            switch (condition.conditionType)
            {
                case eConditionType.HUNGER:
                    hunger = condition;
                    break;
                case eConditionType.THIRST:
                    thirst = condition;
                    break;
                case eConditionType.HEALTH:
                    health = condition;
                    break;
                case eConditionType.STAMINA:
                    stamina = condition;
                    break;
                case eConditionType.TEMPERATURE:
                    temperature = (Temperature)condition;
                    break;
            }
        }

        CharacterManager.Instance.Player.condition.uiCondition = this;
    }
}
