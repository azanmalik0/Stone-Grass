using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIManager : MonoBehaviour
{
    [SerializeField] Image settingsPanel;
    [SerializeField] GameObject inGameUI;
    [SerializeField] GameObject joyStickStopper;

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
            //joyStickStopper.SetActive(true);
            inGameUI.SetActive(false);
            //GameManager.Instance.tractorObject.GetComponent<Rigidbody>().isKinematic = true;
            GameManager.Instance.farmerObject.GetComponent<Rigidbody>().isKinematic = true;
        }
        else
        {
            //joyStickStopper.SetActive(false);
           // GameManager.Instance.tractorObject.GetComponent<Rigidbody>().isKinematic = false;
            GameManager.Instance.farmerObject.GetComponent<Rigidbody>().isKinematic = false;
            inGameUI.SetActive(true);
        }

    }

}
