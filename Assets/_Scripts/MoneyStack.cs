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
    bool IsLoading;
    float delay;
    [SerializeField] GameObject moneyPrefab;
    [SerializeField] Transform moneySpawmPoimt;
    public int coinsStored;
    [SerializeField] GameObject coinPrefab;

    private void Awake()
    {

        SetGridYOffset(gridOffset.y);
    }
    void Start()
    {
        CalculateCellPositions();
        ES3AutoSaveMgr.Current.Load();
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
        if (coinsStored>0)
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
        for (int i = 0; i < 3; i++)
        {
            GameObject money = Instantiate(moneyPrefab, moneySpawmPoimt.position, Quaternion.identity, this.transform);
            coinsStored++;
            money.transform.DOLocalJump(cellPositions[currentC, currentR], 1, 1, 0.5f).SetDelay(delay).SetEase(Ease.OutSine);
            previousPositions.Add(cellPositions[currentC, currentR]);
            UpdateGridPositions();
            delay += 0.1f;
            if (i == (3 - 1))
            {
                delay = 0;
            }
        }

    }
    private void OnApplicationQuit()
    {
        ES3AutoSaveMgr.Current.Save();
    }
}
