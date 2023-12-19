using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
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
    int everything;
    [Title("References")]
    [SerializeField] Transform coinPos;
    [SerializeField] public Text farmerCapacityFullText;
    [SerializeField] GameObject feedPrefab;
    [SerializeField] GameObject eggPrefab;
    [SerializeField] GameObject milkPrefab;
    //====================================
    public List<GameObject> TweeningMoney = new();
    float delay;
    bool IsLoading;
    int index;
    int n;
    AudioManager AM;
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
        AM = AudioManager.instance;
        SetGridYOffset(0.92f);
        CalculateCellPositions();
        LoadFeedCollected();
        UpdateMaxFarmerCapacity();
    }
    public void CheckMax()
    {
        //everything = feedCollected + eggCollected + milkCollected;
        if (totalItems >= maxHayCapacity - 1)
        {
            //print("Here");
            farmerCapacityFullText.gameObject.SetActive(true);
        }
        else
        {
            farmerCapacityFullText.gameObject.SetActive(false);

        }
    }
    void LoadEggsCollected()
    {
        if (eggCollected > 0)
        {
            for (int i = 0; i < eggCollected; i++)
            {
                GameObject cell = Instantiate(eggPrefab, this.transform);
                cell.transform.localPosition = previousPositions[index];
                index++;
                if (i == eggCollected - 1)
                {
                    CheckMax();
                    RefreshGrid();
                }
            }
        }
        else
        {
            index = 0;
            RefreshGrid();
            CheckMax();
        }
    }
    void LoadCheeseCollected()
    {
        if (milkCollected > 0)
        {
            for (int i = 0; i < milkCollected; i++)
            {
                GameObject cell = Instantiate(milkPrefab, this.transform);
                cell.transform.localPosition = previousPositions[index];
                cell.transform.GetChild(0).localScale = new(0.38f, 1.1f, 0.791418f);
                index++;
                if (i == milkCollected - 1)
                {
                    LoadEggsCollected();
                }
            }
        }
        else
        {
            LoadEggsCollected();
        }
    }
    void LoadFeedCollected()
    {
        if (feedCollected > 0)
        {
            for (int i = 0; i < feedCollected; i++)
            {
                GameObject cell = Instantiate(feedPrefab, this.transform);
                cell.transform.localPosition = previousPositions[index];
                cell.transform.GetChild(0).localScale = new(0.38f, 1.1f, 1.1f);
                index++;
                if (i == feedCollected - 1)
                {
                    LoadCheeseCollected();
                }
            }
        }
        else
        {
            LoadCheeseCollected();
        }


        //RefreshGrid();

    }
    private void UpdateMaxFarmerCapacity()
    {
        maxHayCapacity = FarmUpgradeManager.Instance.maxFarmerCapacity;
        CheckMax();
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
            AudioManager.instance.Stop("PickupStuff");
            RefreshGrid();
            IsLoading = false;
        }
        if (other.CompareTag("LS_ProductShelf"))
        {
            AudioManager.instance.Stop("PickupStuff");
            RefreshGrid();
            IsLoading = false;
        }
        if (other.CompareTag("Market"))
        {
            AM.Stop("GetMoney");
            IsLoading = false;
        }
    }
    void LoadFeedOnFarmer(Collider other)
    {
        if (totalItems >= maxHayCapacity - 1)
        {
            farmerCapacityFullText.gameObject.SetActive(true);
            RefreshGrid();
            AudioManager.instance.Stop("PickupStuff");
            IsLoading = false;

        }
        else if (other.GetComponent<HayLoft>().feedStored <= 0)
        {
            RefreshGrid();
            AudioManager.instance.Stop("PickupStuff");
            IsLoading = false;

        }
        else if (other.GetComponent<HayLoft>().feedStored > 0)
        {
            feedCollected++;
            if (!AudioManager.instance.IsPlaying("PickupStuff"))
                AudioManager.instance.Play("PickupStuff");
            totalItems++;
            Transform feedCell = other.transform.GetChild(other.transform.childCount - 1);
            feedCell.SetParent(this.transform);
            other.GetComponent<HayLoft>().feedStored--;
            if ((other.GetComponent<HayLoft>().previousPositions.Count - 1) >= 0)
                other.GetComponent<HayLoft>().previousPositions.RemoveAt(other.GetComponent<HayLoft>().previousPositions.Count - 1);
            feedCell.transform.GetChild(0).localScale = new(0.38f, 1.1f, 1.1f);
            feedCell.DOLocalJump(cellPositions[currentR, currentC], 3, 1, 0.3f).SetDelay(delay).SetEase(Ease.InOutSine);
            feedCell.localRotation = Quaternion.identity;
            UpdateGridPositions();
            previousPositions.Add(cellPositions[currentR, currentC]);
            delay += 0.00001f;
            other.GetComponent<HayLoft>().ResetGridPositions();
            other.GetComponent<HayLoft>().DisplayCrudeStorageCounter();
            RefreshGrid();
            DOTween.Complete(feedCell);
            IsLoading = false;
        }
    }
    private void GetMoneyFromCounter(Collider other)
    {

        if (other.GetComponent<MoneyStack>().coinsStored <= 0)
        {
            IsLoading = false;
            AM.Stop("GetMoney");
            delay = 0;
        }
        else if (other.GetComponent<MoneyStack>().coinsStored > 0)
        {
            if (!AM.IsPlaying("GetMoney"))
            {
                AM.Play("GetMoney");
                VibrationManager.SpecialVibrate(SpecialVibrationTypes.Peek);
            }
            CurrencyManager.Instance.RecieveCoins(1);
            Transform money = other.transform.GetChild(other.transform.childCount - 1);
            TweeningMoney.Add(money.gameObject);
            money.SetParent(this.transform);
            money.DOLocalJump(coinPos.localPosition, 1, 1, 0.2f).SetDelay(delay).SetEase(Ease.OutSine).OnComplete(() =>
            {
                Destroy(TweeningMoney[n]);
                n++;
            });
            delay += 0.001f;
            other.GetComponent<MoneyStack>().ResetGridPositions();
            if ((other.GetComponent<MoneyStack>().previousPositions.Count - 1) >= 0)
                other.GetComponent<MoneyStack>().previousPositions.RemoveAt(other.GetComponent<MoneyStack>().previousPositions.Count - 1);
            other.GetComponent<MoneyStack>().coinsStored--;
            IsLoading = false;
        }
    }
    public void LoadProductOnFarmer(Collider other)
    {
        if (other.GetComponent<ProductStack>().type == ProductType.Egg)
        {
            if (totalItems >= maxHayCapacity - 1)
            {
                AudioManager.instance.Stop("PickupStuff");
                farmerCapacityFullText.gameObject.SetActive(true);
                RefreshGrid();
                IsLoading = false;

            }
            else if (other.GetComponent<ProductStack>().eggsGenerated <= 0)
            {
                AudioManager.instance.Stop("PickupStuff");
                IsLoading = false;
                RefreshGrid();
            }
            else if (other.GetComponent<ProductStack>().eggsGenerated > 0)
            {
                if (!AudioManager.instance.IsPlaying("PickupStuff"))
                    AudioManager.instance.Play("PickupStuff");
                eggCollected++;
                totalItems++;
                other.GetComponent<ProductStack>().eggsGenerated--;
                Transform product = other.transform.GetChild(other.transform.childCount - 1);
                product.SetParent(this.transform);
                product.DOLocalJump(cellPositions[currentR, currentC], 3, 1, 0.2f).SetDelay(delay).SetEase(Ease.OutSine);
                product.localRotation = Quaternion.identity;
                previousPositions.Add(cellPositions[currentR, currentC]);
                if ((other.GetComponent<ProductStack>().previousPositions.Count - 1) >= 0)
                    other.GetComponent<ProductStack>().previousPositions.RemoveAt(other.GetComponent<ProductStack>().previousPositions.Count - 1);
                delay += 0.0001f;
                UpdateGridPositions();
                other.GetComponent<ProductStack>().ResetGridPositions();
                CheckMax();
                DOTween.Complete(product);
                RefreshGrid();
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
                if (!AudioManager.instance.IsPlaying("PickupStuff"))
                    AudioManager.instance.Play("PickupStuff");
                milkCollected++;
                totalItems++;
                other.GetComponent<ProductStack>().milkGenerated--;
                Transform product = other.transform.GetChild(other.transform.childCount - 1);
                product.SetParent(this.transform);
                product.transform.GetChild(0).localScale = new(0.38f, 1.1f, 0.791418f);
                product.DOLocalJump(cellPositions[currentR, currentC], 3, 1, 0.2f).SetDelay(delay).SetEase(Ease.OutQuint);
                product.localRotation = Quaternion.identity;
                previousPositions.Add(cellPositions[currentR, currentC]);
                if ((other.GetComponent<ProductStack>().previousPositions.Count - 1) >= 0)
                    other.GetComponent<ProductStack>().previousPositions.RemoveAt(other.GetComponent<ProductStack>().previousPositions.Count - 1);
                delay += 0.0001f;
                UpdateGridPositions();
                other.GetComponent<ProductStack>().ResetGridPositions();
                CheckMax();
                DOTween.Complete(product);
                RefreshGrid();
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
            //transform.GetChild(i).DOLocalMove(cellPositions[currentR, currentC], 0.1f).SetEase(Ease.OutQuint);
            transform.GetChild(i).localPosition = cellPositions[currentR, currentC];
            UpdateGridPositions();
        }
    }

}
