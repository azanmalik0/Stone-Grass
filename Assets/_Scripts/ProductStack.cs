using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProductStack : Stacker
{
    public enum ProductType { Egg, Milk }
    public ProductType type;
    public float initialYOffset;
    [SerializeField] GameObject hayParent;
    [SerializeField] GameObject productPrefab;
    [SerializeField] GameObject eggPrefab;
    [SerializeField] GameObject milkPrefab;

    float delay;
    //public int products;
    public int eggsGenerated;
    public int milkGenerated;
    bool IsLoading;
    private void Awake()
    {

    }
    private void Start()
    {
        SetGridYOffset(0);
        CalculateCellPositions();
        LoadProductStored();
    }
    private void LoadProductStored()
    {

        if (type == ProductType.Egg)
        {
            for (int i = 0; i < eggsGenerated; i++)
            {
                GameObject cell = Instantiate(eggPrefab, this.transform);
                cell.transform.localPosition = previousPositions[i];

            }
        }
        if (type == ProductType.Milk)
        {
            for (int i = 0; i < milkGenerated; i++)
            {
                GameObject cell = Instantiate(milkPrefab, this.transform);
                cell.transform.localPosition = previousPositions[i];
            }
        }


    }
    private void OnEnable()
    {
        Timer.OnTimeOut += GenerateProduct;
    }
    private void OnDisable()
    {

        Timer.OnTimeOut -= GenerateProduct;
    }

    public void GenerateProduct(AnimalType animal)
    {

        if (hayParent.transform.childCount > 0)
        {

            GameObject feedCell = hayParent.transform.GetChild(hayParent.transform.childCount - 1).gameObject;
            if (type == ProductType.Egg)
            {
                GameObject product = Instantiate(eggPrefab, feedCell.transform.position, Quaternion.identity, hayParent.transform);
                if (!IsLoading)
                {
                    IsLoading = true;
                    LoadProductOnShelf(product);
                }

            }
            if (type == ProductType.Milk)
            {
                GameObject product = Instantiate(milkPrefab, feedCell.transform.position, Quaternion.identity, hayParent.transform);
                if (!IsLoading)
                {
                    IsLoading = true;
                    LoadProductOnShelf(product);
                }

            }
            feedCell.transform.SetParent(null);
            hayParent.GetComponent<TroughStack>().feedStored--;
            hayParent.GetComponent<TroughStack>().DisplayCrudeTroughCounter();
            if ((hayParent.GetComponent<TroughStack>().previousPositions.Count - 1) > 0)
                hayParent.GetComponent<TroughStack>().previousPositions.RemoveAt(hayParent.GetComponent<TroughStack>().previousPositions.Count - 1);
            Destroy(feedCell);
            GenerateProduct(animal);

        }
    }
    void LoadProductOnShelf(GameObject product)
    {
        product.transform.SetParent(this.transform);
        product.transform.DOLocalJump(cellPositions[currentC, currentR], 1, 1, 0.2f).SetDelay(delay).SetEase(Ease.OutSine);

        if (type == ProductType.Egg)
        {
            eggsGenerated++;

        }
        if (type == ProductType.Milk)
        {

            milkGenerated++;

        }
        previousPositions.Add(cellPositions[currentC, currentR]);
        delay += 0.001f;
        UpdateGridPositions();
        hayParent.GetComponent<TroughStack>().ResetGridPositions();
        IsLoading = false;
    }
    
    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {

        }
    }


}
