using UnityEngine;

public class FindComponents : MonoBehaviour
{
    public string componentName = "PausePanel"; // 검사할 컴포넌트 이름

    void Start()
    {
        // 씬의 모든 게임 오브젝트 찾기
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            // 해당 오브젝트가 비활성화 상태일 수 있으므로 활성화 상태로 설정
            if (!obj.activeInHierarchy)
                continue;

            // 특정 컴포넌트를 가지고 있는지 검사
            Component component = obj.GetComponent(componentName);
            if (component != null)
            {
                Debug.Log("Found " + componentName + " on object: " + obj.name);
            }
        }
    }
}
