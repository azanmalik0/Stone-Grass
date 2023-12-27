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
    AdsManager adsManager;
    public static event Action<string> AreaUnlocked;
    //=======================================================
    [Title("References")]

    [TabGroup("Henhouse")][SerializeField] Text henhouseLocked_CRT;
    [TabGroup("Henhouse")][SerializeField] RectTransform henhouseLockedPanel;
    [TabGroup("Henhouse")][SerializeField] GameObject henhouseInActive;
    [TabGroup("Henhouse")][SerializeField] GameObject henhouseActive;
    [TabGroup("Henhouse")][SerializeField] ParticleSystem henhouse_smokeParticle;
    [Title("Preferences")]
    [TabGroup("Henhouse")][SerializeField] int henhouseLocked_CR;
    //=============================================================
    [Title("References")]

    [TabGroup("Farm")][SerializeField] Text farmLocked_CRT;
    [TabGroup("Farm")][SerializeField] RectTransform farmLockedPanel;
    [TabGroup("Farm")][SerializeField] RectTransform farmCanUnlockPanel;
    [TabGroup("Farm")][SerializeField] GameObject farmCannotUnlockedPanel;
    [TabGroup("Farm")][SerializeField] GameObject farmInActive;
    [TabGroup("Farm")][SerializeField] GameObject farmActive;
    [TabGroup("Farm")][SerializeField] ParticleSystem farm_smokeParticle;
    [Title("Preferences")]
    [TabGroup("Farm")][SerializeField] int farmLocked_CR;
    //=============================================================
    [Title("References")]

    [TabGroup("Barn")][SerializeField] Text barnLocked_CRT;
    [TabGroup("Barn")][SerializeField] RectTransform barnLockedPanel;
    [TabGroup("Barn")][SerializeField] GameObject barnInActive;
    [TabGroup("Barn")][SerializeField] GameObject barnActive;
    [TabGroup("Barn")][SerializeField] ParticleSystem barn_smokeParticle;
    [Title("Preferences")]
    [TabGroup("Barn")][SerializeField] int barnLocked_CR;
    //==============================================================
    [Title("References")]
    [TabGroup("Market")][SerializeField] RectTransform marketLockedPanel;
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
        adsManager = AdsManager.Instance;
        henhouseLocked_CRT.text = henhouseLocked_CR.ToString();
        barnLocked_CRT.text = barnLocked_CR.ToString();
        farmLocked_CRT.text = farmLocked_CR.ToString();

    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HenhouseUnlock") && this.CompareTag("Farmer_Stack"))
        {
            if (!IsCounting)
            {
                henhouseLockedPanel.DOAnchorPosY(606.37f, 0.3f).SetEase(Ease.OutBack).SetDelay(0.5f).SetAutoKill(false).OnComplete(() => StartCoroutine(GiveCoinsToHenhouse()));
            }

        }
        else if (other.CompareTag("FarmUnlock") && this.CompareTag("Farmer_Stack"))
        {
            if (!IsCounting)
            {
                farmLockedPanel.DOAnchorPosY(606.37f, 0.3f).SetEase(Ease.OutBack).SetDelay(0.5f).SetAutoKill(false).OnComplete(() => StartCoroutine(GiveCoinsToFarm()));
            }
        }

        else if (other.CompareTag("BarnUnlock") && this.CompareTag("Farmer_Stack"))
        {
            if (!IsCounting)
            {
                barnLockedPanel.DOAnchorPosY(606.37f, 0.3f).SetEase(Ease.OutBack).SetDelay(0.5f).SetAutoKill(false).OnComplete(() => StartCoroutine(GiveCoinsToBarn()));

            }
        }
        else if (other.CompareTag("MarketUnlock") && this.CompareTag("Farmer_Stack"))
        {
            marketLockedPanel.DOAnchorPosY(606.37f, 0.3f).SetEase(Ease.OutBack).SetDelay(0.5f).SetAutoKill(false);
        }
    }
    bool IsCounting;
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("HenhouseUnlock") && this.CompareTag("Farmer_Stack"))
        {
            DOTween.SmoothRewind(henhouseLockedPanel);
            AM.Stop("Count");
            IsCounting = false;
            print("Exit");
        }
        else if (other.CompareTag("FarmUnlock") && this.CompareTag("Farmer_Stack"))
        {
            DOTween.SmoothRewind(farmLockedPanel);
            AM.Stop("Count");
            IsCounting = false;
            print("Exit");
        }
        else if (other.CompareTag("BarnUnlock") && this.CompareTag("Farmer_Stack"))
        {
            DOTween.SmoothRewind(barnLockedPanel);
            AM.Stop("Count");
            IsCounting = false;
            print("Exit");
        }
        else if (other.CompareTag("MarketUnlock") && this.CompareTag("Farmer_Stack"))
        {
            DOTween.SmoothRewind(marketLockedPanel);
            AM.Stop("Count");
            IsCounting = false;
            print("Exit");

        }

    }
    IEnumerator GiveCoinsToHenhouse()
    {
        AM.Play("Count");
        IsCounting = true;
        while (IsCounting && CurrencyManager.Instance.coins > 0 && henhouseLocked_CR > 0)
        {
            if (henhouseLocked_CR <= 0)
                break;
            if (AM.GetPitch("Count") <= 2.2f)
                AM.SetPitch("Count", AM.GetPitch("Count") + 0.005f);
            henhouseLocked_CR -= decrementAmount;
            henhouseLocked_CRT.text = henhouseLocked_CR.ToString();
            CurrencyManager.Instance.DeductCoins(decrementAmount);
            yield return null;
        }
        AM.SetPitch("Count", 1);
        AM.Stop("Count");
        if (henhouseLocked_CR <= 0)
        {
            if (!henhouseUnlocked)
            {
                DOTween.SmoothRewind(henhouseLockedPanel);
                yield return new WaitForSeconds(0.5f);
                GameManager.Instance.UpdateGameState(GameState.UnlockingArea);
                UnlockArea("Henhouse");
            }
        }
    }
    IEnumerator GiveCoinsToBarn()
    {
        AM.Play("Count");
        IsCounting = true;
        while (IsCounting && CurrencyManager.Instance.coins > 0 && barnLocked_CR > 0)
        {
            if (barnLocked_CR <= 0)
                break;
            if (AM.GetPitch("Count") <= 2.2f)
                AM.SetPitch("Count", AM.GetPitch("Count") + 0.005f);
            barnLocked_CR -= decrementAmount;
            barnLocked_CRT.text = barnLocked_CR.ToString();
            CurrencyManager.Instance.DeductCoins(decrementAmount);
            yield return null;
        }
        AM.SetPitch("Count", 1);
        AM.Stop("Count");
        if (barnLocked_CR <= 0)
        {
            if (!barnUnlocked)
            {
                DOTween.SmoothRewind(barnLockedPanel);
                yield return new WaitForSeconds(0.5f);
                GameManager.Instance.UpdateGameState(GameState.UnlockingArea);
                UnlockArea("Barn");
            }
        }

    }
    IEnumerator GiveCoinsToFarm()
    {
        if (PlayerPrefs.GetInt("FarmCanUnlock") == 1)
        {

            AM.Play("Count");
            IsCounting = true;
            while (IsCounting && CurrencyManager.Instance.coins > 0 && farmLocked_CR > 0)
            {
                if (farmLocked_CR <= 0)
                    break;
                if (AM.GetPitch("Count") <= 2.2f)
                    AM.SetPitch("Count", AM.GetPitch("Count") + 0.005f);
                farmLocked_CR -= decrementAmount;
                farmLocked_CRT.text = farmLocked_CR.ToString();
                CurrencyManager.Instance.DeductCoins(decrementAmount);
                yield return null;
            }
            AM.SetPitch("Count", 1);
            AM.Stop("Count");
            if (farmLocked_CR <= 0)
            {
                if (!farmUnlocked)
                {
                    DOTween.SmoothRewind(farmLockedPanel);
                    yield return new WaitForSeconds(0.5f);
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
            PlayerPrefs.SetInt("HayloftUnlocked", 1);
            PlayerPrefs.SetInt("FarmCanUnlock", 1);
            farmCannotUnlockedPanel.SetActive(false);
            farmCanUnlockPanel.gameObject.SetActive(true);


            if (!marketUnlocked)
            {
                UnlockArea("Market");
            }

            if (area == "Henhouse")
            {
                adsManager.LogEvent("henhouse_unlocked");
                AM.Play("AreaUnlock");
                henhouseInActive.SetActive(false);
                henhouse_smokeParticle.Play();
                henhouseActive.SetActive(true);
                GameManager.Instance.UpdateGameState(GameState.InGame);
                henhouseUnlocked = true;
                AreaUnlocked?.Invoke("Henhouse");
                IsCounting = false;
            }
            else if (area == "Barn")
            {
                adsManager.LogEvent("barn_unlocked");
                AM.Play("AreaUnlock");
                barnInActive.SetActive(false);
                barn_smokeParticle.Play();
                barnActive.SetActive(true);
                GameManager.Instance.UpdateGameState(GameState.InGame);
                barnUnlocked = true;
                AreaUnlocked?.Invoke("Barn");
                IsCounting = false;
            }
        }

        else if (area == "Farm")
        {
            adsManager.LogEvent("barn_unlocked");
            AM.Play("AreaUnlock");
            farmInActive.SetActive(false);
            farm_smokeParticle.Play();
            farmActive.SetActive(true);
            GameManager.Instance.UpdateGameState(GameState.InGame);
            farmUnlocked = true;
            IsCounting = false;
        }

        else if (area == "Market")
        {
            marketInActive.SetActive(false);
            marketActive.SetActive(true);
            GameManager.Instance.UpdateGameState(GameState.InGame);
            marketUnlocked = true;
            IsCounting = false;
        }


    }


}
