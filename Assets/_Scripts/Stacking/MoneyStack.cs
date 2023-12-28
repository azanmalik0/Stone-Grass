using DG.Tweening;
using ES3Internal;
using EZ_Pooling;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ProductMarketStack;
using static ProductStack;

public class MoneyStack : Stacker
{
    bool IsGenerating;
    float delay;
    [SerializeField] GameObject moneyPrefab;
    [SerializeField] Transform moneySpawmPoimt;
    public int coinsStored;
    [SerializeField] GameObject coinPrefab;
    void Start()
    {
        SetGridYOffset(0);
        CalculateCellPositions();
        LoadMoneyStored();

    }
    private void OnEnable()
    {
        BuyerStack.OnProductBought += GenerateMoney;
    }
    private void OnDisable()
    {
        BuyerStack.OnProductBought -= GenerateMoney;

    }
    private void LoadMoneyStored()
    {
        if (coinsStored > 0)
        {
            for (int i = 0; i < coinsStored; i++)
            {
                Transform coins = EZ_PoolManager.Spawn(coinPrefab.transform, Vector3.zero, Quaternion.identity);
                coins.SetParent(this.transform);
                coins.localPosition = previousPositions[i];

            }
        }

    }
    void GenerateMoney()
    {
        StartCoroutine(AnimateMoney());
    }
    IEnumerator AnimateMoney()
    {

        if (!IsGenerating)
        {
            IsGenerating = true;
            for (int i = 0; i < 3; i++)
            {
                Transform money = EZ_PoolManager.Spawn(moneyPrefab.transform, moneySpawmPoimt.position, Quaternion.identity);
                money.SetParent(this.transform);
                DOTween.Complete(money);
                money.transform.DOLocalJump(cellPositions[currentR, currentC], 1, 1, 0.2f).SetDelay(delay).SetEase(Ease.Linear).OnComplete(() => coinsStored++);
                previousPositions.Add(cellPositions[currentR, currentC]);
                UpdateGridPositions();
                delay += 0.0001f;
                if (i == (3 - 1))
                {
                    delay = 0;
                    IsGenerating = false;
                }
                yield return null;
            }
        }


    }

}
