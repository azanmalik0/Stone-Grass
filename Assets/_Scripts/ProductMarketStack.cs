using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductMarketStack : Stacker
{
    public enum ProductTypes { Egg, Milk }
    public ProductTypes productType;
    public string productString;
    bool IsLoading;
    float delay = 0;
    [SerializeField] int productCheckIndex;
    void Start()
    {
        productCheckIndex = 1;
        CalculateCellPositions();
        SetGridYOffset(gridOffset.y);
        SetProductType(productType);

    }

    void SetProductType(ProductTypes product)
    {
        if (product == ProductTypes.Egg)
            productString = "Egg";
        if (product == ProductTypes.Milk)
            productString = "Milk";

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Farmer_Stack"))
        {
            if (!IsLoading)
            {
                LoadProductOnMarketShelf(other);
            }
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Farmer_Stack"))
        {
            IsLoading = false;
            delay = 0;
        }
    }

    void LoadProductOnMarketShelf(Collider other)
    {
        if (transform.childCount >= maxHayCapacity)
        {


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
                    IsLoading = true;
                    Transform product = other.transform.GetChild(other.transform.childCount - productCheckIndex);
                    DOTween.Complete(product);
                    product.SetParent(this.transform);
                    product.DOLocalJump(cellPositions[currentC, currentR], 2, 1, 0.5f).SetDelay(delay).SetEase(Ease.OutSine).OnComplete(() => product.localRotation = Quaternion.identity);
                    delay += 0.0001f;
                    UpdateGridPositions();
                    other.GetComponent<FarmerStack>().ResetGridPositions();
                    other.GetComponent<FarmerStack>().RefreshGrid();
                    IsLoading = false;

                }
            }

        }


    }
}
