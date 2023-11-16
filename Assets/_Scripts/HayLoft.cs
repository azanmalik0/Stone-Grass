using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HayLoft : Stacker
{
    [Title("General")]
    public int requiredHay;
    [Title("References")]
    [SerializeField] GameObject feedCellPrefab;
    [SerializeField] Transform feedCellStart;
    [SerializeField] Transform feedCellLast;
    public float initialYOffset;
    int collected = 0;
    bool IsGenerating;
    int hayProcessed;
    [SerializeField] Text crudeStorageCapacityText;

    private void OnEnable()
    {
        HayStack.OnSellingHarvest += GetValue;
        FarmUpgradeManager.OnIncreasingStorageCapcaity += DisplayCrudeStorageCounter;
    }
    private void OnDisable()
    {
        HayStack.OnSellingHarvest -= GetValue;
        FarmUpgradeManager.OnIncreasingStorageCapcaity -= DisplayCrudeStorageCounter;

    }
    private void Start()
    {
        SetGridYOffset(gridOffset.y);
        CalculateCellPositions();
        DisplayCrudeStorageCounter();
    }

    private void DisplayCrudeStorageCounter()
    {
        maxHayCapacity = FarmUpgradeManager.Instance.maxStorageCapacity;
        crudeStorageCapacityText.text = $"{transform.childCount}/{maxHayCapacity}";
    }

    void GetValue(int value)
    {
        collected++;

        if (collected >= requiredHay && !IsGenerating)
        {
            IsGenerating = true;
            GenerateFeed();
        }
    }
    void GenerateFeed()
    {
        if (collected < requiredHay)
        {
            IsGenerating = false;
        }
        else
        {

            collected -= requiredHay;
            GameObject feedCell = Instantiate(feedCellPrefab, feedCellStart.position, Quaternion.identity);
            feedCell.transform.DOLocalMove(feedCellLast.position, 0.5f).SetEase(Ease.Linear).OnComplete(() =>
            {
                LoadOnLoftPlatform(feedCell.transform);
                GenerateFeed();
            });
        }
    }
    void LoadOnLoftPlatform(Transform hay)
    {
        if (transform.childCount > maxHayCapacity)
        {
            //Debug.LogError("MaxCapacityReached");

        }
        else
        {
            hayProcessed++;
            DisplayCrudeStorageCounter();
            hay.transform.SetParent(this.transform);
            hay.transform.DOLocalJump(cellPositions[currentR, currentC], 1, 1, 0.5f).SetEase(Ease.Linear);
            UpdateGridPositions();

        }


    }


}




