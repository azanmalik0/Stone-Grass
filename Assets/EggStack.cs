using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggStack : Stacker
{
    bool IsLoading;
    int feedCollected;
    float delay;


    private void Start()
    {
        CalculateCellPositions();
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Farmer_Stack"))
        {
            if (!IsLoading && other.transform.childCount > 0)
                Load(other);


        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Farmer_Stack"))
        {
            IsLoading = false;
        }
    }
    protected override void Load(Collider other)
    {

        IsLoading = true;

        if (feedCollected >= maxHayCapacity)
        {
            //Debug.LogError("MaxCapacityReached");

        }
        else
        {
            feedCollected++;
            other.transform.GetChild(0).SetParent(this.transform);
            Transform feedCell = transform.GetChild(0);
            feedCell.DOLocalJump(cellPositions[currentC, currentR], 2, 1, 0.5f).SetDelay(delay).SetEase(Ease.OutSine).OnComplete(() =>
            {
                feedCell.localRotation = Quaternion.identity;

            });

            delay += 0.0001f;

            currentC++;
            if (currentC >= maxColumns)
            {
                currentC = 0;
                currentR++;

                if (currentR >= maxRows)
                {
                    Debug.LogError("StackComplete");
                    RepositionStack(false);
                }
            }
            other.GetComponent<FarmerStack>().ResetGridPositions();




            IsLoading = false;
        }
    }



    //protected override IEnumerator LoadDelay(Collider other)
    //{

    //    IsLoading = true;
    //    while (IsLoading && other.transform.childCount > 0)
    //    {
    //        if (feedCollected >= maxHayCapacity)
    //        {
    //            //Debug.LogError("MaxCapacityReached");

    //        }
    //        else
    //        {
    //            feedCollected++;
    //            other.transform.GetChild(0).SetParent(this.transform);
    //            Transform feedCell = transform.GetChild(transform.childCount - 1);
    //            Debug.LogError(feedCell.name);
    //            feedCell.DOLocalJump(cellPositions[currentC, currentR], 2, 1, 0.5f).SetDelay(delay).SetEase(Ease.OutSine).OnComplete(() =>
    //            {
    //                feedCell.localRotation = Quaternion.identity;

    //            });

    //            delay += 0.000001f;

    //            currentC++;
    //            if (currentC >= maxColumns)
    //            {
    //                currentC = 0;
    //                currentR++;

    //                if (currentR >= maxRows)
    //                {
    //                    Debug.LogError("StackComplete");
    //                    RepositionStack(false);
    //                }
    //            }
    //            other.GetComponent<FarmerStack>().ResetGridPositions();

    //        }


    //        IsLoading = false;
    //        yield return null;
    //    }
    //}
    public void UpdateGridpositions()
    {
        currentC++;
        if (currentC >= maxColumns)
        {
            currentC = 0;
            currentR++;

            if (currentR >= maxRows)
            {
                Debug.LogError("StackComplete");
                RepositionStack(false);
            }

        }
    }
}
