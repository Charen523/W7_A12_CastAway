using System;
using UnityEngine;

public interface IDamagable //데미지를 받을 수 있는 오브젝트에 인터페이스 다중상속을 위한 선언
{
    void TakePhysicalDamage(int damageAmount);
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

    [Header("Condition Delta Values")]
    public float noHungerHealthDecay;
    public float staminaRegen;

    /*Components*/
    private PlayerController controller;

    /**/
    private bool isDead;

    private void Start()
    {
        controller = CharacterManager.Instance.Player.controller;
       staminaRegen = stamina.deltaRate; //회복될 값 받아오기
    }
    private void Update()
    {
        hunger.Subtract(hunger.deltaRate * Time.deltaTime);
        thirst.Subtract(thirst.deltaRate * Time.deltaTime);
        

        if (hunger.curValue == 0.0f)
        {
            health.Subtract(noHungerHealthDecay * Time.deltaTime);
        }

        if (health.curValue == 0.0f && !isDead)
        {
            Die();
        }

        if (stamina.curValue == 0.0f)
        {
            controller.canRun = false;
        }
        else
        {
            stamina.Add(stamina.deltaRate * Time.deltaTime);
        }

        Debug.Log(stamina.curValue);
    }

    public void Heal(float amount)
    {
        health.Add(amount);
    }

    public void Eat(float amount)
    {
        hunger.Add(amount);
    }

    public void Die() // 죽었을 시 메세지 호출
    {
        Debug.Log("플레이어 사망.");
        isDead = true;
        OnDeath?.Invoke();
    }

    public void TakePhysicalDamage(int damageAmount) //데미지를 받을 경우 데미지만큼 HP를 줄임
    {
        health.Subtract(damageAmount);
        onTakeDamage?.Invoke();
    }

    public bool UseStamina(float amount)
    {
        if (stamina.curValue - amount < 0) // 스태미나가 충분하지 못할 때
        {
            return false; //false 반환
        }
        stamina.Subtract(amount); // 스태미나에서 amount만큼 제거
        return true;
    }
}