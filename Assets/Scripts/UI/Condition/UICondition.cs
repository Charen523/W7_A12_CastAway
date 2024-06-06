using UnityEngine;

public class UICondition : MonoBehaviour
{
    public Condition hunger;
    public Condition thirst;
    public Condition health;
    public Condition stamina;

    private void Start()
    {
        CharacterManager.Instance.Player.condition.uiCondition = this;
    }
}
