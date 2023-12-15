using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TruckUpgradeManager : MonoBehaviour
{
    public static TruckUpgradeManager Instance;
    AudioManager AM;
    AdsManager adsManager;
    public static event Action<int> OnIncreasingRotationSpeed;
    public static event Action<int> OnBuyingUpgrade;
    public static event Action OnIncreasingCarCapacity;

    //===============================================
    public Image truckUpgradePanel;
    [SerializeField] ParticleSystem upgradeParticle;
    public bool UpgradeWithRewardAd;
    //===============================================
    [TabGroup("Collections")][SerializeField] GameObject[] sawBladeUpgrades;
    [TabGroup("Collections")][SerializeField] GameObject[] truckWheelUpgrades;
    int currentSawBlades = 0;
    int currentWheels = 0;
    //===============================================
    [TabGroup("Tutorial References")] public GameObject tutorialFadedPanel;
    [TabGroup("Tutorial References")] public RectTransform tutorialHand;
    [TabGroup("Tutorial References")] public GameObject truckUpgradeZone;

    //===============================================
    [Title("Text Rreferences")]

    [TabGroup("Saw Menu")] public Text sawBlades_CT;
    [TabGroup("Saw Menu")] public Text rotationSpeed_CT;

    [Title("Slider References")]

    [TabGroup("Saw Menu")] public Slider sawBlades_Slider;
    [TabGroup("Saw Menu")] public Slider rotationSpeed_Slider;

    [Title("Maxed Text References")]

    [TabGroup("Saw Menu")] public GameObject sawBlades_maxText;
    [TabGroup("Saw Menu")] public GameObject rotationSpeed_maxText;

    [Title("Upgrade Button References")]

    [TabGroup("Saw Menu")] public GameObject sawBlades_upgradeButtonObject;
    [TabGroup("Saw Menu")] public GameObject rotationSpeed_upgradeButtonObject;

    [Title("Upgrade Reward Button References")]

    [TabGroup("Saw Menu")] public GameObject sawBlades_rewardUpgradeButtonObject;
    [TabGroup("Saw Menu")] public GameObject rotationSpeed_rewardUpgradeButtonObject;

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

    [Title("Maxed Text References")]

    [TabGroup("Truck Menu")] public GameObject carCapacity_maxText;
    [TabGroup("Truck Menu")] public GameObject wheels_maxText;

    [Title("Upgrade Button References")]

    [TabGroup("Truck Menu")] public GameObject carCapacity_upgradeButtonObject;
    [TabGroup("Truck Menu")] public GameObject wheels_upgradeButtonObject;

    [Title("Upgrade Reward Button References")]

    [TabGroup("Truck Menu")] public GameObject carCapacity_rewardUpgradeButtonObject;
    [TabGroup("Truck Menu")] public GameObject wheels_rewardUpgradeButtonObject;

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
    private void OnDisable()
    {
        GameManager.OnGameStateChanged -= OpenTruckUpgradeMenu;
    }
    private void Start()
    {
        AM = AudioManager.instance;
        adsManager = AdsManager.Instance;
        SetDefaultValues();
    }
    private void SetDefaultValues()
    {
        CheckTextColor();

        carCapacity_Slider.minValue = 150;
        //===========================================
        sawBlades_CT.text = "$" + SawBlades_CR.ToString();
        rotationSpeed_CT.text = "$" + rotationSpeed_CR.ToString();
        wheels_CT.text = "$" + wheels_CR.ToString();
        carCapacity_CT.text = "$" + maxCarCapacity_CR.ToString();
        //===========================================
        sawBlades_Slider.maxValue = sawBladeUpgrades.Length - 1;
        sawBlades_Slider.value = currentSawBlades;
        rotationSpeed_Slider.maxValue = maxRotationSpeedUpgrade;
        wheels_Slider.maxValue = truckWheelUpgrades.Length - 1;
        wheels_Slider.value = currentWheels;
        carCapacity_Slider.maxValue = maxCarCapacityUpgrade;
        carCapacity_Slider.value = maxCarCapacity;

    }
    private void CheckTextColor()
    {
        CurrencyManager.Instance.UpdateAffordabilityStatus(sawBlades_CT, SawBlades_CR);
        CurrencyManager.Instance.UpdateAffordabilityStatus(carCapacity_CT, maxCarCapacity);
        CurrencyManager.Instance.UpdateAffordabilityStatus(rotationSpeed_CT, rotationSpeed_CR);
        CurrencyManager.Instance.UpdateAffordabilityStatus(wheels_CT, wheels_CR);
    }
    public void OnButtonClick(string button)
    {
        if (button == "Exit")
        {

            AM.Play("Pop");
            CloseTruckUpgradeMenu();
        }
        if (button == "AddSawBlades")
        {
            AM.Play("Pop");
            VibrationManager.SpecialVibrate(SpecialVibrationTypes.Pop);
            AddBlades();
        }
        if (button == "IncreaseRotationSpeed")
        {
            AM.Play("Pop");
            VibrationManager.SpecialVibrate(SpecialVibrationTypes.Pop);
            IncreaseRotationSpeed(incrementRotationSpeed);
        }
        if (button == "AddWheels")
        {
            AM.Play("Pop");
            VibrationManager.SpecialVibrate(SpecialVibrationTypes.Pop);
            AddWheels();

        }
        if (button == "TutorialAddSawBlades")
        {
            AM.Play("Pop");
            VibrationManager.SpecialVibrate(SpecialVibrationTypes.Pop);
            TutorialAddBlades();

        }
        if (button == "IncreaseCarCapacity")
        {
            AM.Play("Pop");
            VibrationManager.SpecialVibrate(SpecialVibrationTypes.Pop);
            IncreaseCarCapacity(incrementCarCapacity);

        }
        if (button == "RewardIncreaseRotationSpeed")
        {
            AM.Play("Pop");
            VibrationManager.SpecialVibrate(SpecialVibrationTypes.Pop);
            adsManager.ShowRewardedAd("IncreaseRotationSpeed");
        }
        if (button == "RewardAddWheels")
        {
            AM.Play("Pop");
            VibrationManager.SpecialVibrate(SpecialVibrationTypes.Pop);
            adsManager.ShowRewardedAd("AddWheels");
        }
        if (button == "RewardAddSawBlades")
        {
            AM.Play("Pop");
            VibrationManager.SpecialVibrate(SpecialVibrationTypes.Pop);
            adsManager.ShowRewardedAd("AddSawBlades");
        }
        if (button == "RewardIncreaseCarCapacity")
        {
            AM.Play("Pop");
            VibrationManager.SpecialVibrate(SpecialVibrationTypes.Pop);
            adsManager.ShowRewardedAd("IncreaseCarCapacity");
        }

    }
    private void TutorialAddBlades()
    {
        if (PlayerPrefs.GetInt("FirstTimeUpgrading") == 0)
            PlayerPrefs.SetInt("FirstTimeUpgrading", 1);

        sawBladeUpgrades[currentSawBlades].SetActive(false);
        sawBladeUpgrades[currentSawBlades + 1].SetActive(true);
        currentSawBlades++;
        sawBlades_Slider.value = currentSawBlades;
        AM.Play("Upgrade");
        tutorialFadedPanel.SetActive(false);
        DOTween.Kill(tutorialHand);
        PlayerPrefs.SetInt("FirstTimeUpgrading", 1);
        HayStack.instance.UpgradePathDraw.SetActive(false);
    }
    public void IncreaseRotationSpeed(int speed)
    {
        if (PlayerPrefs.GetInt("FirstTimeUpgrading") == 0)
            PlayerPrefs.SetInt("FirstTimeUpgrading", 1);


        if (!UpgradeWithRewardAd)
        {
            if (CurrencyManager.Instance.CheckRequiredCoins(rotationSpeed_CR))
            {
                if (RotationSetter.Instance.RotationSpeed < maxRotationSpeedUpgrade)
                {
                    if (RotationSetter.Instance.RotationSpeed == (maxRotationSpeedUpgrade - speed))
                    {
                        rotationSpeed_upgradeButtonObject.SetActive(false);
                        rotationSpeed_maxText.SetActive(true);
                    }
                    OnIncreasingRotationSpeed?.Invoke(speed);
                    OnBuyingUpgrade?.Invoke(rotationSpeed_CR);
                    rotationSpeed_CR += rotationSpeed_CI;
                    rotationSpeed_CT.text = "$" + rotationSpeed_CR.ToString();
                    rotationSpeed_Slider.value = RotationSetter.Instance.RotationSpeed;
                    CheckTextColor();
                    AM.Play("Upgrade");

                }
                else
                {
                    //Debug.LogError("MaxRotation");
                }
            }
        }
        else
        {
            if (RotationSetter.Instance.RotationSpeed < maxRotationSpeedUpgrade)
            {
                if (RotationSetter.Instance.RotationSpeed == (maxRotationSpeedUpgrade - speed))
                {
                    rotationSpeed_rewardUpgradeButtonObject.SetActive(false);
                    rotationSpeed_maxText.SetActive(true);
                }
                OnIncreasingRotationSpeed?.Invoke(speed);
                rotationSpeed_Slider.value = RotationSetter.Instance.RotationSpeed;
                AM.Play("Upgrade");

            }
            else
            {
                //Debug.LogError("MaxRotation");
            }


        }

    }
    public void IncreaseCarCapacity(int increment)
    {
        if (PlayerPrefs.GetInt("FirstTimeUpgrading") == 0)
            PlayerPrefs.SetInt("FirstTimeUpgrading", 1);


        if (!UpgradeWithRewardAd)
        {
            if (CurrencyManager.Instance.CheckRequiredCoins(maxCarCapacity))
            {
                if (maxCarCapacity < maxCarCapacityUpgrade)
                {
                    if (maxCarCapacity == (maxCarCapacityUpgrade - increment))
                    {
                        carCapacity_upgradeButtonObject.SetActive(false);
                        carCapacity_maxText.SetActive(true);

                    }
                    maxCarCapacity += increment;
                    OnBuyingUpgrade?.Invoke(maxCarCapacity_CR);
                    maxCarCapacity_CR += maxCarCapacity_CI;
                    carCapacity_CT.text = "$" + maxCarCapacity_CR.ToString();
                    carCapacity_Slider.value = maxCarCapacity;
                    OnIncreasingCarCapacity?.Invoke();
                    CheckTextColor();
                    AM.Play("Upgrade");

                }
                else
                {
                    //Debug.LogError("MaxRotation");
                }
            }
        }
        else
        {
            if (maxCarCapacity < maxCarCapacityUpgrade)
            {

                if (maxCarCapacity == (maxCarCapacityUpgrade - increment))
                {
                    carCapacity_rewardUpgradeButtonObject.SetActive(false);
                    carCapacity_maxText.SetActive(true);

                }
                maxCarCapacity += increment;
                carCapacity_Slider.value = maxCarCapacity;
                OnIncreasingCarCapacity?.Invoke();
                AM.Play("Upgrade");
            }

        }


    }
    public void AddWheels()
    {
        if (PlayerPrefs.GetInt("FirstTimeUpgrading") == 0)
            PlayerPrefs.SetInt("FirstTimeUpgrading", 1);


        if (!UpgradeWithRewardAd)
        {

            if (CurrencyManager.Instance.CheckRequiredCoins(wheels_CR))
            {
                if (currentWheels < truckWheelUpgrades.Length - 1)
                {
                    if (currentWheels == (truckWheelUpgrades.Length - 2))
                    {
                        wheels_upgradeButtonObject.SetActive(false);
                        wheels_maxText.SetActive(true);

                    }
                    truckWheelUpgrades[currentWheels].SetActive(false);
                    truckWheelUpgrades[currentWheels + 1].SetActive(true);
                    OnBuyingUpgrade?.Invoke(wheels_CR);
                    wheels_CR += wheels_CI;
                    wheels_CT.text = "$" + wheels_CR.ToString();
                    currentWheels++;
                    wheels_Slider.value = currentWheels;
                    CheckTextColor();
                    AM.Play("Upgrade");

                }
                else
                {
                    //Debug.LogError("MaxWheels");
                }
            }
        }
        else
        {
            if (currentWheels < truckWheelUpgrades.Length - 1)
            {

                if (currentWheels == (truckWheelUpgrades.Length - 2))
                {
                    wheels_rewardUpgradeButtonObject.SetActive(false);
                    wheels_maxText.SetActive(true);

                }
                truckWheelUpgrades[currentWheels].SetActive(false);
                truckWheelUpgrades[currentWheels + 1].SetActive(true);
                currentWheels++;
                wheels_Slider.value = currentWheels;
                AM.Play("Upgrade");
            }

        }

    }
    public void AddBlades()
    {
        if (PlayerPrefs.GetInt("FirstTimeUpgrading") == 0)
            PlayerPrefs.SetInt("FirstTimeUpgrading", 1);



        if (!UpgradeWithRewardAd)
        {

            if (CurrencyManager.Instance.CheckRequiredCoins(SawBlades_CR))
            {
                if (currentSawBlades < sawBladeUpgrades.Length - 1)
                {
                    if (currentSawBlades == (sawBladeUpgrades.Length - 2))
                    {
                        sawBlades_upgradeButtonObject.SetActive(false);
                        sawBlades_maxText.SetActive(true);

                    }

                    sawBladeUpgrades[currentSawBlades].SetActive(false);
                    sawBladeUpgrades[currentSawBlades + 1].SetActive(true);
                    OnBuyingUpgrade?.Invoke(SawBlades_CR);
                    SawBlades_CR += SawBlades_CI;
                    sawBlades_CT.text = "$" + SawBlades_CR.ToString();
                    currentSawBlades++;
                    sawBlades_Slider.value = currentSawBlades;
                    CheckTextColor();
                    AM.Play("Upgrade");


                }
                else
                {
                    //Debug.LogError("MaxBlades");
                }
            }
        }
        else
        {
            if (currentSawBlades < sawBladeUpgrades.Length - 1)
            {

                if (currentSawBlades == (sawBladeUpgrades.Length - 2))
                {
                    sawBlades_rewardUpgradeButtonObject.SetActive(false);
                    sawBlades_maxText.SetActive(true);

                }
                sawBladeUpgrades[currentSawBlades].SetActive(false);
                sawBladeUpgrades[currentSawBlades + 1].SetActive(true);
                currentSawBlades++;
                sawBlades_Slider.value = currentSawBlades;
                AM.Play("Upgrade");
            }

        }

    }
    void OpenTruckUpgradeMenu(GameState state)
    {
        if (state == GameState.Upgrading)
        {
            //InGameUIManager.Instance.inGameUI.SetActive(false);
            //InGameUIManager.Instance.progressBar.SetActive(false);

            if (PlayerPrefs.GetInt("FirstTimeUpgrading") == 0)
            {
                PlayerPrefs.SetInt("FirstTimeUnloading", 1);

                if (HayStack.instance.UpgradePathDraw.activeInHierarchy)
                {
                    HayStack.instance.UpgradePathDraw.SetActive(false);
                    tutorialFadedPanel.SetActive(true);
                    AnimateTutorialHand();
                }
            }
            CheckTextColor();
            truckUpgradePanel.gameObject.SetActive(true);
            ShopManager.instance.shopPanel.gameObject.SetActive(false);

        }
    }
    private void AnimateTutorialHand()
    {
        tutorialHand.DOAnchorPosY(155, 0.5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InQuint);
    }
    void CloseTruckUpgradeMenu()
    {
        //InGameUIManager.Instance.inGameUI.SetActive(true);
        //InGameUIManager.Instance.progressBar.SetActive(true);
        truckUpgradePanel.gameObject.SetActive(false);
        GameManager.Instance.UpdateGameState(GameState.InGame);
    }

}
