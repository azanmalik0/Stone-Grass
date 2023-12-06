using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static ProductStack;

public class FarmerStack : Stacker
{
    // public static event Action OnMoneyCollect;
    public static FarmerStack Instance;
    public int feedCollected;
    public int milkCollected;
    public int eggCollected;
    public int totalItems;
    [Title("References")]
    [SerializeField] Transform coinPos;
    [SerializeField] public Text farmerCapacityFullText;
    [SerializeField] GameObject feedPrefab;
    [SerializeField] GameObject eggPrefab;
    [SerializeField] GameObject milkPrefab;
    //====================================
    float delay;
    bool IsLoading;
    private void Awake()
    {
        Instance = this;
    }
    private void OnEnable()
    {
        FarmUpgradeManager.OnIncreasingFarmerCapcaity += UpdateMaxFarmerCapacity;
    }
    private void OnDisable()
    {
        FarmUpgradeManager.OnIncreasingFarmerCapcaity -= UpdateMaxFarmerCapacity;

    }
    private void Start()
    {
        SetGridYOffset(0.92f);
        CalculateCellPositions();
        LoadStack();
        UpdateMaxFarmerCapacity();
    }
  
   
    int everything;
    public void CheckMax()
    {
        everything = feedCollected + eggCollected + milkCollected;
        if (everything >= maxHayCapacity)
        {
            farmerCapacityFullText.gameObject.SetActive(true);
        }
        else
        {
            farmerCapacityFullText.gameObject.SetActive(false);

        }
    }

    private void LoadStack()
    {

        for (int i = 0; i < feedCollected; i++)
        {
            GameObject cell = Instantiate(feedPrefab, this.transform);
            cell.transform.localPosition = previousPositions[i];
            cell.transform.GetChild(0).localScale = new(0.38f, 1.1f, 1.1f);

        }
        for (int i = 0; i < eggCollected; i++)
        {
            GameObject cell = Instantiate(eggPrefab, this.transform);
            cell.transform.localPosition = previousPositions[i];

        }
        for (int i = 0; i < milkCollected; i++)
        {
            GameObject cell = Instantiate(milkPrefab, this.transform);
            cell.transform.localPosition = previousPositions[i];
            cell.transform.GetChild(0).localScale = new(0.38f, 1.1f, 0.791418f);

        }
        RefreshGrid();

    }
    private void UpdateMaxFarmerCapacity()
    {
        maxHayCapacity = FarmUpgradeManager.Instance.maxFarmerCapacity;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("HayLoft"))
        {
            if (!IsLoading)
            {
                IsLoading = true;
                LoadFeedOnFarmer(other);
            }


        }
        if (other.CompareTag("LS_ProductShelf"))
        {
            if (!IsLoading)
            {
                IsLoading = true;
                LoadProductOnFarmer(other);
            }
        }

        if (other.CompareTag("Market"))
        {
            if (!IsLoading)
            {
                IsLoading = true;
                GetMoneyFromCounter(other);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("HayLoft"))
        {
            RefreshGrid();
            IsLoading = false;
        }
        if (other.CompareTag("LS_ProductShelf"))
        {
            RefreshGrid();
            IsLoading = false;
        }
        if (other.CompareTag("Market"))
        {
            RefreshGrid();
            IsLoading = false;
        }
    }
    void LoadFeedOnFarmer(Collider other)
    {
        if (totalItems >= maxHayCapacity - 1)
        {
            farmerCapacityFullText.gameObject.SetActive(true);
            RefreshGrid();
            IsLoading = false;

        }
        else if (other.GetComponent<HayLoft>().feedStored <= 0)
        {
            RefreshGrid();
            IsLoading = false;

        }
        else if (other.GetComponent<HayLoft>().feedStored > 0)
        {
            feedCollected++;
            totalItems++;
            Transform feedCell = other.transform.GetChild(other.transform.childCount - 1);
            feedCell.SetParent(this.transform);
            other.GetComponent<HayLoft>().feedStored--;
            if ((other.GetComponent<HayLoft>().previousPositions.Count - 1) > 0)
                other.GetComponent<HayLoft>().previousPositions.RemoveAt(other.GetComponent<HayLoft>().previousPositions.Count - 1);
            feedCell.transform.GetChild(0).localScale = new(0.38f, 1.1f, 1.1f);
            feedCell.DOLocalJump(cellPositions[currentC, currentR], 3, 1, 0.3f).SetDelay(delay).SetEase(Ease.InOutSine);
            RefreshGrid();
            feedCell.localRotation = Quaternion.identity;
            UpdateGridPositions();
            previousPositions.Add(cellPositions[currentC, currentR]);
            delay += 0.0001f;
            other.GetComponent<HayLoft>().ResetGridPositions();
            other.GetComponent<HayLoft>().DisplayCrudeStorageCounter();
            IsLoading = false;
        }
    }
  
    private void GetMoneyFromCounter(Collider other)
    {

        if (other.transform.childCount <= 0)
        {
            IsLoading = false;
            delay = 0;
        }
        else if (other.transform.childCount > 0)
        {

            CurrencyManager.Instance.RecieveCoins(1);
            Transform money = other.transform.GetChild(other.transform.childCount - 1);
            money.SetParent(this.transform);
            money.DOLocalJump(coinPos.localPosition, 1, 1, 0.2f).SetDelay(delay).SetEase(Ease.OutSine).OnComplete(() => Destroy(money.gameObject));
            delay += 0.001f;
            other.GetComponent<MoneyStack>().ResetGridPositions();
            IsLoading = false;
        }
    }
    public void LoadProductOnFarmer(Collider other)
    {
        if (other.GetComponent<ProductStack>().type == ProductType.Egg)
        {
            if (totalItems >= maxHayCapacity - 1)
            {
                farmerCapacityFullText.gameObject.SetActive(true);
                RefreshGrid();
                IsLoading = false;

            }
            else if (other.GetComponent<ProductStack>().eggsGenerated <= 0)
            {
                IsLoading = false;
                RefreshGrid();
            }
            else if (other.GetComponent<ProductStack>().eggsGenerated > 0)
            {
                eggCollected++;
                totalItems++;
                other.GetComponent<ProductStack>().eggsGenerated--;
                Transform product = other.transform.GetChild(other.transform.childCount - 1);
                product.SetParent(this.transform);
                product.DOLocalJump(cellPositions[currentC, currentR], 2, 1, 0.2f).SetDelay(delay).SetEase(Ease.OutSine).OnComplete(() => product.localRotation = Quaternion.identity);
                previousPositions.Add(cellPositions[currentC, currentR]);
                if ((other.GetComponent<ProductStack>().previousPositions.Count - 1) > 0)
                    other.GetComponent<ProductStack>().previousPositions.RemoveAt(other.GetComponent<ProductStack>().previousPositions.Count - 1);
                delay += 0.0001f;
                UpdateGridPositions();
                other.GetComponent<ProductStack>().ResetGridPositions();
                CheckMax();
                IsLoading = false;

            }


        }
        if (other.GetComponent<ProductStack>().type == ProductType.Milk)
        {
            if (totalItems >= maxHayCapacity - 1)
            {
                farmerCapacityFullText.gameObject.SetActive(true);
                RefreshGrid();
                IsLoading = false;

            }
            else if (other.GetComponent<ProductStack>().milkGenerated <= 0)
            {
                IsLoading = false;
                RefreshGrid();
            }
            else if (other.GetComponent<ProductStack>().milkGenerated > 0)
            {
                milkCollected++;
                totalItems++;
                other.GetComponent<ProductStack>().milkGenerated--;
                Transform product = other.transform.GetChild(other.transform.childCount - 1);
                product.SetParent(this.transform);
                product.transform.GetChild(0).localScale = new(0.38f, 1.1f, 0.791418f);
                product.DOLocalJump(cellPositions[currentC, currentR], 2, 1, 0.2f).SetDelay(delay).SetEase(Ease.OutQuint).OnComplete(() => product.localRotation = Quaternion.identity);
                previousPositions.Add(cellPositions[currentC, currentR]);
                if ((other.GetComponent<ProductStack>().previousPositions.Count - 1) > 0)
                    other.GetComponent<ProductStack>().previousPositions.RemoveAt(other.GetComponent<ProductStack>().previousPositions.Count - 1);
                delay += 0.0001f;
                UpdateGridPositions();
                other.GetComponent<ProductStack>().ResetGridPositions();
                CheckMax();
                IsLoading = false;

            }


        }

    }
    public void RefreshGrid()
    {

        currentC = 0;
        currentR = 0;
        gridOffset.y = 0.92f;
        CalculateCellPositions();
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).DOLocalMove(cellPositions[currentC, currentR], 0.1f).SetEase(Ease.OutQuint);
            UpdateGridPositions();


        }
    }

}
