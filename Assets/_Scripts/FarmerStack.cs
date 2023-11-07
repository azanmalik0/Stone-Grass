using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmerStack : Stacker
{
    private void Start()
    {
        CalculateCellPositions();
    }
    bool IsLoading;
    float delay;
    int n = 0;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HayLoft"))
        {
            IsLoading = true;
            StartCoroutine(LoadDelay(other));
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("HayLoft"))
        {
            IsLoading = false;
        }
    }

    protected override IEnumerator LoadDelay(Collider other)
    {
        while (IsLoading && other.transform.childCount > 0)
        {
            other.transform.GetChild(0).SetParent(this.transform);
            Transform feedCell = transform.GetChild(transform.childCount - 1);
            feedCell.DOLocalJump(cellPositions[currentC, currentR], 2, 1, 0.5f).SetDelay(delay).SetEase(Ease.OutSine).OnComplete(() =>
            {
                feedCell.localRotation = Quaternion.identity;
            });

            delay += 0.000001f;

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

            yield return null;
        }
    }
}
