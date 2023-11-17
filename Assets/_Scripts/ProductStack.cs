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
    private void Awake()
    {
        SetGridYOffset(gridOffset.y);
        
    }
    private void Start()
    {
        CalculateCellPositions();
        ES3AutoSaveMgr.Current.Load();
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

    public void GenerateProduct()
    {

        if (hayParent.transform.childCount > 0)
        {

            GameObject feedCell = hayParent.transform.GetChild(hayParent.transform.childCount - 1).gameObject;
            if (type == ProductType.Egg)
            {
                GameObject product = Instantiate(eggPrefab, feedCell.transform.position, Quaternion.identity, hayParent.transform);
                LoadProductOnShelf(product);

            }
            if (type == ProductType.Milk)
            {
                GameObject product = Instantiate(milkPrefab, feedCell.transform.position, Quaternion.identity, hayParent.transform);
                LoadProductOnShelf(product);

            }
            feedCell.transform.SetParent(null);
            hayParent.GetComponent<TroughStack>().feedCollected--;
            if ((hayParent.GetComponent<TroughStack>().previousPositions.Count - 1) > 0)
                hayParent.GetComponent<TroughStack>().previousPositions.RemoveAt(hayParent.GetComponent<TroughStack>().previousPositions.Count - 1);
            Destroy(feedCell);
            GenerateProduct();

        }
    }
    void LoadProductOnShelf(GameObject product)
    {
        if (type == ProductType.Egg)
        {
            eggsGenerated++;

        }
        if (type == ProductType.Milk)
        {

            milkGenerated++;

        }
        product.transform.SetParent(this.transform);
        product.transform.DOLocalJump(cellPositions[currentC, currentR], 1, 1, 0.5f).SetDelay(delay).SetEase(Ease.OutSine);
        previousPositions.Add(cellPositions[currentC, currentR]);
        delay += 0.001f;
        UpdateGridPositions();
        hayParent.GetComponent<TroughStack>().ResetGridPositions();
    }
    private void OnApplicationQuit()
    {
        ES3AutoSaveMgr.Current.Load();
    }


}
