using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIManager : MonoBehaviour
{
    [SerializeField] Image settingsPanel;
    [SerializeField] GameObject inGameUI;
    [SerializeField] GameObject joyStickStopper;
    AudioManager AM;

    private void Start()
    {
        AM = AudioManager.instance;
    }

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
            AM.Play("Pop");
            settingsPanel.gameObject.SetActive(true);
            inGameUI.SetActive(false);
        }
        if (button == "ExitSettings")
        {
            AM.Play("Pop");
            settingsPanel.gameObject.SetActive(false);
            inGameUI.SetActive(true);
        }
        if (button == "PrivacyPolicy")
        {
            AM.Play("Pop");
            //=================================
        }

    }
    void SetjoyStickState(GameState state)
    {
        if ((state == GameState.UnlockingArea))
        {
            inGameUI.SetActive(false);
            GameManager.Instance.farmerObject.GetComponent<Rigidbody>().isKinematic = true;
        }
        else
        {
            GameManager.Instance.farmerObject.GetComponent<Rigidbody>().isKinematic = false;
            inGameUI.SetActive(true);
        }

    }

}
