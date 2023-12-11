using DG.Tweening;
using PT.Garden;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LevelMenuManager : MonoBehaviour
{
    public static LevelMenuManager Instance;
    AudioManager AM;
    [TabGroup("Collections")][SerializeField] GameObject[] levelObjects;
    [TabGroup("Collections")][SerializeField] GameObject[] levelButtons;
    //===============================================
    [SerializeField] Image levelSelectionPanel;
    [SerializeField] GameObject loadingPanel;
    [SerializeField] CanvasGroup levelUnlockedPopup;
    //===============================================
    public int currentLevel = 0;
    public int loadedLevel = 0;
    private void Awake()
    {
        Instance = this;

    }
    private void OnEnable()
    {
        GameManager.OnGameStateChanged += OpenLevelSelectionMenu;
        ProgressBarManager.OnFirstStarUnlock += UnlockNextLevelButton;
        ProgressBarManager.OnFirstStarUnlock += ShowLevelUnlockedPopup;
    }
    private void OnDisable()
    {
        GameManager.OnGameStateChanged -= OpenLevelSelectionMenu;
        ProgressBarManager.OnFirstStarUnlock -= UnlockNextLevelButton;
        ProgressBarManager.OnFirstStarUnlock -= ShowLevelUnlockedPopup;

    }
    private void Start()
    {
        AM = AudioManager.instance;
        levelObjects[currentLevel].SetActive(true);
    }
    public void OnButtonClick(string button)
    {
        if (button == "Exit")
        {
            AM.Play("Pop");
            CloseLevelSelectionMenu();
        }

    }


    public void LoadLevel(int level)
    {
        AM.Play("Pop");
        if (currentLevel != level)
        {
            //PercentageChecker.Instance.SaveLevelTexture();
            levelObjects[currentLevel].SetActive(false);
            currentLevel = level;
            ES3AutoSaveMgr.Current.Save();
            loadingPanel.SetActive(true);
        }
    }

    void OpenLevelSelectionMenu(GameState state)
    {
        if (state == GameState.InLevelMenu)
            levelSelectionPanel.gameObject.SetActive(true);
    }
    void CloseLevelSelectionMenu()
    {
        levelSelectionPanel.gameObject.SetActive(false);
        GameManager.Instance.UpdateGameState(GameState.InGame);
    }

    void UnlockNextLevelButton()
    {
        levelButtons[currentLevel + 1].transform.GetChild(1).gameObject.SetActive(false);
        levelButtons[currentLevel + 1].GetComponent<Button>().interactable = true;
    }

    void ShowLevelUnlockedPopup()
    {
        print("popup========<<");
        if (PlayerPrefs.GetInt($"PopupShown{LevelMenuManager.Instance.currentLevel}") == 0 && LevelMenuManager.Instance.currentLevel < 9)
        {
            DG.Tweening.Sequence sequence = DOTween.Sequence();
            sequence.Append(levelUnlockedPopup.DOFade(1, 1.5f).SetEase(Ease.Linear))
                .Insert(3, levelUnlockedPopup.DOFade(0, 1.5f).SetEase(Ease.Linear));

            PlayerPrefs.SetInt($"PopupShown{LevelMenuManager.Instance.currentLevel}", 1);
        }
    }
}
