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
    [SerializeField] Transform farmerCoinPos;
    [SerializeField] GameObject coinPrefab;
    //=======================================================
    [Title("References")]
    [TabGroup("Henhouse")][SerializeField] Transform henhouseCoinPos;
    [TabGroup("Henhouse")][SerializeField] RectTransform henhouseLockedPanel;
    [TabGroup("Henhouse")][SerializeField] Text henhouseLocked_CT;
    [TabGroup("Henhouse")][SerializeField] GameObject henhouseInActive;
    [TabGroup("Henhouse")][SerializeField] GameObject henhouseActive;
    [TabGroup("Henhouse")][SerializeField] ParticleSystem henhouse_smokeParticle;
    [TabGroup("Henhouse")][SerializeField] BoxCollider henhouse_collider;
    [TabGroup("Henhouse")][SerializeField] GameObject henhouse_unlockZone;
    [Title("Preferences")]
    [TabGroup("Henhouse")][SerializeField] int henhouseLocked_CR;
    [TabGroup("Henhouse")][ReadOnly][SerializeField] int henhouseLocked_RM;
    //=============================================================
    [Title("References")]
    [TabGroup("Farm")][SerializeField] Transform farmCoinPos;
    [TabGroup("Farm")][SerializeField] RectTransform farmLockedPanel;
    [TabGroup("Farm")][SerializeField] Text farmLocked_CT;
    [TabGroup("Farm")][SerializeField] GameObject farmInActive;
    [TabGroup("Farm")][SerializeField] GameObject farmActive;
    [TabGroup("Farm")][SerializeField] ParticleSystem farm_smokeParticle;
    [TabGroup("Farm")][SerializeField] BoxCollider farm_collider;
    [TabGroup("Farm")][SerializeField] GameObject farm_unlockZone;
    [Title("Preferences")]
    [TabGroup("Farm")][SerializeField] int farmLocked_CR;
    [TabGroup("Farm")][ReadOnly][SerializeField] int farmLocked_RM;
    //=============================================================
    [Title("References")]
    [TabGroup("Barn")][SerializeField] Transform barnCoinPos;
    [TabGroup("Barn")][SerializeField] RectTransform barnLockedPanel;
    [TabGroup("Barn")][SerializeField] Text barnLocked_CT;
    [TabGroup("Barn")][SerializeField] GameObject barnInActive;
    [TabGroup("Barn")][SerializeField] GameObject barnActive;
    [TabGroup("Barn")][SerializeField] ParticleSystem barn_smokeParticle;
    [TabGroup("Barn")][SerializeField] BoxCollider barn_collider;
    [TabGroup("Barn")][SerializeField] GameObject barn_unlockZone;
    [Title("Preferences")]
    [TabGroup("Barn")][SerializeField] int barnLocked_CR;
    [TabGroup("Barn")][ReadOnly][SerializeField] int barnLocked_RM;
    //==============================================================
    [Title("References")]
    [TabGroup("Market")][SerializeField] Transform marketCoinPos;
    [TabGroup("Market")][SerializeField] RectTransform marketLockedPanel;
    [TabGroup("Market")][SerializeField] Text marketLocked_CT;
    [TabGroup("Market")][SerializeField] GameObject marketInActive;
    [TabGroup("Market")][SerializeField] GameObject marketActive;
    [TabGroup("Market")][SerializeField] ParticleSystem market_smokeParticle;
    [TabGroup("Market")][SerializeField] BoxCollider market_collider;
    [TabGroup("Market")][SerializeField] GameObject market_unlockZone;
    [Title("Preferences")]
    [TabGroup("Market")][SerializeField] int marketLocked_CR;
    [TabGroup("Market")][ReadOnly][SerializeField] int marketLocked_RM;
    //==============================================================
    private void Awake()
    {
        Instance = this;
    }
    public bool Loading;
    public bool barnUnlocked;
    public bool farmUnlocked;
    public bool marketUnlocked;
    public bool henhouseUnlocked;

    private void Start()
    {
        henhouseLocked_CT.text = henhouseLocked_RM + "/" + henhouseLocked_CR;
        farmLocked_CT.text = farmLocked_RM + "/" + farmLocked_CR;
        barnLocked_CT.text = barnLocked_RM + "/" + barnLocked_CR;
        marketLocked_CT.text = marketLocked_RM + "/" + marketLocked_CR;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("HenhouseUnlock") && this.CompareTag("Farmer_Stack"))
        {
            if (!Loading)
            {
                Loading = true;
                GiveCoinsToHenhouse();
            }

        }
        else if (other.CompareTag("FarmUnlock") && this.CompareTag("Farmer_Stack"))
        {
            if (!Loading)
            {
                Loading = true;
                GiveCoinsToFarm();
            }
        }
        else if (other.CompareTag("BarnUnlock") && this.CompareTag("Farmer_Stack"))
        {
            if (!Loading)
            {
                Loading = true;
                GiveCoinsToBarn();
            }
        }
        else if (other.CompareTag("MarketUnlock") && this.CompareTag("Farmer_Stack"))
        {
            if (!Loading)
            {
                Loading = true;
                GiveCoinsToMarket();
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("HenhouseUnlock") && this.CompareTag("Farmer_Stack"))
        {
            Loading = false;
        }
        else if (other.CompareTag("FarmUnlock") && this.CompareTag("Farmer_Stack"))
        {
            Loading = false;
        }
        else if (other.CompareTag("BarnUnlock") && this.CompareTag("Farmer_Stack"))
        {
            Loading = false;
        }
        else if (other.CompareTag("MarketUnlock") && this.CompareTag("Farmer_Stack"))
        {
            Loading = false;
        }

    }
    void GiveCoinsToMarket()
    {

        if (CurrencyManager.coins > 0)
        {
            if (marketLocked_RM < marketLocked_CR)
            {
                marketLocked_RM++;
                marketLocked_CT.text = marketLocked_RM + "/" + marketLocked_CR;
                CurrencyManager.Instance.DeductCoins(1);
                Loading = false;

            }
            else
            {
                if (!marketUnlocked)
                {

                    GameManager.Instance.UpdateGameState(GameState.UnlockingArea);
                    UnlockArea("Market");
                }

            }




        }
    }
    void GiveCoinsToHenhouse()
    {



        if (CurrencyManager.coins > 0)
        {
            if (henhouseLocked_RM < henhouseLocked_CR)
            {

                henhouseLocked_RM++;
                henhouseLocked_CT.text = henhouseLocked_RM + "/" + henhouseLocked_CR;
                CurrencyManager.Instance.DeductCoins(1);
                Loading = false;

            }
            else
            {

                if (!henhouseUnlocked)
                {
                    GameManager.Instance.UpdateGameState(GameState.UnlockingArea);
                    UnlockArea("Henhouse");
                }
            }


        }




    }
    void GiveCoinsToFarm()
    {
        // Debug.LogError("Coins available = > " + CurrencyManager.coins);
        if (CurrencyManager.coins > 0)
        {
            if (farmLocked_RM < farmLocked_CR)
            {
                farmLocked_RM++;
                farmLocked_CT.text = farmLocked_RM + "/" + farmLocked_CR;
                CurrencyManager.Instance.DeductCoins(1);
                Loading = false;

            }
            else
            {
                if (!farmUnlocked)
                {
                    GameManager.Instance.UpdateGameState(GameState.UnlockingArea);
                    UnlockArea("Farm");
                }
            }


        }


    }
    void GiveCoinsToBarn()
    {
        if (CurrencyManager.coins > 0)
        {
            if (barnLocked_RM < barnLocked_CR)
            {
                barnLocked_RM++;
                barnLocked_CT.text = barnLocked_RM + "/" + barnLocked_CR;
                CurrencyManager.Instance.DeductCoins(1);
                Loading = false;
            }
            else
            {
                if (!barnUnlocked)
                {
                    GameManager.Instance.UpdateGameState(GameState.UnlockingArea);
                    UnlockArea("Barn");
                }
            }
        }
    }
    private void UnlockArea(string area)
    {
        if (area == "Henhouse")
        {
            henhouseInActive.SetActive(false);
            henhouse_smokeParticle.Play();
            henhouseActive.SetActive(true);
            market_unlockZone.SetActive(true);
            GameManager.Instance.UpdateGameState(GameState.InGame);
            henhouseUnlocked = true;
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

            barnInActive.SetActive(false);
            barn_smokeParticle.Play();
            barnActive.SetActive(true);
            farm_unlockZone.SetActive(true);
            GameManager.Instance.UpdateGameState(GameState.InGame);
            barnUnlocked = true;
            Loading = false;
        }
        if (area == "Market")
        {

            marketInActive.SetActive(false);
            market_smokeParticle.Play();
            marketActive.SetActive(true);
            barn_unlockZone.SetActive(true);
            GameManager.Instance.UpdateGameState(GameState.InGame);
            marketUnlocked = true;
            Loading = false;
        }


    }


}
