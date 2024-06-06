using UnityEngine;

public class Temperature : Condition
{
    public Gradient tempColor;

    protected override void Update()
    {
        base.Update();
        uiBar.color = tempColor.Evaluate(GetPertentage());
    }

    public override void ChangeValue(float amount)
    {
        base.ChangeValue(amount);
    }

    public float GetCurrentValue()
    {
        return curValue;
    }
}

