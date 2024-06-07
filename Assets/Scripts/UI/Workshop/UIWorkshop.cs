using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIWorkshop : MonoBehaviour
{
    /*플레이어.*/
    private PlayerController controller;

    [Header("Workshop")]
    public List<GameObject> workshopBtns = new List<GameObject>();
    public List<GameObject> workshopPanels = new List<GameObject>();
    public int currentActiveIndex;
    //Workshop Index
    //[0]: Inventory
    //[1]: Equip
    //[2]: Craft

    private void Awake()
    {
        foreach ( Transform child in transform.Find("Btns"))
        {
            workshopBtns.Add(child.gameObject);
        }

        foreach (Transform child in transform.Find("Panels"))
        {
            workshopPanels.Add(child.gameObject);
        }
    }

    private void Start()
    {
        controller = CharacterManager.Instance.Player.controller;

        //인벤토리 창 Toggle 이벤트 등록.
        //TODO: 액션 이름 inventory > workshop으로 바꾸기.
        controller.WorkshopInput += ToggleWorkshop; 

        /*버튼 이벤트 리스너 추가*/
        workshopBtns[0].GetComponent<Button>().onClick.AddListener(OnInvenClicked);
        workshopBtns[1].GetComponent<Button>().onClick.AddListener(OnEquipClicked);
        workshopBtns[2].GetComponent<Button>().onClick.AddListener(OnCraftClicked);

        foreach (GameObject obj in workshopPanels)
        {
            obj.SetActive(false);
        }

        currentActiveIndex = 0;
        ToggleBtns(currentActiveIndex);

        gameObject.SetActive(false); //모든 초기화 완료 후 자기자신 끄기.
    }

    public void ToggleWorkshop()
    {
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }

    private void ToggleBtns(int index)
    {
        workshopBtns[currentActiveIndex].SetActive(true);
        workshopPanels[currentActiveIndex].SetActive(false); 

        currentActiveIndex = index;
        workshopBtns[currentActiveIndex].SetActive(false);
        workshopPanels[currentActiveIndex].SetActive(true);
    }

    private void OnInvenClicked()
    {
        ToggleBtns(0);
    }

    private void OnEquipClicked()
    {
        ToggleBtns(1);
    }

    private void OnCraftClicked()
    {
        ToggleBtns(2);
    }
}
