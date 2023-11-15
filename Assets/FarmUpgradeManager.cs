using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FarmUpgradeManager : MonoBehaviour
{
    [Title("Chickens")]
    [SerializeField] GameObject[] chickens;
    int currentChickens = 0;
    [Title("Cows")]
    [SerializeField] GameObject[] cows;
    int currentCows = 0;
    [SerializeField] Image farmUpgradePanel;

    private void OnEnable()
    {
        GameManager.OnGameStateChanged += OpenFarmUpgradeMenu;
    }
    public void OnButtonClick(string button)
    {
        if (button == "Exit")
        {
            CloseUpgradeMenu();
        }
        if (button == "AddChickens")
        {
            AddChickens();
        }
        if (button == "AddCows")
        {
            AddCows();
        }
        if (button == "ChickenTray")
        {
            IncreaseChickenTrayCapacity();
        }
        if (button == "CowTray")
        {
            IncreaseCowTrayCapacity();
        }
    }

    public void IncreaseCowTrayCapacity()
    {

    }

    public void IncreaseChickenTrayCapacity()
    {

    }

    public void AddCows()
    {
        if (currentCows < cows.Length - 1)
        {
            cows[currentCows + 1].SetActive(true);
            currentCows++;
        }
        else
        {
            Debug.LogError("MaxCows");
        }
    }

    public void AddChickens()
    {
        if (currentChickens < chickens.Length - 1)
        {
            chickens[currentChickens + 1].SetActive(true);
            currentChickens++;
        }
        else
        {
            Debug.LogError("MaxChickens");
        }

    }

    void OpenFarmUpgradeMenu(GameState state)
    {
        if (state == GameState.InFarmUpgrade)
            farmUpgradePanel.gameObject.SetActive(true);
    }
    void CloseUpgradeMenu()
    {
        farmUpgradePanel.gameObject.SetActive(false);
        GameManager.Instance.UpdateGameState(GameState.InGame);
    }

}
