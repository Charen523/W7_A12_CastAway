using System.Collections;
using UnityEngine;
using static ResourcePoolManager;

public class ItemObject : MonoBehaviour, IInteractable
{
    public ItemData data;
    public AudioSource pickupSound;
    private CraftSystem craftSystem;

    private void Start()
    {
        craftSystem = FindObjectOfType<CraftSystem>();
    }

    public void GetInteractPrompt()
    {
        if (data != null)
        {
            craftSystem.promptPanel.SetActive(true);
            //추후 주울 수 있는 아이템 옆에 [data.displayName] 띄워주기.
            string str = $"[{data.displayName}]\n{data.description}";
            craftSystem.promptText.text = str;

            StartCoroutine(ClosePromptAfterDelay(3.0f));
        }
    }

    private IEnumerator ClosePromptAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ClosePrompt();
    }

    public void ClosePrompt()
    {
        if (craftSystem.promptPanel.activeSelf)
        {
            //판넬 setactive false
            craftSystem.promptPanel.SetActive(false);
        }
    }

    public void OnInteract()
    {
        //inventory에 아이템 넣기.
        CharacterManager.Instance.Player.itemData = data;
        CharacterManager.Instance.Player.addItem?.Invoke();
        PickupItem();
        Destroy(gameObject);
    }

    private void PickupItem()
    {
        // 아이템 획득 사운드 재생
        if (pickupSound != null)
        {
            pickupSound.Play();
        }
    }
}
