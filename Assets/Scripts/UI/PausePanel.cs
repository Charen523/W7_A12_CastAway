using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PausePanel : MonoBehaviour
{
    public InputActionAsset inputActionAsset;
    private InputActionMap inputActions;
    private InputAction settingAction;
    private PlayerController controller;

    private void Start()
    {
        inputActions = inputActionAsset.actionMaps[0];
        settingAction = inputActions.actions[5];

        controller = CharacterManager.Instance.Player.controller;
        controller.SettingInput += TogglePause;
        GameManager.Instance.OnPause += BlockInputs;
        GameManager.Instance.OnResume += ReleaseInputs;

        gameObject.SetActive(false);
    }

    private void BlockInputs()
    {
        foreach (var action in inputActions)
        {
            if (action != settingAction)
            {
                action.Disable();
            }
        }

        settingAction.Enable();
    }

    private void ReleaseInputs()
    {
        foreach (var action in inputActions)
        {
            if (action != settingAction)
            {
                action.Enable();
            }
        }
    }

    public void TogglePause()
    {
        controller.ToggleCursor();

        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
            GameManager.Instance.OnResumeEvent();
        }
        else
        {
            gameObject.SetActive(true);
            GameManager.Instance.OnPauseEvent();
        }
    }

    public void CallMainScene()
    {
        Time.timeScale = 1.0f; //원래 게임매니저에서 해주어야 할거 같지만...
        SceneManager.LoadScene(0);
    }
}