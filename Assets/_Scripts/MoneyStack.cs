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
        StartCoroutine(AnimateMoney());
    }
    IEnumerator AnimateMoney()
    {
        // Debug.LogError("Bought"); 

        if (!IsGenerating)
        {
            IsGenerating = true;
            for (int i = 0; i < 3; i++)
            {
                GameObject money = Instantiate(moneyPrefab, moneySpawmPoimt.position, Quaternion.identity, this.transform);
                DOTween.Complete(money);
                money.transform.DOLocalJump(cellPositions[currentR, currentC], 1, 1, 0.2f).SetDelay(delay).SetEase(Ease.Linear).OnComplete(() =>
                {
                    coinsStored++;

                });
                previousPositions.Add(cellPositions[currentR, currentC]);
                UpdateGridPositions();
                delay += 0.0001f;
                if (i == (3 - 1))
                {
                    delay = 0;
                    IsGenerating = false;
                    //RefreshGrid();
                }
                yield return null;
            }
        }


    }
    public void RefreshGrid()
    {
        currentR = 0;
        currentC = 0;
        gridOffset.y = 0f;
        CalculateCellPositions();
        for (int i = 0; i < transform.childCount; i++)
        {
            DOTween.Complete(transform.GetChild(i));
            transform.GetChild(i).localPosition = cellPositions[currentR, currentC];
            UpdateGridPositions();

        }

    }

}
