using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIManager : MonoBehaviour
{
    AudioManager AM;
    [SerializeField] Image settingsPanel;
    [SerializeField] GameObject inGameUI;
    [SerializeField] GameObject joyStickStopper;
    [SerializeField] RectTransform nextLevelPopup;

    private void Start()
    {
        AM = AudioManager.instance;
    }
    private void OnEnable()
    {
        GameManager.OnGameStateChanged += SetjoyStickState;
        ProgressBarManager.OnSecondStarUnlock += EnableNextLevelPopup;
    }
    private void OnDisable()
    {
        GameManager.OnGameStateChanged -= SetjoyStickState;
        ProgressBarManager.OnSecondStarUnlock -= EnableNextLevelPopup;
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
    void EnableNextLevelPopup()
    {
        if (LevelMenuManager.Instance.currentLevel < 9)
        {
            nextLevelPopup.gameObject.SetActive(true);
            nextLevelPopup.DOScale(Vector3.one, 1f).SetLoops(-1, LoopType.Yoyo);
        }
    }

}
