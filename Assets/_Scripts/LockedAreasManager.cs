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
    [TabGroup("Henhouse")][SerializeField] BoxCollider henhouse_collider;
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
        while (Loading && CurrencyManager.coins > 0)
        {
            if (marketLocked_RM < marketLocked_CR)
            {
                marketLocked_RM++;
                marketLocked_CT.text = marketLocked_RM + "/" + marketLocked_CR;

                print(CurrencyManager.coins);
                GameObject coin = Instantiate(coinPrefab, farmerCoinPos.position, Quaternion.identity);
                float randomAngle = UnityEngine.Random.Range(0, 360);
                coin.transform.DOLocalRotate(new Vector3(randomAngle, randomAngle, randomAngle), 0.5f).SetEase(Ease.OutQuad);
                coin.transform.DOJump(marketCoinPos.position, 3, 1, 0.5f).SetDelay(delay).OnComplete(() => Destroy(coin));
                delay += 0.01f;
                CurrencyManager.Instance.DeductCoins(1);
                yield return null;
            }
            else
            {
                GameManager.Instance.UpdateGameState(GameState.UnlockingArea);
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
        while (Loading && CurrencyManager.coins > 0)
        {
            if (henhouseLocked_RM < henhouseLocked_CR)
            {
                henhouseLocked_RM++;
                henhouseLocked_CT.text = henhouseLocked_RM + "/" + henhouseLocked_CR;

                print(CurrencyManager.coins);
                GameObject coin = Instantiate(coinPrefab, farmerCoinPos.position, Quaternion.identity);
                float randomAngle = UnityEngine.Random.Range(0, 360);
                coin.transform.DOLocalRotate(new Vector3(randomAngle, randomAngle, randomAngle), 0.5f).SetEase(Ease.OutQuad);
                coin.transform.DOJump(henhouseCoinPos.position, 3, 1, 0.5f).SetDelay(delay).OnComplete(() => Destroy(coin));
                delay += 0.01f;
                CurrencyManager.Instance.DeductCoins(1);
                yield return null;
            }
            else
            {
                GameManager.Instance.UpdateGameState(GameState.UnlockingArea);
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
        while (Loading && CurrencyManager.coins > 0)
        {
            if (farmLocked_RM < farmLocked_CR)
            {
                farmLocked_RM++;
                farmLocked_CT.text = farmLocked_RM + "/" + farmLocked_CR;

                GameObject coin = Instantiate(coinPrefab, farmerCoinPos.position, Quaternion.identity);
                float randomAngle = UnityEngine.Random.Range(0, 360);
                coin.transform.DOLocalRotate(new Vector3(randomAngle, randomAngle, randomAngle), 0.5f).SetEase(Ease.OutQuad);
                coin.transform.DOJump(farmCoinPos.position, 3, 1, 0.5f).SetDelay(delay).OnComplete(() => Destroy(coin));
                delay += 0.01f;
                CurrencyManager.Instance.DeductCoins(1);
                yield return null;
            }
            else
            {
                GameManager.Instance.UpdateGameState(GameState.UnlockingArea);
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
        while (Loading && CurrencyManager.coins > 0)
        {
            if (barnLocked_RM < barnLocked_CR)
            {
                barnLocked_RM++;
                barnLocked_CT.text = barnLocked_RM + "/" + barnLocked_CR;

                print(CurrencyManager.coins);
                GameObject coin = Instantiate(coinPrefab, farmerCoinPos.position, Quaternion.identity);
                float randomAngle = UnityEngine.Random.Range(0, 360);
                coin.transform.DOLocalRotate(new Vector3(randomAngle, randomAngle, randomAngle), 0.5f).SetEase(Ease.OutQuad);
                coin.transform.DOJump(barnCoinPos.position, 3, 1, 0.5f).SetDelay(delay).OnComplete(() => Destroy(coin));
                delay += 0.01f;
                CurrencyManager.Instance.DeductCoins(1);
                yield return null;
            }
            else
            {
                GameManager.Instance.UpdateGameState(GameState.UnlockingArea);
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
            henhouseInActive.SetActive(false);
            //Camera.main.transform.DOMove(new Vector3(2.6f, 4.973701f, -8.43f), 1f).SetEase(Ease.Linear);
            //Camera.main.transform.DORotate(new Vector3(16.2f, -77.05f, 0.547f), 1f).SetEase(Ease.Linear);
          //  yield return new WaitForSeconds(1);
            henhouse_smokeParticle.Play();
           // yield return new WaitForSeconds(1);
            henhouseActive.SetActive(true);
            //henhouse_collider.enabled = false;
          //  henhouseActive.transform.DOPunchScale(new Vector3(0.5f, 0.5f, 0f), 2f, vibrato: 2).SetEase(Ease.Linear).OnComplete(()=> henhouse_collider.enabled = true);
          //  yield return new WaitForSeconds(1);
            GameManager.Instance.UpdateGameState(GameState.InGame);
        }
        if (area == "Farm")
        {

            farmInActive.SetActive(false);
            //Camera.main.transform.DOMove(new Vector3(-3.07585239f, 6.00184631f, -10.0817223f), 1f).SetEase(Ease.Linear);
            //Camera.main.transform.DORotate(new Vector3(12.1820517f, 12.1642532f, 0.529350638f), 1f).SetEase(Ease.Linear);
           // yield return new WaitForSeconds(1);
            farm_smokeParticle.Play();
           // yield return new WaitForSeconds(1);
            farmActive.SetActive(true);
            //farm_collider.enabled = false;
          //  farmActive.transform.DOPunchScale(new Vector3(0.5f, 0.5f, 0f), 2f, vibrato: 2).SetEase(Ease.Linear).OnComplete(() => farm_collider.enabled = true);
          //  yield return new WaitForSeconds(1);
            GameManager.Instance.UpdateGameState(GameState.InGame);
        }
        if (area == "Barn")
        {

            barnInActive.SetActive(false);
            //Camera.main.transform.DOMove(new Vector3(-4.11999989f, 6.71000004f, -0.150000006f), 1f).SetEase(Ease.Linear);
            //Camera.main.transform.DORotate(new Vector3(17.2778378f, 104.517273f, 0.542637885f), 1f).SetEase(Ease.Linear);
           // yield return new WaitForSeconds(1);
            barn_smokeParticle.Play();
           // yield return new WaitForSeconds(1);
            barnActive.SetActive(true);
           // barn_collider.enabled = false;
           // barnActive.transform.DOPunchScale(new Vector3(0.5f, 0.5f, 0f), 2f, vibrato: 2).SetEase(Ease.Linear).OnComplete(() => barn_collider.enabled = true);
           // yield return new WaitForSeconds(1);
            GameManager.Instance.UpdateGameState(GameState.InGame);
        }
        if (area == "Market")
        {

            marketInActive.SetActive(false);
            //Camera.main.transform.DOMove(new Vector3(-2.88000011f, 5.21999979f, 1.11000001f), 1f).SetEase(Ease.Linear);
            //Camera.main.transform.DORotate(new Vector3(17.3302288f, 161.742752f, 359.360199f), 1f).SetEase(Ease.Linear);
          //  yield return new WaitForSeconds(1);
            market_smokeParticle.Play();
           // yield return new WaitForSeconds(1);
            marketActive.SetActive(true);
            //market_collider.enabled = false;
          //  marketActive.transform.DOPunchScale(new Vector3(0.5f, 0.5f, 0f), 2f, vibrato: 2).SetEase(Ease.Linear).OnComplete(() => market_collider.enabled = true);
          //  yield return new WaitForSeconds(1);
            GameManager.Instance.UpdateGameState(GameState.InGame);
        }

        yield return null;
    }
  
}
