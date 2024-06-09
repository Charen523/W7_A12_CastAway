using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPanel : MonoBehaviour
{
    public GameObject KeyPanelUI;

    public void KeyBtn()
    {
        KeyPanelUI.SetActive(true);
    }

    public void ExitBtn()
    {
        KeyPanelUI.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            KeyPanelUI.SetActive(false);
        }

    }
}
