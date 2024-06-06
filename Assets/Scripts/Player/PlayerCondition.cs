using System;
using UnityEngine;

public interface IDamagable //데미지를 받을 수 있는 오브젝트에 인터페이스 다중상속을 위한 선언
{
    void TakeDamage(int damageAmount);
}

public class PlayerCondition : MonoBehaviour, IDamagable
{
    /*Events*/
    public event Action onTakeDamage;
    public event Action OnDeath;

    [Header("UI Condition")]
    public UICondition uiCondition;
    private Condition hunger { get { return uiCondition.hunger; } }
    private Condition thirst { get { return uiCondition.thirst; } }
    private Condition health { get { return uiCondition.health; } }
    private Condition stamina { get { return uiCondition.stamina; } }
    private Condition temperature { get { return uiCondition.temperature; } }

    [Header("Condition Delta Values")]
    public float healthDecay;
    
    /*Components*/
    private PlayerController controller;

    /*player Condition Status*/
    private bool isDead;

    private void Start()
    {
        controller = CharacterManager.Instance.Player.controller;
    }

    private void Update()
    {
        /*default Decays*/
        hunger.ChangeValue(-hunger.deltaRate * Time.deltaTime);
        thirst.ChangeValue(-thirst.deltaRate * Time.deltaTime);
        
        if (hunger.curValue == 0 || thirst.curValue == 0)
        {
            health.ChangeValue(-healthDecay * Time.deltaTime);
        }

        if (health.curValue == 0 && !isDead)
        {
            Die();
        }

        if (controller.animator.GetBool("IsRun"))
        {
            if (stamina.curValue != 0)
            {
                stamina.ChangeValue(-stamina.deltaRate * Time.deltaTime); //달리는 중 스테미나 소모
            }
            else
            {
                controller.canRun = false;
            }
        }
        else if (stamina.curValue < stamina.maxValue)
        {
            stamina.ChangeValue(stamina.deltaRate * Time.deltaTime); //스테미나 회복
        }

        if (temperature.curValue > 80 || temperature.curValue < 20)
        {
            health.ChangeValue(-healthDecay * Time.deltaTime);
        }
    }

    public void Eat(float amount)
    {
        hunger.ChangeValue(amount);
    }

    public void Drink(float amount)
    {
        hunger.ChangeValue(amount);
    }

    public void Heal(float amount)
    {
        health.ChangeValue(amount);
    }

    public void TakeDamage(int damageAmount)
    {
        health.ChangeValue(-damageAmount);
        onTakeDamage?.Invoke();
    }

    public void Die()
    {
        isDead = true;
        OnDeath?.Invoke();
    }

    public void Warm()
    {
        temperature.ChangeValue(Mathf.Max(temperature.curValue, 20));
    }

    public void Cool()
    {
        temperature.ChangeValue(Mathf.Min(temperature.curValue, 80));
    }
}