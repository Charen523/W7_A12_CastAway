using UnityEngine;
using UnityEngine.SceneManagement;



public class SceneManagers : MonoBehaviour
{
    public UIInventory uiInventory;
    public GameObject uiInventoryObject; 
    public void StartBtn()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void RestartBtn()
    {
        UIInventory uiInventory = FindObjectOfType<UIInventory>();
        if (uiInventory != null)
        {
            uiInventory.ClearInventory();
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        
    }
    public void GameExit()
    {
        Application.Quit();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 씬이 로드된 후 UIInventory 오브젝트를 활성화
        if (uiInventoryObject != null)
        {
            uiInventoryObject.SetActive(true);
        }
    }



}
