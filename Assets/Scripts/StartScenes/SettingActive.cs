using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingActive : MonoBehaviour
{
    public GameObject Setting;

    public void SettingBtn()
    {
        if(Setting != null)
        {
            Setting.SetActive(true);
        }
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(Setting != null && Setting.activeSelf)
            {
                Setting.SetActive(false);
            }
        }
    }
}
