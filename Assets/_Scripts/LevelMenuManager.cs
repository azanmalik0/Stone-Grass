using PT.Garden;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelMenuManager : MonoBehaviour
{
    [TabGroup("Collections")][SerializeField] GameObject[] levelObjects;
    [TabGroup("Collections")][SerializeField] GameObject[] levelButtons;
    //===============================================
    [SerializeField] Image levelSelectionPanel;
    [SerializeField] GameObject loadingPanel;
    //===============================================
    [SerializeField] int currentLevel = 0;
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

            levelObjects[level].SetActive(true);
            levelObjects[currentLevel].SetActive(false);
            currentLevel = level;
        }
        loadingPanel.SetActive(true);
        ES3AutoSaveMgr.Current.Save();

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
        levelButtons[currentLevel + 1].GetComponent<Button>().enabled = true;

    }
}
