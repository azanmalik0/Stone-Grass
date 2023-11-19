using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LockedAreasManager : MonoBehaviour
{
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
    [Title("Preferences")]
    [TabGroup("Market")][SerializeField] int marketLocked_CR;
    [TabGroup("Market")][ReadOnly][SerializeField] int marketLocked_RM;
    //==============================================================
    float delay;
    bool Loading;
    private void OnEnable()
    {
        MenuTrigger.OnEnteringLockedZone += UpdateUnlockProgress;
    }
    private void OnDisable()
    {
        MenuTrigger.OnEnteringLockedZone -= UpdateUnlockProgress;

    }
    private void Start()
    {
        henhouseLocked_CT.text = henhouseLocked_RM + "/" + henhouseLocked_CR;
        farmLocked_CT.text = farmLocked_RM + "/" + farmLocked_CR;
        barnLocked_CT.text = barnLocked_RM + "/" + barnLocked_CR;
        marketLocked_CT.text = marketLocked_RM + "/" + marketLocked_CR;
    }

    void UpdateUnlockProgress(LockedAreas area)
    {
        switch (area)
        {
            case LockedAreas.Farm:
                if (!Loading)
                {
                    Loading = true;
                    StartCoroutine(GiveCoinsToFarm());
                }
                break;
            case LockedAreas.Henhouse:
                if (!Loading)
                {
                    Loading = true;
                    StartCoroutine(GiveCoinsToHenhouse());
                }
                break;
            case LockedAreas.Barn:
                if (!Loading)
                {
                    Loading = true;
                    StartCoroutine(GiveCoinsToBarn());
                }
                break;
            case LockedAreas.Market:
                if (!Loading)
                {
                    Loading = true;
                    StartCoroutine(GiveCoinsToMarket());
                }
                break;
            default:
                break;
        }

    }
    IEnumerator GiveCoinsToMarket()
    {
        Debug.LogError("here");
        while (Loading && CurrencyManager.coins > 0)
        {
            if (marketLocked_RM < marketLocked_CR)
            {
                marketLocked_RM++;
                marketLocked_CT.text = marketLocked_RM + "/" + marketLocked_CR;

                print(CurrencyManager.coins);
                Debug.LogError("here");
                GameObject coin = Instantiate(coinPrefab, farmerCoinPos.position, Quaternion.identity);
                float randomAngle = UnityEngine.Random.Range(0, 360);
                coin.transform.DOLocalRotate(new Vector3(randomAngle, randomAngle, randomAngle), 0.5f).SetEase(Ease.OutQuad);
                coin.transform.DOJump(marketCoinPos.position, 3, 1, 0.5f).SetDelay(delay).OnComplete(() => Destroy(coin));
                delay += 0.01f;
                CurrencyManager.Instance.DeductCoins(1);
                yield return null;
                Loading = false;
            }
            else
            {
                StartCoroutine(UnlockArea("Market"));
                break;

            }
        }
        Loading = false;
        if (!DOTween.IsTweening(marketLockedPanel) && CurrencyManager.coins > 0)
            marketLockedPanel.DOScale(new Vector3(0.00582442f, 0.00582442f, 0.00582442f), 0.1f).SetDelay(0.5f).SetLoops(CurrencyManager.coins, LoopType.Yoyo);
        delay = 0;
    }

    IEnumerator GiveCoinsToHenhouse()
    {
        Debug.LogError("here");
        while (Loading && CurrencyManager.coins > 0)
        {
            if (henhouseLocked_RM < henhouseLocked_CR)
            {
                henhouseLocked_RM++;
                henhouseLocked_CT.text = henhouseLocked_RM + "/" + henhouseLocked_CR;

                print(CurrencyManager.coins);
                Debug.LogError("here");
                GameObject coin = Instantiate(coinPrefab, farmerCoinPos.position, Quaternion.identity);
                float randomAngle = UnityEngine.Random.Range(0, 360);
                coin.transform.DOLocalRotate(new Vector3(randomAngle, randomAngle, randomAngle), 0.5f).SetEase(Ease.OutQuad);
                coin.transform.DOJump(henhouseCoinPos.position, 3, 1, 0.5f).SetDelay(delay).OnComplete(() => Destroy(coin));
                delay += 0.01f;
                CurrencyManager.Instance.DeductCoins(1);
                yield return null;
                Loading = false;
            }
            else
            {
                StartCoroutine(UnlockArea("Henhouse"));
                break;

            }
        }
        Loading = false;
        if (!DOTween.IsTweening(henhouseLockedPanel) && CurrencyManager.coins > 0)
            henhouseLockedPanel.DOScale(new Vector3(0.00582442f, 0.00582442f, 0.00582442f), 0.1f).SetDelay(0.5f).SetLoops(CurrencyManager.coins, LoopType.Yoyo);
        delay = 0;
    }
    IEnumerator GiveCoinsToFarm()
    {
        Debug.LogError("here");
        while (Loading && CurrencyManager.coins > 0)
        {
            if (farmLocked_RM < farmLocked_CR)
            {
                farmLocked_RM++;
                farmLocked_CT.text = farmLocked_RM + "/" + farmLocked_CR;

                print(CurrencyManager.coins);
                Debug.LogError("here");
                GameObject coin = Instantiate(coinPrefab, farmerCoinPos.position, Quaternion.identity);
                float randomAngle = UnityEngine.Random.Range(0, 360);
                coin.transform.DOLocalRotate(new Vector3(randomAngle, randomAngle, randomAngle), 0.5f).SetEase(Ease.OutQuad);
                coin.transform.DOJump(farmCoinPos.position, 3, 1, 0.5f).SetDelay(delay).OnComplete(() => Destroy(coin));
                delay += 0.01f;
                CurrencyManager.Instance.DeductCoins(1);
                yield return null;
                Loading = false;
            }
            else
            {
                StartCoroutine(UnlockArea("Farm"));
                break;

            }
        }
        Loading = false;
        if (!DOTween.IsTweening(farmLockedPanel) && CurrencyManager.coins > 0)
            farmLockedPanel.DOScale(new Vector3(0.00582442f, 0.00582442f, 0.00582442f), 0.1f).SetDelay(0.5f).SetLoops(CurrencyManager.coins, LoopType.Yoyo);
        delay = 0;
    }
    IEnumerator GiveCoinsToBarn()
    {
        Debug.LogError("here");
        while (Loading && CurrencyManager.coins > 0)
        {
            if (barnLocked_RM < barnLocked_CR)
            {
                barnLocked_RM++;
                barnLocked_CT.text = barnLocked_RM + "/" + barnLocked_CR;

                print(CurrencyManager.coins);
                Debug.LogError("here");
                GameObject coin = Instantiate(coinPrefab, farmerCoinPos.position, Quaternion.identity);
                float randomAngle = UnityEngine.Random.Range(0, 360);
                coin.transform.DOLocalRotate(new Vector3(randomAngle, randomAngle, randomAngle), 0.5f).SetEase(Ease.OutQuad);
                coin.transform.DOJump(barnCoinPos.position, 3, 1, 0.5f).SetDelay(delay).OnComplete(() => Destroy(coin));
                delay += 0.01f;
                CurrencyManager.Instance.DeductCoins(1);
                yield return null;
                Loading = false;
            }
            else
            {
                StartCoroutine(UnlockArea("Barn"));
                break;

            }
        }
        Loading = false;
        if (!DOTween.IsTweening(barnLockedPanel) && CurrencyManager.coins > 0)
            barnLockedPanel.DOScale(new Vector3(0.00582442f, 0.00582442f, 0.00582442f), 0.1f).SetDelay(0.5f).SetLoops(CurrencyManager.coins, LoopType.Yoyo);
        delay = 0;
    }

    private IEnumerator UnlockArea(string area)
    {
        if (area == "Henhouse")
        {

            henhouse_smokeParticle.Play();
            henhouseInActive.SetActive(false);
            yield return new WaitForSeconds(1);
            henhouseActive.SetActive(true);
            henhouseActive.transform.DOPunchScale(new Vector3(0.5f, 0.5f, 0f), 2f, vibrato: 2).SetEase(Ease.Linear);
        }
        if (area == "Farm")
        {

            farm_smokeParticle.Play();
            farmInActive.SetActive(false);
            yield return new WaitForSeconds(1);
            farmActive.SetActive(true);
            farmActive.transform.DOPunchScale(new Vector3(0.5f, 0.5f, 0f), 2f, vibrato: 2).SetEase(Ease.Linear);
        }
        if (area == "Barn")
        {

            barn_smokeParticle.Play();
            barnInActive.SetActive(false);
            yield return new WaitForSeconds(1);
            barnActive.SetActive(true);
            barnActive.transform.DOPunchScale(new Vector3(0.5f, 0.5f, 0f), 2f, vibrato: 2).SetEase(Ease.Linear);
        }
        if (area == "Market")
        {

            market_smokeParticle.Play();
            marketInActive.SetActive(false);
            yield return new WaitForSeconds(1);
            marketActive.SetActive(true);
            marketActive.transform.DOPunchScale(new Vector3(0.5f, 0.5f, 0f), 2f, vibrato: 2).SetEase(Ease.Linear);
        }
    }
}
