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
            switch (condition.name)
            {
                case "Hunger":
                    hunger = condition;
                    break;
                case "Thirst":
                    thirst = condition;
                    break;
                case "Health":
                    health = condition;
                    break;
                case "Stamina":
                    stamina = condition;
                    break;
                case "Temperature":
                    temperature = (Temperature)condition;
                    break;
            }
        }

        CharacterManager.Instance.Player.condition.uiCondition = this;
    }
}
