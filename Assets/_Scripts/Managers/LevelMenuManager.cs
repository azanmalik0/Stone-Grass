using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class LevelMenuManager : MonoBehaviour
{
    public static LevelMenuManager Instance;
    [TabGroup("Collections")][SerializeField] GameObject[] levelObjects;
    [TabGroup("Collections")][SerializeField] GameObject[] levelButtons;
    //===============================================
    [SerializeField] Image levelSelectionPanel;
    [SerializeField] GameObject loadingPanel;
    [SerializeField] CanvasGroup levelUnlockedPopup;
    //===============================================
    [SerializeField] Sprite currentLevelSprite;

    //===============================================
    public int currentLevel = 0;
    public int loadedLevel = 0;
    //===============================================
    AudioManager AM;
    AdsManager adsManager;
    private void Awake()
    {
        Instance = this;

    }
    private void OnEnable()
    {
        GameManager.OnGameStateChanged += OpenLevelSelectionMenu;
        ProgressBarManager.OnSecondStarUnlock += UnlockNextLevelButton;
        ProgressBarManager.OnSecondStarUnlock += ShowLevelUnlockedPopup;
    }
    private void OnDisable()
    {
        GameManager.OnGameStateChanged -= OpenLevelSelectionMenu;
        ProgressBarManager.OnSecondStarUnlock -= UnlockNextLevelButton;
        ProgressBarManager.OnSecondStarUnlock -= ShowLevelUnlockedPopup;

    }
    private void Start()
    {
        AM = AudioManager.instance;
        adsManager = AdsManager.Instance;
        levelObjects[currentLevel].SetActive(true);
        levelButtons[currentLevel].GetComponent<Image>().sprite = currentLevelSprite;
        adsManager.LogEvent($"game_launched_{currentLevel}");
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
        adsManager.LogEvent($"level_selected_{level}");
        AM.Play("Pop");
        if (currentLevel != level)
        {
            adsManager.ShowNonVideoInterstitialAd();
            // PercentageChecker.Instance.SaveLevelTexture();
            levelObjects[currentLevel].SetActive(false);
            currentLevel = level;
            ES3AutoSaveMgr.Current.Save();
            HayStack.instance.RevertMaterialColour();
            loadingPanel.SetActive(true);
        }
        else if (currentLevel == level)
            levelSelectionPanel.gameObject.SetActive(false);
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
        if (currentLevel < 9)
        {
            levelButtons[currentLevel + 1].transform.GetChild(1).gameObject.SetActive(false);
            levelButtons[currentLevel + 1].GetComponent<Button>().interactable = true;
        }
    }
    void ShowLevelUnlockedPopup()
    {
        print("popup========<<");
        if (PlayerPrefs.GetInt($"PopupShown{LevelMenuManager.Instance.currentLevel}") == 0 && LevelMenuManager.Instance.currentLevel < 9)
        {
            DG.Tweening.Sequence sequence = DOTween.Sequence();
            sequence.Append(levelUnlockedPopup.DOFade(1, 1.5f).SetEase(Ease.Linear))
                .Insert(3, levelUnlockedPopup.DOFade(0, 1.5f).SetEase(Ease.Linear));
            //adsManager.ShowNonVideoInterstitialAd();
            PlayerPrefs.SetInt($"PopupShown{LevelMenuManager.Instance.currentLevel}", 1);
        }
    }
    public void LoadNextLevel()
    {
        adsManager.LogEvent($"level_loaded_{currentLevel}");
        adsManager.ShowNonVideoInterstitialAd();
        levelObjects[currentLevel].SetActive(false);
        currentLevel++;
        ES3AutoSaveMgr.Current.Save();
        HayStack.instance.RevertMaterialColour();
        loadingPanel.SetActive(true);
    }

}
