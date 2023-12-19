using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SearchService;
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

    void Start()
    {
        if (productType == ProductTypes.Egg)
            SetGridYOffset(0f);
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
                    GameObject milk = Instantiate(milkPrefab, this.transform);
                    milk.transform.localEulerAngles = new(0, 90, 0);
                    milk.transform.localPosition = previousPositions[i];
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
            //other.GetComponent<FarmerStack>().RefreshGrid();
            IsLoading = false;
            delay = 0;
        }
    }
    private void Update()
    {
        if (this.transform.childCount <= 0)
        {
            currentC = 0;
            currentR = 0;
        }
    }
    void LoadProductOnMarketShelf(Collider other)
    {
        if (other.transform.childCount <= 0)
        {
            other.GetComponent<FarmerStack>().RefreshGrid();
            IsLoading = false;
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

                    other.GetComponent<FarmerStack>().totalItems--;
                    Transform product = other.transform.GetChild(other.transform.childCount - productCheckIndex);
                    if (productType == ProductTypes.Egg)
                    {
                        eggsStored++;
                        other.GetComponent<FarmerStack>().eggCollected--;

                    }
                    if (productType == ProductTypes.Milk)
                    {
                        milkStored++;
                        product.transform.GetChild(0).localScale = new(1.1f, 1.1f, 1.1f);
                        other.GetComponent<FarmerStack>().milkCollected--;

                    }
                    DOTween.Complete(product);
                    product.SetParent(this.transform);
                    product.DOLocalJump(cellPositions[currentR, currentC], 2, 1, 0.2f).SetDelay(delay).SetEase(Ease.Linear);
                    product.localEulerAngles = new(0, 90, 0);
                    if ((other.GetComponent<FarmerStack>().previousPositions.Count - 1) >= 0)
                        other.GetComponent<FarmerStack>().previousPositions.RemoveAt(other.GetComponent<FarmerStack>().previousPositions.Count - 1);
                    previousPositions.Add(cellPositions[currentR, currentC]);
                    delay += 0.0001f;
                    UpdateGridPositions();
                    other.GetComponent<FarmerStack>().ResetGridPositions();
                    other.GetComponent<FarmerStack>().farmerCapacityFullText.gameObject.SetActive(false);
                    IsLoading = false;

                }
            }

        }


    }

}
