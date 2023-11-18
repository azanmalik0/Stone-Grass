using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIManager : MonoBehaviour
{
    [SerializeField] Image settingsPanel;
    [SerializeField] GameObject inGameUI;
    public void OnButtonClick(string button)
    {
        if (button == "OpenSettings")
        {
            settingsPanel.gameObject.SetActive(true);
            inGameUI.SetActive(false);
        }
        if (button == "ExitSettings")
        {
            settingsPanel.gameObject.SetActive(false);
            inGameUI.SetActive(true);
        }
        if (button == "PrivacyPolicy")
        {
            //=================================
        }

    }

}
