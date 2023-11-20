using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIManager : MonoBehaviour
{
    [SerializeField] Image settingsPanel;
    [SerializeField] GameObject inGameUI;
    [SerializeField] GameObject joyStick;

    private void OnEnable()
    {
        GameManager.OnGameStateChanged += SetjoyStickState;
    }
    private void OnDisable()
    {

        GameManager.OnGameStateChanged -= SetjoyStickState;
    }
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
    void SetjoyStickState(GameState state)
    {
        if ((state == GameState.UnlockingArea))
        {
            joyStick.SetActive(false);
            inGameUI.SetActive(false);
        }
        else
        {
            joyStick.SetActive(true);
            inGameUI.SetActive(true);
        }

    }

}
