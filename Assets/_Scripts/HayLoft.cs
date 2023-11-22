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
    public bool FeedStorageFull = false;
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
        //if (!FeedStorageFull)
        //    GenerateFeed();
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
   
   int comingFeed;
 
    void GetValue(int value)
    {


        comingFeed++;

        if(comingFeed == 10)
        {
          
            comingFeed = 0;
            feedGenerated++;
          
            feedGeneratedText.text = feedGenerated.ToString();

            if (!FeedStorageFull)
            {
                GenerateFeed();
                Debug.LogError("Katoooo");
            }

             HayStack.instance.haySold = 0;





        }

       

        

    }


    private void Update()
    {
        DisplayCrudeStorageCounter();

    }
    
    void GenerateFeed()
    {
       
        
            GameObject feedCell = Instantiate(feedCellPrefab, feedCellStart.position, Quaternion.identity);
            feedCell.transform.DOLocalMove(feedCellLast.position, 2f).SetEase(Ease.Linear).SetDelay(1).OnComplete(() =>
            {
                feedGenerated--;

                 LoadOnLoftPlatform(feedCell.transform);



                if (feedGenerated <= 0)
                {
                    feedGenerated = 0;
                }

                

                feedGeneratedText.text = feedGenerated.ToString();
            });
        
      
    }
   
    void LoadOnLoftPlatform(Transform hay)
    {
        if (transform.childCount >= maxHayCapacity )
        {
           // IsGenerating = false;
           FeedStorageFull = true;
            Debug.LogError("Feed full ho gayi");
        }
        else
        {

            feedStored++;
            hay.transform.SetParent(this.transform);
            hay.transform.DOLocalJump(cellPositions[currentR, currentC], 3, 1, 0.5f).SetEase(Ease.OutQuint).OnComplete(() => hay.transform.SetParent(this.transform));
            previousPositions.Add(cellPositions[currentR, currentC]);
            UpdateGridPositions();
            FeedStorageFull = false;
        }


    }
    private void OnApplicationQuit()
    {
        ES3AutoSaveMgr.Current.Save();
    }


}




