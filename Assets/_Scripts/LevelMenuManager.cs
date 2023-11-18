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
    //===============================================
    [SerializeField] int currentLevel = 0;
    private void OnEnable()
    {
        GameManager.OnGameStateChanged += OpenLevelSelectionMenu;
        //ProgressBarManager.OnFirstStarUnlock += UnlockNextLevel;
        PercentageChecker.OnFirstStarUnlock += UnlockNextLevel;
    }
    private void OnDisable()
    {

        GameManager.OnGameStateChanged -= OpenLevelSelectionMenu;
       // ProgressBarManager.OnFirstStarUnlock -= UnlockNextLevel;
        PercentageChecker.OnFirstStarUnlock -= UnlockNextLevel;
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

    void UnlockNextLevel()
    {
        levelButtons[currentLevel + 1].transform.GetChild(1).gameObject.SetActive(false);
        levelButtons[currentLevel + 1].GetComponent<Button>().enabled = true;

    }
}
