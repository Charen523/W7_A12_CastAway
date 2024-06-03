using UnityEngine;

public interface IDamagable //데미지를 받을 수 있는 오브젝트에 인터페이스 다중상속을 위한 선언
{
    void TakePhysicalDamage(int damageAmount);
}

public class PlayerCondition : MonoBehaviour, IDamagable
{
    //public UICondition uiCondition; // Uicondition 스크립트 받아오기

    private float standardRegen;

    //Condition health { get { return uiCondition.health; } }
    //Condition hunger { get { return uiCondition.hunger; } }
    //Condition stamina { get { return uiCondition.stamina; } } // 각 UI의 값을 받아옴.
    //Condition thirst { get { return uiCondition.thirst; } }

    public float noHungerHealthDecay; //배고픔 게이지가 0일시 줄어들 HP값
    //public event Action onTakeDamage; // 데미지를 입는 이벤트 추가
    private void Start()
    {
        //standardRegen = stamina.regenRate; 회복될 값 받아오기
    }
    private void Update()
    {
        //hunger.Subtract(hunger.decayRate * Time.deltaTime); // 꾸준히 hunger의 게이지를 깎아줌
        //stamina.Add(stamina.regenRate * Time.deltaTime); //꾸준히 stamina의 게이지를 채워줌

        //if (hunger.curvalue == 0.0f) //배고픔 게이지가 0일 경우
        //{
        //    health.subtract(nohungerhealthdecay * time.deltatime); // hp게이지를 깎아줌
        //}

        //if (health.curValue == 0.0f)
        //{
        //    Die(); //HP가 0일 경우 죽음
        //}
    }

    public void Heal(float amount) //회복하면 특정 양 만큼 회복함
    {
        //health.Add(amount);
    }

    public void Eat(float amount) // 먹으면 배고픔을 특정 양 만큼 회복함
    {
        //hunger.Add(amount);
    }

    public void Die() // 죽었을 시 메세지 호출
    {
        Debug.Log("플레이어가 죽었다.");
    }

    public void TakePhysicalDamage(int damageAmount) //데미지를 받을 경우 데미지만큼 HP를 줄임
    {
        //health.Subtract(damageAmount);
        //onTakeDamage?.Invoke();
    }

    public bool UseStamina(float amount)
    {
        //if (stamina.curValue - amount < 0) // 스태미나가 충분하지 못할 때
        //{
        //    return false; //false 반환
        //}
        //stamina.Subtract(amount); // 스태미나에서 amount만큼 제거
        return true;
    }
}