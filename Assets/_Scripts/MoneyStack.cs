using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyStack : Stacker
{
    bool IsLoading;
    float delay;
    [SerializeField] GameObject moneyPrefab;
    [SerializeField] Transform moneySpawmPoimt;
    void Start()
    {
        CalculateCellPositions();
        SetGridYOffset(gridOffset.y);

    }
    private void OnEnable()
    {
        BuyerStack.OnProductBought += GenerateMoney;
    }
    private void OnDisable()
    {
        BuyerStack.OnProductBought -= GenerateMoney;

    }

    void GenerateMoney()
    {
        Debug.LogError("Generate");
        for (int i = 0; i < 3; i++)
        {
            GameObject money = Instantiate(moneyPrefab, moneySpawmPoimt.position, Quaternion.identity, this.transform);
            money.transform.DOLocalJump(cellPositions[currentC, currentR], 1, 1, 0.5f).SetDelay(delay).SetEase(Ease.OutSine);
            UpdateGridPositions();
            delay += 0.1f;
            if (i == (3 - 1))
            {
                delay = 0;
            }
        }

    }
}
