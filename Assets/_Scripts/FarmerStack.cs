using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FarmerStack : Stacker
{
    public static FarmerStack Instance;
    public float initialYOffset;
    bool IsLoading;
    float delay;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        CalculateCellPositions();
        initialYOffset = gridOffset.y;
        print(initialYOffset);
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("HayLoft"))
        {
            if (!IsLoading)
            {
                LoadFeedOnFarmer(other);
            }
        }
        if (other.CompareTag("LS_ProductShelf"))
        {
            if (!IsLoading)
            {
                Debug.LogError("LS_ProductShelf");
                LoadProductOnFarmer(other);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("HayLoft"))
        {
            IsLoading = false;
        }
        if (other.CompareTag("LS_ProductShelf"))
        {
            IsLoading = false;
        }
    }

    void LoadFeedOnFarmer(Collider other)
    {
        if (other.transform.childCount == 0)
        {
            IsLoading = false;
            RefreshGrid(initialYOffset);
        }
        else if (other.transform.childCount > 0)
        {
            IsLoading = true;
            Transform feedCell = other.transform.GetChild(other.transform.childCount - 1);
            DOTween.Complete(feedCell);
            feedCell.SetParent(this.transform);
            feedCell.DOLocalJump(cellPositions[currentC, currentR], 2, 1, 0.5f).SetDelay(delay).SetEase(Ease.OutSine).OnComplete(() => feedCell.localRotation = Quaternion.identity);
            delay += 0.0001f;
            UpdateGridPositions(initialYOffset);
            other.GetComponent<HayLoft>().ResetGridPositions(other.GetComponent<HayLoft>().initialYOffset);
            IsLoading = false;
        }
    }

    public void LoadProductOnFarmer(Collider other)
    {
        if (other.transform.childCount == 0)
        {
            IsLoading = false;
            RefreshGrid(initialYOffset);
        }
        else if (other.transform.childCount > 0)
        {
            IsLoading = true;
            Transform product = other.transform.GetChild(other.transform.childCount - 1);
            DOTween.Complete(product);
            product.SetParent(this.transform);
            product.DOLocalJump(cellPositions[currentC, currentR], 2, 1, 0.5f).SetDelay(delay).SetEase(Ease.OutSine).OnComplete(() => product.localRotation = Quaternion.identity);
            delay += 0.0001f;
            UpdateGridPositions(initialYOffset);
            other.GetComponent<ProductStack>().ResetGridPositions(other.GetComponent<ProductStack>().initialYOffset);
            IsLoading = false;
        }
    }


}
