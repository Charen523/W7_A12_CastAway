using System.Collections;
using UnityEngine;

public class DeathUIController : MonoBehaviour
{
    public GameObject DeathUI;

    private void OnEnable()
    {
        DeathUI.SetActive(false);
    }

    public void ShowDeathUI()
    {
        DeathUI.SetActive(true);
    }
}
