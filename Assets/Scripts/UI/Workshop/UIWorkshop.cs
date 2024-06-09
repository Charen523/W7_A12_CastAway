using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIWorkshop : MonoBehaviour
{
    private enum eWorkshop
    {
        INVENTORY,
        EQUIP,
        CRAFT
    }

    /*플레이어.*/
    private PlayerController controller;

    [Header("Workshop")]
    public List<GameObject> workshopBtns = new List<GameObject>();
    public List<GameObject> workshopPanels = new List<GameObject>();
    public int currentActiveIndex;

    private void Awake()
    {
        foreach ( Transform child in transform.Find("Btns"))
        {
            workshopBtns.Add(child.gameObject);
        }

        foreach (Transform child in transform.Find("Panels"))
        {
            workshopPanels.Add(child.gameObject);
            if (!child.gameObject.activeSelf)
                child.gameObject.SetActive(true);
        }
    }

    private void Start()
    {
        controller = CharacterManager.Instance.Player.controller;
        controller.WorkshopInput += ToggleWorkshop; 

        /*버튼 이벤트 리스너 추가*/
        workshopBtns[(int)eWorkshop.INVENTORY].GetComponent<Button>().onClick.AddListener(OnInvenClicked);
        workshopBtns[(int)eWorkshop.EQUIP].GetComponent<Button>().onClick.AddListener(OnEquipClicked);
        workshopBtns[(int)eWorkshop.CRAFT].GetComponent<Button>().onClick.AddListener(OnCraftClicked);

        foreach (GameObject obj in workshopPanels)
        {
            obj.SetActive(false);
        }

        currentActiveIndex = 0;
        ToggleBtns(currentActiveIndex);

        gameObject.SetActive(false); //모든 초기화 완료 후 자기자신 끄기.
    }

    private void ToggleWorkshop()
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
        ToggleBtns((int)eWorkshop.INVENTORY);
    }

    private void OnEquipClicked()
    {
        ToggleBtns((int)eWorkshop.EQUIP);
    }

    private void OnCraftClicked()
    {
        ToggleBtns((int)eWorkshop.CRAFT);
    }
}
