using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField] Image UpgradePanel;
    [Title("Saw")]
    [SerializeField] GameObject[] sawBladeUpgrades;
    int currentSawBlades = 0;
    [Title("Truck")]
    [SerializeField] GameObject[] truckWheelUpgrades;
    int currentWheels = 0;


    private void OnEnable()
    {
        GameManager.OnGameStateChanged += OpenMenu;
    }

    public void OnButtonClick(string button)
    {
        if (button == "Exit")
        {
            CloseMenu();
        }
        if (button == "AddSawBlades")
        {
            AddBlades();

        }
        if (button == "AddWheels")
        {
            AddWheels();

        }
        if (button == "IncreaseCapacity")
        {
            IncreaseCarCapacity();

        }

    }

    void IncreaseCarCapacity()
    {
        HayStack.instance.maxHayCapacity += 50;

    }
    void AddWheels()
    {
        if (currentWheels < truckWheelUpgrades.Length - 1)
        {
            truckWheelUpgrades[currentWheels].SetActive(false);
            truckWheelUpgrades[currentWheels + 1].SetActive(true);
            currentWheels++;
        }
        else
        {
            Debug.LogError("MaxWheels");
        }

    }
    void AddBlades()
    {
        if (currentSawBlades < sawBladeUpgrades.Length - 1)
        {
            sawBladeUpgrades[currentSawBlades].SetActive(false);
            sawBladeUpgrades[currentSawBlades + 1].SetActive(true);
            currentSawBlades++;
        }
        else
        {
            Debug.LogError("MaxBlades");
        }

    }
    void OpenMenu(GameState state)
    {
        if (state == GameState.Upgrading)
            UpgradePanel.gameObject.SetActive(true);
    }
    void CloseMenu()
    {
        UpgradePanel.gameObject.SetActive(false);
        GameManager.Instance.UpdateGameState(GameState.InGame);
    }
}
