using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HayLoft : Stacker
{
    [Title("General")]
    public int requiredHay;
    [Title("References")]
    [SerializeField] GameObject feedCellPrefab;
    [SerializeField] Transform feedCellStart;
    [SerializeField] Transform feedCellLast;

    int collected = 0;
    bool IsGenerating;
    int hayProcessed;

    private void OnEnable()
    {
        HayStack.OnSellingHarvest += GetValue;
    }
    private void OnDisable()
    {
        HayStack.OnSellingHarvest -= GetValue;

    }
    private void Start()
    {
        CalculateCellPositions();
    }
    protected override void Load(Transform hay)
    {
        if (hayProcessed >= maxHayCapacity)
        {
            //Debug.LogError("MaxCapacityReached");

        }
        else
        {
            hayProcessed++;
            hay.transform.SetParent(this.transform);
            hay.transform.DOLocalJump(cellPositions[currentR, currentC], 1, 1, 0.5f).SetEase(Ease.Linear);

            currentC++;
            if (currentC >= maxColumns)
            {
                currentC = 0;
                currentR++;

                if (currentR >= maxRows)
                {
                    //Debug.LogError("StackComplete");
                    RepositionStack(false);
                }
            }
        }


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
            feedCell.transform.DOLocalMove(feedCellLast.position, 3).SetEase(Ease.Linear).OnComplete(() =>
            {
                Load(feedCell.transform);
                GenerateFeed();
            });
        }
    }

}
