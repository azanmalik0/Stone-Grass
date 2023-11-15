using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    [Title("Preferences")]
    public int carCapacityIncrement;
    [Title("UI References")]
    [SerializeField] Image UpgradePanel;
    [Title("Saw")]
    [SerializeField] GameObject[] sawBladeUpgrades;
    int currentSawBlades = 0;
    [Title("Truck")]
    [SerializeField] GameObject[] truckWheelUpgrades;
    int currentWheels = 0;


    private void OnEnable()
    {
        GameManager.OnGameStateChanged += OpenTruckUpgradeMenu;
    }

    public void OnButtonClick(string button)
    {
        if (button == "Exit")
        {
            CloseTruckUpgradeMenu();
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
            IncreaseCarCapacity(carCapacityIncrement);

        }

    }

    void IncreaseCarCapacity(int capacity)
    {
        HayStack.instance.maxHayCapacity += capacity;

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
    void OpenTruckUpgradeMenu(GameState state)
    {
        if (state == GameState.Upgrading)
            UpgradePanel.gameObject.SetActive(true);
    }
    void CloseTruckUpgradeMenu()
    {
        UpgradePanel.gameObject.SetActive(false);
        GameManager.Instance.UpdateGameState(GameState.InGame);
    }
}
