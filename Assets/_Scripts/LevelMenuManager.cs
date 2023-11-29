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
    [TabGroup("Collections")][SerializeField] GameObject[] levelObjects;
    [TabGroup("Collections")][SerializeField] GameObject[] levelButtons;
    //===============================================
    [SerializeField] Image levelSelectionPanel;
    [SerializeField] GameObject loadingPanel;
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
        PercentageChecker.OnFirstStarUnlock += UnlockNextLevelButton;
    }
    private void OnDisable()
    {
        GameManager.OnGameStateChanged -= OpenLevelSelectionMenu;
        PercentageChecker.OnFirstStarUnlock -= UnlockNextLevelButton;

    }
    private void Start()
    {
        // ES3AutoSaveMgr.Current.Load();
        levelObjects[currentLevel].SetActive(true);

        //==============Hasnat=======================================
        //loadedLevel = currentLevel;
        //PlayerPrefs.SetInt("CurrentPlayingLevel", loadedLevel);
        //levelObjects[PlayerPrefs.GetInt("CurrentPlayingLevel")].SetActive(true);


        //for (int i = 0; i <= PlayerPrefs.GetInt("TotalLevelsUnlocked"); i++)
        //{
        //    levelButtons[i].transform.GetChild(1).gameObject.SetActive(false);
        //    levelButtons[i].GetComponent<Button>().enabled = true;
        //}

    }
    public void OnButtonClick(string button)
    {
        if (button == "Exit")
        {
            CloseLevelSelectionMenu();
        }

    }


    public void LoadLevel(int level)
    {

        if (currentLevel != level)
        {
            PercentageChecker.Instance.SaveLevelTexture();
            levelObjects[currentLevel].SetActive(false);
            currentLevel = level;
            print("CurrentLevelSaved" + " " + currentLevel);
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

        //=================Hasnat============================================
        //if (currentLevel == PlayerPrefs.GetInt("TotalLevelsUnlocked"))
        //{
        //    PlayerPrefs.SetInt("TotalLevelsUnlocked", PlayerPrefs.GetInt("TotalLevelsUnlocked") + 1);
        //    Debug.LogError(" Level Barh gaya ");
        //    for (int i = 0; i <= PlayerPrefs.GetInt("TotalLevelsUnlocked"); i++)
        //    {
        //        levelButtons[i].transform.GetChild(1).gameObject.SetActive(false);
        //        levelButtons[i].GetComponent<Button>().enabled = true;
        //    }
        //}
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {

            // ES3AutoSaveMgr.Current.Save();
        }
    }


}
