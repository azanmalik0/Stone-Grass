using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TruckUpgradeManager : MonoBehaviour
{
    public static TruckUpgradeManager Instance;
    public static event Action<int> OnIncreasingRotationSpeed;
    public static event Action<int> OnBuyingUpgrade;
    public static event Action OnIncreasingCarCapacity;

    //===============================================
    [SerializeField] Image truckUpgradePanel;
    //===============================================
    [TabGroup("Collections")][SerializeField] GameObject[] sawBladeUpgrades;
    [TabGroup("Collections")][SerializeField] GameObject[] truckWheelUpgrades;
    int currentSawBlades = 0;
    int currentWheels = 0;
    //===============================================
    [Title("Text Rreferences")]

    [TabGroup("Saw Menu")] public Text sawBlades_CT;
    [TabGroup("Saw Menu")] public Text rotationSpeed_CT;

    [Title("Slider References")]

    [TabGroup("Saw Menu")] public Slider sawBlades_Slider;
    [TabGroup("Saw Menu")] public Slider rotationSpeed_Slider;

    [Title("Preferences")]

    [TabGroup("Saw Menu")] public int SawBlades_CR;
    [TabGroup("Saw Menu")] public int SawBlades_CI;
    [TabGroup("Saw Menu")] public int rotationSpeed_CR;
    [TabGroup("Saw Menu")] public int rotationSpeed_CI;
    [TabGroup("Saw Menu")] public int incrementRotationSpeed;
    [TabGroup("Saw Menu")] public int maxRotationSpeedUpgrade;

    //===============================================
    [Title("Text Rreferences")]

    [TabGroup("Truck Menu")] public Text carCapacity_CT;
    [TabGroup("Truck Menu")] public Text wheels_CT;

    [Title("Slider Rreferences")]

    [TabGroup("Truck Menu")] public Slider carCapacity_Slider;
    [TabGroup("Truck Menu")] public Slider wheels_Slider;

    [Title("Preferences")]

    [TabGroup("Truck Menu")] public int incrementCarCapacity;
    [TabGroup("Truck Menu")] public int wheels_CR;
    [TabGroup("Truck Menu")] public int wheels_CI;
    [TabGroup("Truck Menu")] public int maxCarCapacity;
    [TabGroup("Truck Menu")] public int maxCarCapacityUpgrade;
    [TabGroup("Truck Menu")] public int maxCarCapacity_CR;
    [TabGroup("Truck Menu")] public int maxCarCapacity_CI;


    private void Awake()
    {
        Instance = this;
    }
    private void OnEnable()
    {
        GameManager.OnGameStateChanged += OpenTruckUpgradeMenu;
    }
    private void Start()
    {
        ES3AutoSaveMgr.Current.Load();
        SetDefaultValues();
    }

    private void SetDefaultValues()
    {

        sawBlades_CT.text = "$" + SawBlades_CR.ToString();
        rotationSpeed_CT.text = "$" + rotationSpeed_CR.ToString();
        //===========================================
        sawBlades_Slider.maxValue = sawBladeUpgrades.Length - 1;
        sawBlades_Slider.value = currentSawBlades;
        rotationSpeed_Slider.maxValue = maxRotationSpeedUpgrade;
        //rotationSpeed_Slider.minValue = RotationSetter.Instance.RotationSpeed;
        // rotationSpeed_Slider.value = 0;
        //================================================
        wheels_CT.text = "$" + wheels_CR.ToString();
        carCapacity_CT.text = "$" + maxCarCapacity_CR.ToString();
        //================================================
        wheels_Slider.maxValue = truckWheelUpgrades.Length - 1;
        wheels_Slider.value = currentWheels;
        carCapacity_Slider.maxValue = maxCarCapacityUpgrade;
        carCapacity_Slider.minValue = maxCarCapacity;
        // carCapacity_Slider.value = 0;

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
        if (button == "IncreaseRotationSpeed")
        {
            IncreaseRotationSpeed(incrementRotationSpeed);

        }
        if (button == "AddWheels")
        {
            AddWheels();

        }
        if (button == "IncreaseCarCapacity")
        {
            IncreaseCarCapacity(incrementCarCapacity);

        }

    }
    private void IncreaseRotationSpeed(int speed)
    {
        //if (CurrencyManager.CheckRequiredCoins(rotationSpeed_CR))
        // {
        if (RotationSetter.Instance.RotationSpeed < maxRotationSpeedUpgrade)
        {

            OnIncreasingRotationSpeed?.Invoke(speed);
            OnBuyingUpgrade?.Invoke(rotationSpeed_CR);
            rotationSpeed_CR += rotationSpeed_CI;
            rotationSpeed_CT.text = "$" + rotationSpeed_CR.ToString();
            rotationSpeed_Slider.value = RotationSetter.Instance.RotationSpeed;
        }
        else
        {
            Debug.LogError("MaxRotation");
        }
        // }

    }
    void IncreaseCarCapacity(int increment)
    {
        // if (CurrencyManager.CheckRequiredCoins(maxCarCapacity))
        // {
        if (maxCarCapacity < maxCarCapacityUpgrade)
        {
            maxCarCapacity += increment;
            OnBuyingUpgrade?.Invoke(maxCarCapacity_CR);
            maxCarCapacity_CR += maxCarCapacity_CI;
            carCapacity_CT.text = "$" + maxCarCapacity_CR.ToString();
            carCapacity_Slider.value = maxCarCapacity;
            OnIncreasingCarCapacity?.Invoke();
        }
        else
        {
            Debug.LogError("MaxRotation");
        }
        // }


    }
    void AddWheels()
    {
        // if (CurrencyManager.CheckRequiredCoins(wheels_CR))
        // {
        if (currentWheels < truckWheelUpgrades.Length - 1)
        {
            truckWheelUpgrades[currentWheels].SetActive(false);
            truckWheelUpgrades[currentWheels + 1].SetActive(true);
            OnBuyingUpgrade?.Invoke(wheels_CR);
            wheels_CR += wheels_CI;
            wheels_CT.text = "$" + wheels_CR.ToString();
            currentWheels++;
            wheels_Slider.value = currentWheels;
        }
        else
        {
            Debug.LogError("MaxWheels");
        }
        // }

    }
    void AddBlades()
    {
        // if (CurrencyManager.CheckRequiredCoins(SawBlades_CR))
        // {
        if (currentSawBlades < sawBladeUpgrades.Length - 1)
        {
            sawBladeUpgrades[currentSawBlades].SetActive(false);
            sawBladeUpgrades[currentSawBlades + 1].SetActive(true);
            OnBuyingUpgrade?.Invoke(SawBlades_CR);
            SawBlades_CR += SawBlades_CI;
            sawBlades_CT.text = "$" + SawBlades_CR.ToString();
            currentSawBlades++;
            sawBlades_Slider.value = currentSawBlades;
        }
        else
        {
            Debug.LogError("MaxBlades");
        }
        // }

    }
    void OpenTruckUpgradeMenu(GameState state)
    {
        if (state == GameState.Upgrading)
            truckUpgradePanel.gameObject.SetActive(true);
    }
    void CloseTruckUpgradeMenu()
    {
        truckUpgradePanel.gameObject.SetActive(false);
        GameManager.Instance.UpdateGameState(GameState.InGame);
    }
    private void OnApplicationQuit()
    {
        ES3AutoSaveMgr.Current.Save();
    }
}
