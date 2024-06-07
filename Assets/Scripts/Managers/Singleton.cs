using UnityEngine;

//싱글톤 상속만 시키면 바로 사용 가능!!
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                // 이미 씬에 존재하는 T 타입의 오브젝트를 찾기
                _instance = FindObjectOfType<T>();

                if (_instance == null)
                {
                    //Managers 오브젝트에 T 컴포넌트 추가하기
                    GameObject gameObject = new GameObject(typeof(T).Name);
                    _instance = gameObject.AddComponent<T>();
                }
            }

            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }
}