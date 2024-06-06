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
        hunger = transform.Find("CurrentValues").GetChild(0).GetComponent<Condition>();
        thirst = transform.Find("CurrentValues").GetChild(1).GetComponent<Condition>();
        health = transform.Find("CurrentValues").GetChild(2).GetComponent<Condition>();
        stamina = transform.Find("CurrentValues").GetChild(3).GetComponent<Condition>();
        temperature = transform.Find("CurrentValues").GetChild(5).GetComponent<Temperature>();

        CharacterManager.Instance.Player.condition.uiCondition = this;
    }
}
