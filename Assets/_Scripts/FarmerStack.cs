using DG.Tweening;
using Sirenix.OdinInspector;
using System;
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
        SetGridYOffset(gridOffset.y);
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
                LoadProductOnFarmer(other);
            }
        }

        if (other.CompareTag("Market"))
        {
            if (!IsLoading)
            {
                GetMoneyFromCounter(other);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("HayLoft"))
        {
            IsLoading = false;
            RefreshGrid();
        }
        if (other.CompareTag("LS_ProductShelf"))
        {
            IsLoading = false;
            RefreshGrid();
        }
        if (other.CompareTag("Market"))
        {
            IsLoading = false;
        }
    }

    void LoadFeedOnFarmer(Collider other)
    {
        if (other.transform.childCount == 0)
        {
            IsLoading = false;
            RefreshGrid();
        }
        else if (other.transform.childCount > 0)
        {
            IsLoading = true;
            Transform feedCell = other.transform.GetChild(other.transform.childCount - 1);
            DOTween.Complete(feedCell);
            feedCell.SetParent(this.transform);
            feedCell.DOLocalJump(cellPositions[currentC, currentR], 2, 1, 0.5f).SetDelay(delay).SetEase(Ease.OutSine).OnComplete(() => feedCell.localRotation = Quaternion.identity);
            delay += 0.0001f;
            UpdateGridPositions();
            other.GetComponent<HayLoft>().ResetGridPositions();
            IsLoading = false;
        }
    }
    private void GetMoneyFromCounter(Collider other)
    {

        if (other.transform.childCount == 0)
        {
            IsLoading = false;
            delay = 0;
            RefreshGrid();
        }
        else if (other.transform.childCount > 0)
        {
            IsLoading = true;
            Transform money = other.transform.GetChild(other.transform.childCount - 1);
            DOTween.Complete(money);
            money.SetParent(this.transform);
            money.DOLocalMove(cellPositions[currentC, currentR], 0.5f).SetDelay(delay).SetEase(Ease.OutSine).OnComplete(() => Destroy(money.gameObject));
            delay += 0.01f;
            other.GetComponent<MoneyStack>().ResetGridPositions();
            IsLoading = false;
        }
    }

    public void LoadProductOnFarmer(Collider other)
    {
        if (other.transform.childCount == 0)
        {
            IsLoading = false;
            RefreshGrid();
        }
        else if (other.transform.childCount > 0)
        {
            IsLoading = true;
            Transform product = other.transform.GetChild(other.transform.childCount - 1);
            DOTween.Complete(product);
            product.SetParent(this.transform);
            product.DOLocalJump(cellPositions[currentC, currentR], 2, 1, 0.5f).SetDelay(delay).SetEase(Ease.OutSine).OnComplete(() => product.localRotation = Quaternion.identity);
            delay += 0.0001f;
            UpdateGridPositions();
            other.GetComponent<ProductStack>().ResetGridPositions();
            IsLoading = false;
        }
    }


}
