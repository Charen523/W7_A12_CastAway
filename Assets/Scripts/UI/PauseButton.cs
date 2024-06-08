using UnityEngine;

public class PauseButton : MonoBehaviour
{
    public GameObject Pause;
    public GameObject SettingUI;
    public GameObject InventoryUI; // 인벤토리 UI 게임 오브젝트 추가

    public void SetPause()
    {
        Pause.SetActive(true);
        GameManager.Instance.OnPauseEvent();
    }

    public void SetContinue()
    {
        Pause.SetActive(false);
        GameManager.Instance.OnResumeEvent();
    }

    public void Setting()
    {
        SettingUI.SetActive(true);
        GameManager.Instance.OnPauseEvent();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (SettingUI != null && SettingUI.activeSelf)
            {
                SettingUI.SetActive(false);
                GameManager.Instance.OnResumeEvent();
            }
        }
    }
}