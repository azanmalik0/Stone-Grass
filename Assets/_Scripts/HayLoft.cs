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
    [SerializeField] Text feedGeneratedText;
    // [SerializeField] HayStack hayStackScript;
    public float initialYOffset;
    public int feedCollected = 0;
    int feedGenerated = 0;
    public int feedStored = 0;
    public bool IsGenerating;
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

    }
    private void Start()
    {
        SetGridYOffset(gridOffset.y);
        ES3AutoSaveMgr.Current.Load();
        LoadFeedStored();
        CalculateCellPositions();
        feedGeneratedText.text = feedGenerated.ToString();
        GenerateFeed();
        // DisplayCrudeStorageCounter();
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
        if (value > 0 && !IsGenerating)
        {
            feedGenerated = (int)(value * 0.0333f);
            IsGenerating = true;
            feedGeneratedText.text = feedGenerated.ToString();
            GenerateFeed();
        }

    }
    float generateDelay;
    private void Update()
    {
        DisplayCrudeStorageCounter();

    }
    void GenerateFeed()
    {
        if (feedGenerated > 0)
        {
            GameObject feedCell = Instantiate(feedCellPrefab, feedCellStart.position, Quaternion.identity);
            feedCell.transform.DOLocalMove(feedCellLast.position, 2f).SetEase(Ease.Linear).SetDelay(1).OnComplete(() =>
            {
                LoadOnLoftPlatform(feedCell.transform);
                feedGenerated--;
                feedGeneratedText.text = feedGenerated.ToString();
                GenerateFeed();

            });
        }
    }
    //===================OLD=================================
    //void GenerateFeed()
    //{
    //    feedGenerated++;
    //    feedGeneratedText.text = feedGenerated.ToString();

    //    GameObject feedCell = Instantiate(feedCellPrefab, feedCellStart.position, Quaternion.identity);
    //    feedCell.transform.DOLocalMove(feedCellLast.position, 1f).SetEase(Ease.Linear).SetDelay(generateDelay).OnComplete(() =>
    //    {
    //        LoadOnLoftPlatform(feedCell.transform);
    //        GenerateFeed();
    //        feedGenerated--;
    //        feedGeneratedText.text = feedGenerated.ToString();
    //        // generateDelay += 1f;

    //    });
    //}
    //=======================================================
    void LoadOnLoftPlatform(Transform hay)
    {
        if (transform.childCount >= maxHayCapacity)
        {
            IsGenerating = false;
        }
        else
        {
            feedStored++;
            hay.transform.SetParent(this.transform);
            hay.transform.DOLocalJump(cellPositions[currentR, currentC], 3, 1, 0.5f).SetEase(Ease.OutQuint).OnComplete(() => hay.transform.SetParent(this.transform));
            previousPositions.Add(cellPositions[currentR, currentC]);
            UpdateGridPositions();

        }


    }
    private void OnApplicationQuit()
    {
        ES3AutoSaveMgr.Current.Save();
    }


}




