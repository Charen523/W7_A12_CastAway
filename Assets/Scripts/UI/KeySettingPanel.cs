using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeySettingPanel : MonoBehaviour
{
        public GameObject KeySettingPanelUI;

        public void KeyBtn()
        {
            KeySettingPanelUI.SetActive(true);
        }

        public void ExitBtn()
        {
            KeySettingPanelUI.SetActive(false);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                KeySettingPanelUI.SetActive(false);
            }

        }

    }


