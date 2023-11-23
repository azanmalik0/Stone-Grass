using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using PT.Garden;
using TMPro;
using UnityEngine;

public class CoinReward : MonoBehaviour
{
    [SerializeField] private GameObject pileOfCoins;
    // [SerializeField] private TextMeshProUGUI counter;
    [SerializeField] private Vector2[] initialPos;
    [SerializeField] private Quaternion[] initialRotation;
    [SerializeField] private int coinsAmount;


    private void OnEnable()
    {
        PercentageChecker.OnSecondStarUnlock += AnimateCoinReward;
    }
    private void OnDisable()
    {
        PercentageChecker.OnSecondStarUnlock -= AnimateCoinReward;
        
    }
    void Start()
    {

        if (coinsAmount == 0)
            coinsAmount = 7; // you need to change this value based on the number of coins in the inspector

        initialPos = new Vector2[coinsAmount];
        initialRotation = new Quaternion[coinsAmount];

        for (int i = 0; i < pileOfCoins.transform.childCount; i++)
        {
            initialPos[i] = pileOfCoins.transform.GetChild(i).GetComponent<RectTransform>().anchoredPosition;
            initialRotation[i] = pileOfCoins.transform.GetChild(i).GetComponent<RectTransform>().rotation;
        }

    }




    public void AnimateCoinReward()
    {
        pileOfCoins.SetActive(true);
        var delay = 0f;

        for (int i = 0; i < pileOfCoins.transform.childCount; i++)
        {
            pileOfCoins.transform.GetChild(i).DOScale(1f, 0.3f).SetDelay(delay).SetEase(Ease.OutBack);
            pileOfCoins.transform.GetChild(i).GetComponent<RectTransform>().DOAnchorPos(new Vector2(670f, 1580f), 0.8f).SetDelay(delay + 0.5f).SetEase(Ease.InBack);
            pileOfCoins.transform.GetChild(i).DORotate(Vector3.zero, 0.5f).SetDelay(delay + 0.5f).SetEase(Ease.Flash);
            pileOfCoins.transform.GetChild(i).DOScale(0f, 0.3f).SetDelay(delay + 1.5f).SetEase(Ease.OutBack);
            if (i == pileOfCoins.transform.childCount - 1)
            {
                ResetValues();
            }
            delay += 0.1f;
            //counter.transform.parent.GetChild(0).transform.DOScale(1.1f, 0.1f).SetLoops(10, LoopType.Yoyo).SetEase(Ease.InOutSine).SetDelay(1.2f);
        }

        StartCoroutine(CountDollars());
    }

    private void ResetValues()
    {
        for (int i = 0; i < pileOfCoins.transform.childCount; i++)
        {
            pileOfCoins.transform.GetChild(i).GetComponent<RectTransform>().anchoredPosition = initialPos[i];
            pileOfCoins.transform.GetChild(i).GetComponent<RectTransform>().rotation = initialRotation[i];
        }
    }

    IEnumerator CountDollars()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        CurrencyManager.Instance.RecieveCoins(coinsAmount);
    }
}
