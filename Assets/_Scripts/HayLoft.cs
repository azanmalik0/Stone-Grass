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
    private void OnDisable()
    {
        HayStack.OnSellingHarvest -= GetValue;
        FarmUpgradeManager.OnIncreasingStorageCapcaity -= DisplayCrudeStorageCounter;

    }

    private void Start()
    {
        SetGridYOffset(gridOffset.y);
        ES3AutoSaveMgr.Current.Load();
        LoadFeedStored();
        CalculateCellPositions();
        feedGeneratedText.text = feedGenerated.ToString();
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
    public void DisplayCrudeStorageCounter()
    {
        maxHayCapacity = FarmUpgradeManager.Instance.maxStorageCapacity;
        crudeStorageCapacityText.text = $"{feedStored}/{maxHayCapacity}";
    }
    private void LateUpdate()
    {
        //DisplayCrudeStorageCounter();
        if (feedStored < maxHayCapacity)
        {
            StartHayLoft();
        }

    }
    void GetValue(int value)
    {
        hayStored++;
        if (hayStored >= 1)
        {
            feedGenerated++;
            feedGeneratedText.text = feedGenerated.ToString();
            hayStored = 0;
            if (!IsFunctional)
            {
                IsFunctional = true;
                StartHayLoft();
            }

        }
    }
    public void StartHayLoft()
    {
        if (feedGenerated >= 1 && !IsGenerating)
        {
            IsGenerating = true;
            // Debug.LogError("B");
            GameObject feedCell = Instantiate(feedCellPrefab, feedCellStart.position, Quaternion.identity);
            feedCell.transform.DOLocalMove(feedCellLast.position, 0.5f).SetEase(Ease.Linear).OnComplete(() =>
            {
                feedCell.transform.SetParent(this.transform);
                feedCell.transform.DOLocalJump(cellPositions[currentR, currentC], 3, 1, 0.5f).SetEase(Ease.OutQuint).OnComplete(() =>
                {
                    feedGenerated--;
                    if (feedGenerated < 0)
                    {
                        feedGenerated = 0;

                    }
                    feedGeneratedText.text = feedGenerated.ToString();
                    feedStored++;
                    crudeStorageCapacityText.text = $"{feedStored}/{maxHayCapacity}";
                    previousPositions.Add(cellPositions[currentR, currentC]);
                    UpdateGridPositions();
                    if (feedStored < maxHayCapacity)
                    {
                        if (feedGenerated == 0)
                        {
                            feedGenerated = 0;
                            IsFunctional = false;
                        }
                        
                    }
                    IsGenerating = false;

                });

            });
        }

    }
    private void OnApplicationQuit()
    {
        ES3AutoSaveMgr.Current.Save();
    }


}




