using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIManager : MonoBehaviour
{
    [SerializeField] Image settingsPanel;
    public void OnButtonClick(string button)
    {
        if (button == "Settings")
        {
            settingsPanel.gameObject.SetActive(true);
        }

    }

}
