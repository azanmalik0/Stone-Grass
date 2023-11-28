using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static ProductStack;

public class ProductMarketStack : Stacker
{
    public enum ProductTypes { Egg, Milk }
    public ProductTypes productType;
    public string productString;
    [SerializeField] GameObject eggPrefab;
    [SerializeField] GameObject milkPrefab;
    public int eggsStored;
    public int milkStored;
    bool IsLoading;
    float delay = 0;
    [SerializeField] int productCheckIndex;
    private void Awake()
    {

    }
    void Start()
    {
        if (productType == ProductTypes.Egg)
            SetGridYOffset(0.34f);
        if (productType == ProductTypes.Milk)
            SetGridYOffset(0);
        productCheckIndex = 1;
        CalculateCellPositions();
        LoadProductStored();
        SetProductType(productType);

    }

    void SetProductType(ProductTypes product)
    {
        if (product == ProductTypes.Egg)
            productString = "Egg";
        if (product == ProductTypes.Milk)
            productString = "Milk";

    }
    private void LoadProductStored()
    {

        if (productType == ProductTypes.Egg)
        {
            if (eggsStored > 0)
            {
                for (int i = 0; i < eggsStored; i++)
                {
                    GameObject cell = Instantiate(eggPrefab, this.transform);
                    cell.transform.localPosition = previousPositions[i];

                }
            }
        }
        if (productType == ProductTypes.Milk)
        {
            if (milkStored > 0)
            {
                for (int i = 0; i < milkStored; i++)
                {
                    GameObject cell = Instantiate(milkPrefab, this.transform);
                    cell.transform.localPosition = previousPositions[i];
                }
            }
        }


    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Farmer_Stack"))
        {
            if (!IsLoading)
            {
                IsLoading = true;
                LoadProductOnMarketShelf(other);
            }
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Farmer_Stack"))
        {
            other.GetComponent<FarmerStack>().RefreshGrid();
            IsLoading = false;
            delay = 0;
        }
    }

    void LoadProductOnMarketShelf(Collider other)
    {

        if (other.transform.childCount <= 0)
        {
            other.GetComponent<FarmerStack>().RefreshGrid();
        }
        else if (other.transform.childCount > 0)
        {
            if (productCheckIndex > other.transform.childCount)
            {
                productCheckIndex = 1;
                IsLoading = false;
            }
            else
            {
                if (!other.transform.GetChild(other.transform.childCount - productCheckIndex).CompareTag(productString))
                {
                    productCheckIndex++;
                    IsLoading = false;
                }
                else
                {

                    Transform product = other.transform.GetChild(other.transform.childCount - productCheckIndex);
                    //DOTween.Complete(product);
                    product.SetParent(this.transform);
                    if (productType == ProductTypes.Egg)
                    {
                        eggsStored++;
                        other.GetComponent<FarmerStack>().eggCollected--;

                    }
                    if (productType == ProductTypes.Milk)
                    {
                        milkStored++;
                        other.GetComponent<FarmerStack>().milkCollected--;

                    }
                    product.DOLocalJump(cellPositions[currentC, currentR], 2, 1, 0.2f).SetDelay(delay).SetEase(Ease.OutSine).OnComplete(() => product.localRotation = Quaternion.identity);
                    if ((other.GetComponent<FarmerStack>().previousPositions.Count - 1) > 0)
                        other.GetComponent<FarmerStack>().previousPositions.RemoveAt(other.GetComponent<FarmerStack>().previousPositions.Count - 1);
                    previousPositions.Add(cellPositions[currentC, currentR]);
                    delay += 0.0001f;
                    UpdateGridPositions();
                    other.GetComponent<FarmerStack>().ResetGridPositions();
                    //other.GetComponent<FarmerStack>().RefreshGrid();
                    IsLoading = false;

                }
            }

        }


    }
    //private void OnApplicationQuit()
    //{
    //    ES3AutoSaveMgr.Current.Save();
    //}
    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {

        }
    }
}
