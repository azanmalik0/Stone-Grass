using DG.Tweening;
using ES3Internal;
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
                GameObject cell = Instantiate(coinPrefab, this.transform);
                cell.transform.localPosition = previousPositions[i];

            }
        }

    }
    void GenerateMoney()
    {
       // Debug.LogError("Bought"); 


        for (int i = 0; i < 3; i++)
        {
            GameObject money = Instantiate(moneyPrefab, moneySpawmPoimt.position, Quaternion.identity, this.transform);
            money.transform.DOLocalJump(cellPositions[currentR, currentC], 1, 1, 0.2f).SetDelay(delay).SetEase(Ease.OutSine).OnComplete(() =>
            {
                coinsStored++;

            });
            previousPositions.Add(cellPositions[currentR, currentC]);
            UpdateGridPositions();
            delay += 0.0001f;
            if (i == (3 - 1))
            {
                delay = 0;

            }
        }


    }


}
