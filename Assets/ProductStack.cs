using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductStack : Stacker
{
    public float initialYOffset;
    [SerializeField] GameObject hayParent;
    [SerializeField] GameObject eggPrefab;
    float delay;
    public int products;
    private void Start()
    {
        CalculateCellPositions();
        initialYOffset = gridOffset.y;
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
            GameObject egg = Instantiate(eggPrefab, feedCell.transform.position, Quaternion.identity, hayParent.transform);
            feedCell.transform.SetParent(null);
            Destroy(feedCell);
            int feedCollected = hayParent.GetComponent<TroughStack>().feedCollected;
            //hayParent.GetComponent<TroughStack>().feedCollectedText.text = feedCollected.ToString() + "/" + maxHayCapacity.ToString();

            LoadProductOnShelf(egg);
            GenerateProduct();

        }
    }
    void LoadProductOnShelf(GameObject product)
    {
        product.transform.SetParent(this.transform);
        product.transform.DOLocalJump(cellPositions[currentC, currentR], 1, 1, 0.5f).SetDelay(delay).SetEase(Ease.OutSine);
        delay += 0.001f;
        UpdateGridPositions(initialYOffset);
        hayParent.GetComponent<TroughStack>().ResetGridPositions(hayParent.GetComponent<TroughStack>().initialYOffset);
    }


}
