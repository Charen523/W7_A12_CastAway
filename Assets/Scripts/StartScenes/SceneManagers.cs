using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagers : MonoBehaviour
{
    public UIInventory uiInventory;
    public void StartBtn()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void RestartBtn()
    {
        uiInventory.ClearInventory();

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void GameExit()
    {
        Application.Quit();
    }


}
