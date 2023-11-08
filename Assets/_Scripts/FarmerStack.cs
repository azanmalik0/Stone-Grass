using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FarmerStack : Stacker
{
    int feedCollected;
    bool IsLoading;
    bool IsUnloading;
    float delay;
    private void Start()
    {
        CalculateCellPositions();
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("HayLoft"))
        {
            if (!IsLoading)
            {
                IsLoading = true;
                Load(other);
                Debug.LogError("enter1");
            }
            Debug.LogError("enter");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("HayLoft"))
        {
            //IsLoading = false;
            Debug.LogError("exit");
        }
    }

    protected override void Load(Collider other)
    {
        if (IsLoading && other.transform.childCount > 0)
        {
            Debug.LogError(other.transform.childCount);
            feedCollected++;
            Transform feedCell = other.transform.GetChild(0);
            DOTween.Complete(feedCell);
            feedCell.SetParent(this.transform);
            //feedCell.localPosition = cellPositions[currentC, currentR];
            feedCell.DOLocalJump(cellPositions[currentC, currentR], 2, 1, 0.5f).SetDelay(delay).SetEase(Ease.OutSine);
            feedCell.localRotation = Quaternion.identity;
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
            other.GetComponent<HayLoft>().ResetGridPositions();
            Debug.LogError("enter2");
            IsLoading = false;
            print("OUT");
        }

    }

    public void ResetGridPositions()
    {
        currentC--;
        if (currentC < 0)
        {
            currentC = maxColumns - 1;
            currentR--;

            if (currentR < 0)
            {
                Debug.LogError("StackComplete");
                RepositionStack(true);
            }
        }
    }



}
