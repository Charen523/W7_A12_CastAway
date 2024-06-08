using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour
{
    public GameObject Pause;
    public GameObject SettingUI;
    private bool isPaused = false;
    public GameObject InventoryUI; // 인벤토리 UI 게임 오브젝트 추가

    public void SetPause()
    {
        Time.timeScale = 0f;
        Pause.SetActive(true);
        isPaused = true; // Pause 상태로 설정
    }

    public void SetContinue()
    {
        Time.timeScale = 1f;
        Pause.SetActive(false);
        isPaused = false; // Pause 상태 해제
    }

    public void Setting()
    {
        Time.timeScale = 0f;
        SettingUI.SetActive(true);
        isPaused = true; // 설정 상태도 Pause로 간주
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (SettingUI != null && SettingUI.activeSelf)
            {
                SettingUI.SetActive(false);
                isPaused = false; // Setting 상태 해제 시 isPaused도 false로 설정
            }
           
        }
       
    }
}
