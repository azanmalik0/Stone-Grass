using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HayLoft : Stacker
{
    [Title("References")]
    [SerializeField] GameObject feedCellPrefab;
    [SerializeField] Transform feedCellStart;
    [SerializeField] Transform feedCellLast;
    //================================
    [SerializeField] Text feedGeneratedText;
    [SerializeField] Text crudeStorageCapacityText;
    //=================================
    public int requiredHay;
    public int hayStored;
    public int feedStored;
    public int feedGenerated;

    public bool FeedStorageFull;
    public bool IsGenerating;
    public bool IsFunctional;


    private void OnEnable()
    {
        HayStack.OnSellingHarvest += GetValue;
        FarmUpgradeManager.OnIncreasingStorageCapcaity += DisplayCrudeStorageCounter;
    }
    private void OnDestroy()
    {
        HayStack.OnSellingHarvest -= GetValue;
        FarmUpgradeManager.OnIncreasingStorageCapcaity -= DisplayCrudeStorageCounter;

    }

    private void Start()
    {
        SetGridYOffset(0.22f);
        LoadFeedStored();
        CalculateCellPositions();
        feedGeneratedText.text = feedGenerated.ToString();
        DisplayCrudeStorageCounter();
    }
    private void LoadFeedStored()
    {
        if (feedStored > 0 && previousPositions.Count>0)
        {
            for (int i = 0; i < feedStored; i++)
            {
                GameObject cell = Instantiate(feedCellPrefab, this.transform);
                cell.transform.localPosition = previousPositions[i];

            }
        }
    }
    public void DisplayCrudeStorageCounter()
    {
        maxHayCapacity = FarmUpgradeManager.Instance.maxStorageCapacity;
        crudeStorageCapacityText.text = $"{feedStored}/{maxHayCapacity}";
    }
    private void LateUpdate()
    {
        if (feedGenerated > 0 && feedStored < maxHayCapacity && !IsGenerating)
        {
            StartHayLoft();
        }

    }
    void GetValue(int value)
    {
        hayStored = value;
        
        CheckforHayGeneration();
    }

    void CheckforHayGeneration()
    {
        if (hayStored >= 10)
        {
            feedGenerated++;
            feedGeneratedText.text = feedGenerated.ToString();
            hayStored -= 10;
            //ES3AutoSaveMgr.Current.Save();
            CheckforHayGeneration();
        }
        else
        {

        }
    }
    public void StartHayLoft()
    {

        IsGenerating = true;
        // Debug.LogError("B");
        GameObject feedCell = Instantiate(feedCellPrefab, feedCellStart.position, Quaternion.identity);
        feedCell.transform.DOLocalMove(feedCellLast.position, 2f).SetEase(Ease.Linear).OnComplete(() =>
        {
            feedCell.transform.SetParent(this.transform);
            feedCell.transform.DOLocalJump(cellPositions[currentR, currentC], 3, 1, 1f).SetEase(Ease.OutQuint).OnComplete(() =>
            {
                feedGenerated--;
                feedGeneratedText.text = feedGenerated.ToString();
                feedStored++;
                //ES3AutoSaveMgr.Current.Save();
                //print(feedStored);
                crudeStorageCapacityText.text = $"{feedStored}/{maxHayCapacity}";
                previousPositions.Add(cellPositions[currentR, currentC]);
                UpdateGridPositions();
                IsGenerating = false;

            });

        });


    }
    private void OnApplicationPause()
    {
    }


}




