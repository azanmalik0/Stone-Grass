using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FarmUpgradeManager : MonoBehaviour
{
    public static FarmUpgradeManager Instance;
    public static event Action OnIncreasingTrayCapcaity;
    public static event Action<int> OnBuyingUpgrade;
    //====================================================
    [SerializeField] Image farmUpgradePanel;
    [TabGroup("Collections")][SerializeField] GameObject[] chickens;
    int currentChickens = 0;
    [TabGroup("Collections")][SerializeField] GameObject[] cows;
    int currentCows = 0;
    //====================================================
    [Title("Text References")]

    [TabGroup("Animals Menu")] public Text chickenTray_CT;
    [TabGroup("Animals Menu")] public Text chickenNumbers_CT;
    [TabGroup("Animals Menu")] public Text cowTray_CT;
    [TabGroup("Animals Menu")] public Text cowNumbers_CT;

    [Title("Filler Image References")]
    [TabGroup("Animals Menu")] public Slider cowNumbers_Slider;
    [TabGroup("Animals Menu")] public Slider chickenNumbers_Slider;
    [TabGroup("Animals Menu")] public Slider cowTray_Slider;
    [TabGroup("Animals Menu")] public Slider chickenTray_Slider;

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
    [TabGroup("Capacity Menu")]

    private void Awake()
    {
        Instance = this;

    }
    private void Start()
    {
        Init();
    }

    private void Init()
    {
        cowNumbers_CT.text = cowNumbers_CR.ToString();
        chickenNumbers_CT.text = chickenNumbers_CR.ToString();
        chickenTray_CT.text = maxChickenTray_CR.ToString();
        cowTray_CT.text = maxCowTray_CR.ToString();
        //=====================================
        cowNumbers_Slider.maxValue = cows.Length - 1;
        cowNumbers_Slider.value = currentCows;
        chickenNumbers_Slider.maxValue = chickens.Length - 1;
        chickenNumbers_Slider.value = currentChickens;
        cowTray_Slider.maxValue = maxCowTrayUpgrade;
        cowTray_Slider.minValue = maxCowTrayCapacity;
        cowTray_Slider.value = 0;
        chickenTray_Slider.maxValue = maxChickenTrayUpgrade;
        chickenTray_Slider.minValue = maxChickenTrayCapacity;
        chickenTray_Slider.value = 0;

    }

    private void OnEnable()
    {
        GameManager.OnGameStateChanged += OpenFarmUpgradeMenu;
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
    }

    void IncreaseCowTrayCapacity(int increment)
    {
        if (CurrencyManager.CheckRequiredCoins(maxCowTrayCapacity))
        {

            if (maxCowTrayCapacity <= maxCowTrayUpgrade)
            {
                maxCowTrayCapacity += increment;
                OnBuyingUpgrade?.Invoke(maxCowTray_CR);
                maxCowTray_CR += maxCowTray_CI;
                cowTray_CT.text = maxCowTray_CR.ToString();
                cowTray_Slider.value = maxCowTrayCapacity;
                OnIncreasingTrayCapcaity?.Invoke();
            }
        }
    }

    void IncreaseChickenTrayCapacity(int increment)
    {
        if (CurrencyManager.CheckRequiredCoins(maxChickenTray_CR))
        {

            if (maxChickenTrayCapacity <= maxChickenTrayUpgrade)
            {
                maxChickenTrayCapacity += increment;
                OnBuyingUpgrade?.Invoke(maxChickenTray_CR);
                maxChickenTray_CR += maxChickenTray_CI;
                chickenTray_CT.text = maxChickenTray_CR.ToString();
                chickenTray_Slider.value = maxChickenTrayCapacity;
                OnIncreasingTrayCapcaity?.Invoke();
            }
        }

    }

    void AddCows()
    {
        if (CurrencyManager.CheckRequiredCoins(cowNumbers_CR))
        {

            if (currentCows < cows.Length - 1)
            {
                cows[currentCows + 1].SetActive(true);
                OnBuyingUpgrade?.Invoke(cowNumbers_CR);
                cowNumbers_CR += cowNumbers_CI;
                cowNumbers_CT.text = cowNumbers_CR.ToString();
                currentCows++;
                cowNumbers_Slider.value = currentCows;
            }
            else
            {
                Debug.LogError("MaxCows");
            }
        }
    }
    void AddChickens()
    {
        if (CurrencyManager.CheckRequiredCoins(chickenNumbers_CR))
        {

            if (currentChickens < chickens.Length - 1)
            {
                chickens[currentChickens + 1].SetActive(true);
                OnBuyingUpgrade?.Invoke(chickenNumbers_CR);
                chickenNumbers_CR += chickenNumbers_CI;
                chickenNumbers_CT.text = chickenNumbers_CR.ToString();
                currentChickens++;
                chickenNumbers_Slider.value = currentChickens;
            }
            else
            {
                Debug.LogError("MaxChickens");
            }
        }

    }
    void OpenFarmUpgradeMenu(GameState state)
    {
        if (state == GameState.InFarmUpgrade)
            farmUpgradePanel.gameObject.SetActive(true);
    }
    void CloseFarmUpgradeMenu()
    {
        farmUpgradePanel.gameObject.SetActive(false);
        GameManager.Instance.UpdateGameState(GameState.InGame);
    }

}
