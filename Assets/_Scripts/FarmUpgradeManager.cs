using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FarmUpgradeManager : MonoBehaviour
{
    public static FarmUpgradeManager Instance;
    public static event Action OnIncreasingTrayCapcaity;
    public static event Action OnIncreasingFarmerCapcaity;
    public static event Action OnIncreasingStorageCapcaity;
    public static event Action<int> OnBuyingUpgrade;
    //====================================================
    [SerializeField] Image farmUpgradePanel;
    //====================================================
    [TabGroup("Collections")][SerializeField] GameObject[] chickens;
    [TabGroup("Collections")][SerializeField] GameObject[] cows;
    int currentChickens = 0;
    int currentCows = 0;
    //====================================================
    [Title("Text References")]

    [TabGroup("Animals Menu")] public Text chickenTray_CT;
    [TabGroup("Animals Menu")] public Text chickenNumbers_CT;
    [TabGroup("Animals Menu")] public Text cowTray_CT;
    [TabGroup("Animals Menu")] public Text cowNumbers_CT;

    [Title("Slider References")]
    [TabGroup("Animals Menu")] public Slider cowNumbers_Slider;
    [TabGroup("Animals Menu")] public Slider chickenNumbers_Slider;
    [TabGroup("Animals Menu")] public Slider cowTray_Slider;
    [TabGroup("Animals Menu")] public Slider chickenTray_Slider;

    [Title("Maxed UI References")]

    [TabGroup("Animals Menu")] public GameObject cowNumbers_maxed;
    [TabGroup("Animals Menu")] public GameObject chickenNumbers_maxed;
    [TabGroup("Animals Menu")] public GameObject cowTray_maxed;
    [TabGroup("Animals Menu")] public GameObject chickenTray_maxed;

    [Title("Preferences")]
    [TabGroup("Animals Menu")] public int maxChickenTray_CI;
    [TabGroup("Animals Menu")] public int maxChickenTray_CR;
    [TabGroup("Animals Menu")] public int maxCowTray_CI;
    [TabGroup("Animals Menu")] public int maxCowTray_CR;
    [TabGroup("Animals Menu")] public int cowNumbers_CI;
    [TabGroup("Animals Menu")] public int cowNumbers_CR;
    [TabGroup("Animals Menu")] public int chickenNumbers_CI;
    [TabGroup("Animals Menu")] public int chickenNumbers_CR;
    [TabGroup("Animals Menu")] public int maxChickenTrayUpgrade;
    [TabGroup("Animals Menu")] public int maxCowTrayUpgrade;
    [TabGroup("Animals Menu")] public int maxChickenTrayCapacity;
    [TabGroup("Animals Menu")] public int maxCowTrayCapacity;
    [TabGroup("Animals Menu")] public int incrementCowTrayCapacity;
    [TabGroup("Animals Menu")] public int incrementChickenTrayCapacity;
    //=========================================================
    [Title("Text References")]

    [TabGroup("Capacity Menu")] public Text storageCapacity_CT;
    [TabGroup("Capacity Menu")] public Text farmerCapacity_CT;

    [Title("Slider References")]

    [TabGroup("Capacity Menu")] public Slider storageCapacity_Slider;
    [TabGroup("Capacity Menu")] public Slider farmerCapacity_Slider;

    [Title("Maxed UI References")]

    [TabGroup("Capacity Menu")] public GameObject storageCapacity_maxed;
    [TabGroup("Capacity Menu")] public GameObject farmerCapacity_maxed;

    [Title("Preferences")]
    [TabGroup("Capacity Menu")] public int incrementFarmerCapacity;
    [TabGroup("Capacity Menu")] public int incrementStorageCapacity;
    [TabGroup("Capacity Menu")] public int maxStorageCapacity;
    [TabGroup("Capacity Menu")] public int maxStorageUpgrade;
    [TabGroup("Capacity Menu")] public int maxStorageCapacity_CR;
    [TabGroup("Capacity Menu")] public int maxStorageCapacity_CI;
    [TabGroup("Capacity Menu")] public int maxFarmerCapacity;
    [TabGroup("Capacity Menu")] public int maxFarmerUpgrade;
    [TabGroup("Capacity Menu")] public int maxFarmerCapacity_CR;
    [TabGroup("Capacity Menu")] public int maxFarmerCapacity_CI;


    private void Awake()
    {
        Instance = this;

    }
    private void Start()
    {
        SetDefaultValues();
    }

    private void SetDefaultValues()
    {
        CheckTextColor();

        cowNumbers_CT.text = "$" + cowNumbers_CR.ToString();
        chickenNumbers_CT.text = "$" + chickenNumbers_CR.ToString();
        chickenTray_CT.text = "$" + maxChickenTray_CR.ToString();
        cowTray_CT.text = "$" + maxCowTray_CR.ToString();
        //=====================================
        cowNumbers_Slider.maxValue = cows.Length - 1;
        cowNumbers_Slider.value = currentCows;
        chickenNumbers_Slider.maxValue = chickens.Length - 1;
        chickenNumbers_Slider.value = currentChickens;
        cowTray_Slider.maxValue = maxCowTrayUpgrade;
        cowTray_Slider.minValue = maxCowTrayCapacity;
        chickenTray_Slider.maxValue = maxChickenTrayUpgrade;
        chickenTray_Slider.minValue = maxChickenTrayCapacity;
        //=========================================
        storageCapacity_CT.text = "$" + maxStorageCapacity_CR.ToString();
        farmerCapacity_CT.text = "$" + maxFarmerCapacity_CR.ToString();
        //=========================================
        storageCapacity_Slider.maxValue = maxStorageUpgrade;
        storageCapacity_Slider.minValue = maxStorageCapacity;
        farmerCapacity_Slider.maxValue = maxFarmerUpgrade;
        farmerCapacity_Slider.minValue = maxFarmerCapacity;



    }

    private void CheckTextColor()
    {
        CurrencyManager.UpdateAffordabilityStatus(cowNumbers_CT, cowNumbers_CR);
        CurrencyManager.UpdateAffordabilityStatus(chickenNumbers_CT, chickenNumbers_CR);
        CurrencyManager.UpdateAffordabilityStatus(chickenTray_CT, maxChickenTray_CR);
        CurrencyManager.UpdateAffordabilityStatus(cowTray_CT, maxCowTrayCapacity);
        CurrencyManager.UpdateAffordabilityStatus(storageCapacity_CT, maxStorageCapacity);
        CurrencyManager.UpdateAffordabilityStatus(farmerCapacity_CT, maxFarmerCapacity);
    }

    private void OnEnable()
    {
        GameManager.OnGameStateChanged += OpenFarmUpgradeMenu;
    }
    private void OnDisable()
    {
        GameManager.OnGameStateChanged -= OpenFarmUpgradeMenu;
    }


    public void OnButtonClick(string button)
    {
        if (button == "Exit")
        {
            CloseFarmUpgradeMenu();
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
            IncreaseChickenTrayCapacity(incrementChickenTrayCapacity);
        }
        if (button == "CowTray")
        {
            IncreaseCowTrayCapacity(incrementCowTrayCapacity);
        }
        if (button == "IncreaseStorageCapacity")
        {
            IncreaseStorageCapacity(incrementStorageCapacity);
        }
        if (button == "IncreaseFarmerCapacity")
        {
            IncreaseFarmerCapacity(incrementFarmerCapacity);
        }
    }

    private void IncreaseStorageCapacity(int increment)
    {
        if (CurrencyManager.CheckRequiredCoins(maxStorageCapacity))
        {
            if (maxStorageCapacity < maxStorageUpgrade)
            {
                if (maxStorageCapacity == (maxStorageUpgrade - increment))
                    storageCapacity_maxed.SetActive(true);
                maxStorageCapacity += increment;
                OnBuyingUpgrade?.Invoke(maxStorageCapacity_CR);
                maxStorageCapacity_CR += maxStorageCapacity_CI;
                storageCapacity_CT.text = "$" + maxStorageCapacity_CR.ToString();
                storageCapacity_Slider.value = maxStorageCapacity;
                OnIncreasingStorageCapcaity?.Invoke();
                CheckTextColor();
            }
        }
    }
    private void IncreaseFarmerCapacity(int increment)
    {
        if (CurrencyManager.CheckRequiredCoins(maxFarmerCapacity))
        {

            if (maxFarmerCapacity < maxFarmerUpgrade)
            {
                if (maxFarmerCapacity == (maxFarmerUpgrade - increment))
                    farmerCapacity_maxed.SetActive(true);
                maxFarmerCapacity += increment;
                OnBuyingUpgrade?.Invoke(maxFarmerCapacity_CR);
                maxFarmerCapacity_CR += maxFarmerCapacity_CI;
                farmerCapacity_CT.text = "$" + maxFarmerCapacity_CR.ToString();
                farmerCapacity_Slider.value = maxFarmerCapacity;
                OnIncreasingFarmerCapcaity?.Invoke();
                CheckTextColor();
            }
        }
    }

    void IncreaseCowTrayCapacity(int increment)
    {
        if (CurrencyManager.CheckRequiredCoins(maxCowTrayCapacity))
        {

            if (maxCowTrayCapacity <= maxCowTrayUpgrade)
            {
                if (maxCowTrayCapacity == (maxCowTrayUpgrade - increment))
                    cowTray_maxed.SetActive(true);
                maxCowTrayCapacity += increment;
                OnBuyingUpgrade?.Invoke(maxCowTray_CR);
                maxCowTray_CR += maxCowTray_CI;
                cowTray_CT.text = "$" + maxCowTray_CR.ToString();
                cowTray_Slider.value = maxCowTrayCapacity;
                OnIncreasingTrayCapcaity?.Invoke();
                CheckTextColor();
            }
        }
    }

    void IncreaseChickenTrayCapacity(int increment)
    {
        if (CurrencyManager.CheckRequiredCoins(maxChickenTray_CR))
        {

            if (maxChickenTrayCapacity <= maxChickenTrayUpgrade)
            {
                if (maxChickenTrayCapacity == (maxChickenTrayUpgrade - increment))
                    chickenTray_maxed.SetActive(true);
                maxChickenTrayCapacity += increment;
                OnBuyingUpgrade?.Invoke(maxChickenTray_CR);
                maxChickenTray_CR += maxChickenTray_CI;
                chickenTray_CT.text = "$" + maxChickenTray_CR.ToString();
                chickenTray_Slider.value = maxChickenTrayCapacity;
                OnIncreasingTrayCapcaity?.Invoke();
                CheckTextColor();
            }
        }

    }

    void AddCows()
    {
        if (CurrencyManager.CheckRequiredCoins(cowNumbers_CR))
        {

            if (currentCows < cows.Length - 1)
            {
                if (currentCows == (cows.Length - 2))
                    cowNumbers_maxed.SetActive(true);
                cows[currentCows + 1].SetActive(true);
                OnBuyingUpgrade?.Invoke(cowNumbers_CR);
                cowNumbers_CR += cowNumbers_CI;
                cowNumbers_CT.text = "$" + cowNumbers_CR.ToString();
                currentCows++;
                cowNumbers_Slider.value = currentCows;
                CheckTextColor();
            }
            else
            {
                //Debug.LogError("MaxCows");
            }
        }
    }
    void AddChickens()
    {
        if (CurrencyManager.CheckRequiredCoins(chickenNumbers_CR))
        {

            if (currentChickens < chickens.Length - 1)
            {
                if (currentChickens == (chickens.Length - 2))
                    chickenNumbers_maxed.SetActive(true);
                chickens[currentChickens + 1].SetActive(true);
                OnBuyingUpgrade?.Invoke(chickenNumbers_CR);
                chickenNumbers_CR += chickenNumbers_CI;
                chickenNumbers_CT.text = "$" + chickenNumbers_CR.ToString();
                currentChickens++;
                chickenNumbers_Slider.value = currentChickens;
                CheckTextColor();
            }
            else
            {
                //Debug.LogError("MaxChickens");
            }
        }

    }
    void OpenFarmUpgradeMenu(GameState state)
    {
        if (state == GameState.InFarmUpgrade)
        {
            CheckTextColor();
            farmUpgradePanel.gameObject.SetActive(true);

        }
    }
    void CloseFarmUpgradeMenu()
    {
        farmUpgradePanel.gameObject.SetActive(false);
        GameManager.Instance.UpdateGameState(GameState.InGame);
    }

}
