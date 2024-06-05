using UnityEngine;

public class FixObject : MonoBehaviour, IInteractable
{
    public void GetInteractPrompt()
    {
        //고치는 데에 필요한 아이템 알려주기.
    }

    public void ClosePrompt()
    {
        //판넬 setactive false
    }

    public void OnInteract()
    {
       //if로 아이템을 다 모아왔는지 검사한 후, 아이템을 가져가고 고쳐주기.
    }
}