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
    public List<GameObject> TweeningFeed = new();
    int tweeningFeedIndex;
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
    public bool CheckMax()
    {
        if (totalItems >= maxHayCapacity - 1)
        {
            farmerCapacityFullText.gameObject.SetActive(true);
            return true;
        }
        else
        {
            farmerCapacityFullText.gameObject.SetActive(false);
            return false;

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
                cell.transform.GetChild(0).localScale = new(0.202438757f, 0.276839554f, 0.336927205f);
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
            if (other.GetComponent<HayLoft>().feedStored > 0)
            {
                if (!IsLoading)
                {
                    IsLoading = true;
                    StartCoroutine(LoadFeedOnFarmer(other));

                }
            }
            //else if (other.GetComponent<HayLoft>().feedStored <= 0)
            //RefreshGrid();


        }
        if (other.CompareTag("LS_ProductShelf"))
        {
            if (other.GetComponent<ProductStack>().type == ProductType.Egg)
            {
                if (other.GetComponent<ProductStack>().eggsGenerated > 0)
                {
                    if (!IsLoading)
                    {
                        IsLoading = true;
                        StartCoroutine(LoadProductOnFarmer(other));

                    }
                }
                //else if (other.GetComponent<ProductStack>().milkGenerated <= 0)
                //RefreshGrid();
            }
            else if (other.GetComponent<ProductStack>().type == ProductType.Milk)
            {
                if (other.GetComponent<ProductStack>().milkGenerated > 0)
                {
                    if (!IsLoading)
                    {
                        IsLoading = true;
                        StartCoroutine(LoadProductOnFarmer(other));

                    }
                }
                //else if (other.GetComponent<ProductStack>().milkGenerated <= 0)
                //RefreshGrid();

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
            RefreshGrid();
            TweeningMoney.Clear();
            n = 0;
            AM.Stop("GetMoney");
            IsLoading = false;
        }
    }
    IEnumerator LoadFeedOnFarmer(Collider other)
    {
        while (IsLoading && other.GetComponent<HayLoft>().feedStored > 0)
        {
            if (totalItems >= maxHayCapacity - 1)
            {
                farmerCapacityFullText.gameObject.SetActive(true);
                AudioManager.instance.Stop("PickupStuff");
                break;
            }
            feedCollected++;
            if (!AudioManager.instance.IsPlaying("PickupStuff"))
                AudioManager.instance.Play("PickupStuff");
            totalItems++;
            Transform feedCell = other.transform.GetChild(other.transform.childCount - 1);
            feedCell.SetParent(this.transform);
            feedCell.transform.GetChild(0).localScale = new(0.38f, 1.1f, 1.1f);
            feedCell.transform.DOLocalJump(cellPositions[currentR, currentC], 3, 1, 0.5f).SetDelay(delay).SetEase(Ease.Linear);
            // Debug.LogError(cellPositions[currentR, currentC]);
            other.GetComponent<HayLoft>().feedStored--;
            if ((other.GetComponent<HayLoft>().previousPositions.Count - 1) >= 0)
                other.GetComponent<HayLoft>().previousPositions.RemoveAt(other.GetComponent<HayLoft>().previousPositions.Count - 1);
            feedCell.localRotation = Quaternion.identity;
            UpdateGridPositions();
            previousPositions.Add(cellPositions[currentR, currentC]);
            delay += 0.000001f;
            other.GetComponent<HayLoft>().ResetGridPositions();
            other.GetComponent<HayLoft>().DisplayCrudeStorageCounter();
            yield return null;


        }
        RefreshGrid();
        IsLoading = false;
        delay = 0;
    }
    IEnumerator LoadProductOnFarmer(Collider other)
    {
        if (other.GetComponent<ProductStack>().type == ProductType.Egg)
        {

            while (IsLoading && other.GetComponent<ProductStack>().eggsGenerated > 0)
            {
                if (totalItems >= maxHayCapacity - 1)
                {
                    AudioManager.instance.Stop("PickupStuff");
                    farmerCapacityFullText.gameObject.SetActive(true);
                    break;
                }
                if (!AudioManager.instance.IsPlaying("PickupStuff"))
                    AudioManager.instance.Play("PickupStuff");
                eggCollected++;
                totalItems++;
                other.GetComponent<ProductStack>().eggsGenerated--;
                Transform product = other.transform.GetChild(other.transform.childCount - 1);
                product.SetParent(this.transform);
                product.DOLocalJump(cellPositions[currentR, currentC], 3, 1, 0.5f).SetDelay(delay).SetEase(Ease.Linear);
                product.localRotation = Quaternion.identity;
                previousPositions.Add(cellPositions[currentR, currentC]);
                if ((other.GetComponent<ProductStack>().previousPositions.Count - 1) >= 0)
                    other.GetComponent<ProductStack>().previousPositions.RemoveAt(other.GetComponent<ProductStack>().previousPositions.Count - 1);
                delay += 0.000001f;
                UpdateGridPositions();
                other.GetComponent<ProductStack>().ResetGridPositions();
                yield return null;
            }
            RefreshGrid();
            IsLoading = false;
            delay = 0;
        }
        else if (other.GetComponent<ProductStack>().type == ProductType.Milk)
        {
            while (IsLoading && other.GetComponent<ProductStack>().milkGenerated > 0)
            {
                if (totalItems >= maxHayCapacity - 1)
                {
                    AudioManager.instance.Stop("PickupStuff");
                    farmerCapacityFullText.gameObject.SetActive(true);
                    break;
                }
                if (!AudioManager.instance.IsPlaying("PickupStuff"))
                    AudioManager.instance.Play("PickupStuff");
                milkCollected++;
                totalItems++;
                other.GetComponent<ProductStack>().milkGenerated--;
                Transform product = other.transform.GetChild(other.transform.childCount - 1);
                product.SetParent(this.transform);
                product.transform.GetChild(0).localScale = new Vector3(0.202438757f, 0.276839554f, 0.336927205f);
                product.DOLocalJump(cellPositions[currentR, currentC], 3, 1, 0.5f).SetDelay(delay).SetEase(Ease.Linear);
                product.localRotation = Quaternion.identity;
                previousPositions.Add(cellPositions[currentR, currentC]);
                if ((other.GetComponent<ProductStack>().previousPositions.Count - 1) >= 0)
                    other.GetComponent<ProductStack>().previousPositions.RemoveAt(other.GetComponent<ProductStack>().previousPositions.Count - 1);
                delay += 0.000001f;
                UpdateGridPositions();
                other.GetComponent<ProductStack>().ResetGridPositions();
                yield return null;
            }
            RefreshGrid();
            IsLoading = false;
            delay = 0;

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
            CurrencyManager.Instance.RecieveCoins(5);
            Transform money = other.transform.GetChild(other.transform.childCount - 1);
            TweeningMoney.Add(money.gameObject);
            money.SetParent(this.transform);
            money.DOLocalJump(coinPos.localPosition, 2, 1, 0.4f).SetDelay(delay).SetEase(Ease.OutSine).OnComplete(() =>
            {
                Destroy(TweeningMoney[n]);
                n++;
            });
            //delay += 0.001f;
            other.GetComponent<MoneyStack>().ResetGridPositions();
            if ((other.GetComponent<MoneyStack>().previousPositions.Count - 1) >= 0)
                other.GetComponent<MoneyStack>().previousPositions.RemoveAt(other.GetComponent<MoneyStack>().previousPositions.Count - 1);
            other.GetComponent<MoneyStack>().coinsStored--;
            IsLoading = false;
        }
    }
    public void RefreshGrid()
    {
        currentR = 0;
        currentC = 0;
        gridOffset.y = 0.92f;
        CalculateCellPositions();
        for (int i = 0; i < transform.childCount; i++)
        {
            DOTween.Complete(transform.GetChild(i));
            transform.GetChild(i).localPosition = cellPositions[currentR, currentC];
            UpdateGridPositions();

        }

    }

}
