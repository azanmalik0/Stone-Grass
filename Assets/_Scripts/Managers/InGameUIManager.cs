using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIManager : MonoBehaviour
{
    public static InGameUIManager Instance;
    AudioManager AM;
    [SerializeField] Image settingsPanel;
    [SerializeField] public GameObject inGameUI;
    [SerializeField] GameObject joyStickStopper;
    [SerializeField] RectTransform nextLevelPopupButton;
    [SerializeField] RectTransform nextLevelPopupBG;
    [SerializeField] GameObject nextLevelPopupPanel;
    [SerializeField] RectTransform nextLevelPopupShimmer;
    public GameObject progressBar;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        AM = AudioManager.instance;
    }
    private void OnEnable()
    {
        GameManager.OnGameStateChanged += SetjoyStickState;
        ProgressBarManager.OnThirdStarUnlock += EnableNextLevelButtonPopup;
    }
    private void OnDisable()
    {
        GameManager.OnGameStateChanged -= SetjoyStickState;
        ProgressBarManager.OnThirdStarUnlock -= EnableNextLevelButtonPopup;

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
            Application.OpenURL("https://sites.google.com/view/biza-studio/home");
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
    void EnableNextLevelButtonPopup(int DontMind)
    {
        if (LevelMenuManager.Instance.currentLevel < 9)
        {
            nextLevelPopupPanel.SetActive(true);
            nextLevelPopupBG.DORotate(new(0, 0, 360), 5f, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
            nextLevelPopupShimmer.DOAnchorPosX(490, 2.5f).SetDelay(2f).SetLoops(-1, LoopType.Restart);
            nextLevelPopupButton.DOScale(new Vector3(0.8f, 0.8f, 0.8f), 1f).SetLoops(-1, LoopType.Yoyo);
        }
    }



}
