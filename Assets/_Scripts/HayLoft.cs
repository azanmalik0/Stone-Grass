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
    int feedCollected = 0;
    int feedGenerated = 0;
    public int feedStored = 0;
    bool IsGenerating;
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
    private void Awake()
    {
        SetGridYOffset(gridOffset.y);

    }
    private void Start()
    {
        ES3AutoSaveMgr.Current.Load();
        LoadFeedStored();
        CalculateCellPositions();
        DisplayCrudeStorageCounter();
    }
    private void LoadFeedStored()
    {
        if (feedStored > 0)
        {
            for (int i = 0; i < feedStored; i++)
            {
                GameObject cell = Instantiate(feedCellPrefab, this.transform);
                cell.transform.localPosition = previousPositions[i];

            }
        }
    }

    private void DisplayCrudeStorageCounter()
    {
        maxHayCapacity = FarmUpgradeManager.Instance.maxStorageCapacity;
        crudeStorageCapacityText.text = $"{transform.childCount}/{maxHayCapacity}";
    }

    void GetValue(int value)
    {
        feedCollected++;

        if (feedCollected >= requiredHay && !IsGenerating)
        {
            IsGenerating = true;
            GenerateFeed();
        }
    }
    void GenerateFeed()
    {
        if (feedCollected < requiredHay)
        {
            IsGenerating = false;
        }
        else
        {

            feedCollected -= requiredHay;
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
            feedStored++;
            DisplayCrudeStorageCounter();
            hay.transform.SetParent(this.transform);
            hay.transform.DOLocalJump(cellPositions[currentR, currentC], 1, 1, 0.5f).SetEase(Ease.Linear);
            previousPositions.Add(cellPositions[currentR, currentC]);
            UpdateGridPositions();

        }


    }
    private void OnApplicationQuit()
    {
        ES3AutoSaveMgr.Current.Save();
    }


}




