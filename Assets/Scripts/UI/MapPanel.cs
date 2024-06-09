using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MapPanel : MonoBehaviour
{
    public GameObject MapPanelUI;

    public void MapBtn()
    {
        MapPanelUI.SetActive(true);
    }

    public void ExitBtn()
    {
        MapPanelUI.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            MapPanelUI.SetActive(false);
        }

    }

}
