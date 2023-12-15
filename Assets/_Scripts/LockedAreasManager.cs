using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LockedAreasManager : MonoBehaviour
{
    public static LockedAreasManager Instance;
    AudioManager AM;
    public static event Action<string> AreaUnlocked;
    //=======================================================
    [Title("References")]

    [TabGroup("Henhouse")][SerializeField] Text henhouseLocked_CRT;
    [TabGroup("Henhouse")][SerializeField] GameObject henhouseLockedPanel;
    [TabGroup("Henhouse")][SerializeField] GameObject henhouseInActive;
    [TabGroup("Henhouse")][SerializeField] GameObject henhouseActive;
    [TabGroup("Henhouse")][SerializeField] ParticleSystem henhouse_smokeParticle;
    [Title("Preferences")]
    [TabGroup("Henhouse")][SerializeField] int henhouseLocked_CR;
    //=============================================================
    [Title("References")]

    [TabGroup("Farm")][SerializeField] Text farmLocked_CRT;
    [TabGroup("Farm")][SerializeField] GameObject farmLockedPanel;
    [TabGroup("Farm")][SerializeField] GameObject farmCanUnlockPanel;
    [TabGroup("Farm")][SerializeField] GameObject farmCannotUnlockedPanel;
    [TabGroup("Farm")][SerializeField] GameObject farmInActive;
    [TabGroup("Farm")][SerializeField] GameObject farmActive;
    [TabGroup("Farm")][SerializeField] ParticleSystem farm_smokeParticle;
    [Title("Preferences")]
    [TabGroup("Farm")][SerializeField] int farmLocked_CR;
    //=============================================================
    [Title("References")]

    [TabGroup("Barn")][SerializeField] Text barnLocked_CRT;
    [TabGroup("Barn")][SerializeField] GameObject barnLockedPanel;
    [TabGroup("Barn")][SerializeField] GameObject barnInActive;
    [TabGroup("Barn")][SerializeField] GameObject barnActive;
    [TabGroup("Barn")][SerializeField] ParticleSystem barn_smokeParticle;
    [Title("Preferences")]
    [TabGroup("Barn")][SerializeField] int barnLocked_CR;
    //==============================================================
    [Title("References")]
    [TabGroup("Market")][SerializeField] GameObject marketLockedPanel;
    [TabGroup("Market")][SerializeField] GameObject marketInActive;
    [TabGroup("Market")][SerializeField] GameObject marketActive;
    [TabGroup("Market")][SerializeField] ParticleSystem market_smokeParticle;
    [Title("Preferences")]
    [TabGroup("Market")][SerializeField] int marketLocked_CR;
    //==============================================================

    public int decrementAmount;
    private void Awake()
    {
        Instance = this;
    }
    public bool Loading;
    public bool barnUnlocked;
    public bool farmUnlocked;
    public bool marketUnlocked;
    public bool henhouseUnlocked;
    public bool CanUnlock;
    private void Start()
    {
        AM = AudioManager.instance;
        henhouseLocked_CRT.text = henhouseLocked_CR.ToString();
        barnLocked_CRT.text = barnLocked_CR.ToString();
        farmLocked_CRT.text = farmLocked_CR.ToString();

    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("HenhouseUnlock") && this.CompareTag("Farmer_Stack"))
        {
            if (!Loading)
            {
                Loading = true;
                henhouseLockedPanel.SetActive(true);
                GiveCoinsToHenhouse();
            }

        }
        else if (other.CompareTag("FarmUnlock") && this.CompareTag("Farmer_Stack"))
        {
            if (!Loading)
            {
                farmLockedPanel.SetActive(true);
                Loading = true;
                GiveCoinsToFarm();
            }
        }
        else if (other.CompareTag("BarnUnlock") && this.CompareTag("Farmer_Stack"))
        {
            if (!Loading)
            {
                barnLockedPanel.SetActive(true);
                Loading = true;
                GiveCoinsToBarn();
            }
        }
        else if (other.CompareTag("MarketUnlock") && this.CompareTag("Farmer_Stack"))
        {
            if (!Loading)
            {
                marketLockedPanel.SetActive(true);
                Loading = true;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("HenhouseUnlock") && this.CompareTag("Farmer_Stack"))
        {
             henhouseLockedPanel.SetActive(false);
            Loading = false;
        }
        else if (other.CompareTag("FarmUnlock") && this.CompareTag("Farmer_Stack"))
        {
            farmLockedPanel.SetActive(false);
            Loading = false;
        }
        else if (other.CompareTag("BarnUnlock") && this.CompareTag("Farmer_Stack"))
        {
            barnLockedPanel.SetActive(false);
            Loading = false;
        }
        else if (other.CompareTag("MarketUnlock") && this.CompareTag("Farmer_Stack"))
        {
            marketLockedPanel.SetActive(false);
            Loading = false;
        }

    }
    void GiveCoinsToHenhouse()
    {
        if (CurrencyManager.Instance.coins > 0)
        {
            if (henhouseLocked_CR > 0)
            {

                henhouseLocked_CR -= decrementAmount;
                henhouseLocked_CRT.text = henhouseLocked_CR.ToString();
                CurrencyManager.Instance.DeductCoins(decrementAmount);
                Loading = false;

            }
            else
            {

                if (!henhouseUnlocked)
                {
                    henhouseLockedPanel.SetActive(false);
                    GameManager.Instance.UpdateGameState(GameState.UnlockingArea);
                    UnlockArea("Henhouse");
                }
            }
        }

    }
    void GiveCoinsToBarn()
    {
        if (CurrencyManager.Instance.coins > 0)
        {
            if (barnLocked_CR > 0)
            {
                barnLocked_CR -= decrementAmount;
                barnLocked_CRT.text = barnLocked_CR.ToString();
                CurrencyManager.Instance.DeductCoins(decrementAmount);
                Loading = false;
            }
            else
            {
                if (!barnUnlocked)
                {
                    barnLockedPanel.SetActive(false);
                    GameManager.Instance.UpdateGameState(GameState.UnlockingArea);
                    UnlockArea("Barn");
                }
            }
        }
    }
    void GiveCoinsToFarm()
    {
        if (CurrencyManager.Instance.coins > 0 && CanUnlock)
        {
            if (farmLocked_CR > 0)
            {
                farmLocked_CR -= decrementAmount;
                farmLocked_CRT.text = farmLocked_CR.ToString();
                CurrencyManager.Instance.DeductCoins(decrementAmount);
                Loading = false;

            }
            else
            {
                if (!farmUnlocked)
                {
                    farmLockedPanel.SetActive(false);
                    GameManager.Instance.UpdateGameState(GameState.UnlockingArea);
                    UnlockArea("Farm");
                }
            }


        }


    }
    private void UnlockArea(string area)
    {
        if (area == "Henhouse" || area == "Barn")
        {
            CanUnlock = true;
            farmCannotUnlockedPanel.SetActive(false);
            farmCanUnlockPanel.SetActive(true);

            if (!marketUnlocked)
            {
                UnlockArea("Market");
            }
        }
        if (area == "Henhouse")
        {
            AM.Play("AreaUnlock");
            henhouseInActive.SetActive(false);
            henhouse_smokeParticle.Play();
            henhouseActive.SetActive(true);
            GameManager.Instance.UpdateGameState(GameState.InGame);
            henhouseUnlocked = true;
            AreaUnlocked?.Invoke("Henhouse");
            Loading = false;
        }
        if (area == "Farm")
        {
            farmInActive.SetActive(false);
            farm_smokeParticle.Play();
            farmActive.SetActive(true);
            GameManager.Instance.UpdateGameState(GameState.InGame);
            farmUnlocked = true;
            Loading = false;
        }
        if (area == "Barn")
        {
            AM.Play("AreaUnlock");
            barnInActive.SetActive(false);
            barn_smokeParticle.Play();
            barnActive.SetActive(true);
            GameManager.Instance.UpdateGameState(GameState.InGame);
            barnUnlocked = true;
            AreaUnlocked?.Invoke("Barn");
            Loading = false;
        }
        if (area == "Market")
        {
            marketInActive.SetActive(false);
            market_smokeParticle.Play();
            marketActive.SetActive(true);
            GameManager.Instance.UpdateGameState(GameState.InGame);
            marketUnlocked = true;
            Loading = false;
        }


    }


}
