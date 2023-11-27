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
    public static int currentLevel = 0;
    public static int loadedLevel = 0;
    private void Awake()
    {
        Instance = this;

        
           loadedLevel = currentLevel;
        PlayerPrefs.SetInt("CurrentPlayingLevel", loadedLevel);
      
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
      //  ES3AutoSaveMgr.Current.Save();

    }
    private void Start()
    {

        for (int i = 0; i <= PlayerPrefs.GetInt("TotalLevelsUnlocked"); i++)
        {
            levelButtons[i].transform.GetChild(1).gameObject.SetActive(false);
            levelButtons[i].GetComponent<Button>().enabled = true;
        }
        levelObjects[PlayerPrefs.GetInt("CurrentPlayingLevel")].SetActive(true);
     //   UnlockNextLevelButton();
        
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

            //levelObjects[level].SetActive(true);
            levelObjects[currentLevel].SetActive(false);
            currentLevel = level;
        }

        loadingPanel.SetActive(true);
        

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
        //Debug.LogError("123 = > " + PlayerPrefs.GetInt("TotalLevelsUnlocked"));                      // 0
        //levelButtons[currentLevel + 1].transform.GetChild(1).gameObject.SetActive(false);
        //levelButtons[currentLevel + 1].GetComponent<Button>().enabled = true;
        
        if(currentLevel == PlayerPrefs.GetInt("TotalLevelsUnlocked"))
        {
            PlayerPrefs.SetInt("TotalLevelsUnlocked" , PlayerPrefs.GetInt("TotalLevelsUnlocked") + 1);
            //Debug.LogError(" Level Barh gaya ");
            for (int i = 0; i <= PlayerPrefs.GetInt("TotalLevelsUnlocked"); i++)
            {
                levelButtons[i].transform.GetChild(1).gameObject.SetActive(false);
                levelButtons[i].GetComponent<Button>().enabled = true;
            }
        }
        else
        {
            //Debug.LogError(" Turr ja Turr ja ");
        }
           
        
    }
}
