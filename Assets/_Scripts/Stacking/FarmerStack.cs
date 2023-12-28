using DG.Tweening;
using EZ_Pooling;
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
    public bool IsLoading;
    int index;
    int n;
    AudioManager AM;
    public bool CanStackCheese;
    public bool IsLoadingOnTrough;
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
            if (!IsLoading)
            {
                IsLoading = true;
                StartCoroutine(LoadFeedOnFarmer(other));

            }

        }
        if (other.CompareTag("LS_ProductShelf"))
        {
            if (other.GetComponent<ProductStack>().type == ProductType.Egg)
            {
                if (!IsLoading)
                {

                    IsLoading = true;
                    StartCoroutine(LoadProductOnFarmer(other));

                }
            }
            else if (other.GetComponent<ProductStack>().type == ProductType.Milk)
            {
                if (!IsLoading)
                {
                    IsLoading = true;
                    StartCoroutine(LoadProductOnFarmer(other));

                }

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
            IsLoading = false;
        }
        if (other.CompareTag("LS_ProductShelf"))
        {
            IsLoading = false;
        }
        if (other.CompareTag("Market"))
        {
            IsLoading = false;
        }
    }
    IEnumerator LoadFeedOnFarmer(Collider other)
    {

        while (IsLoading && other.GetComponent<HayLoft>().feedStored > 0 && !CheckMax())
        {
            feedCollected++;
            totalItems++;
            Transform feedCell = other.transform.GetChild(other.transform.childCount - 1);
            DOTween.Complete(feedCell);
            feedCell.SetParent(this.transform);
            feedCell.transform.GetChild(0).localScale = new(0.38f, 1.1f, 1.1f);
            feedCell.transform.DOLocalJump(cellPositions[currentR, currentC], 3, 1, 0.5f).SetDelay(delay).SetEase(Ease.Linear);
            other.GetComponent<HayLoft>().feedStored--;
            if ((other.GetComponent<HayLoft>().previousPositions.Count - 1) >= 0)
                other.GetComponent<HayLoft>().previousPositions.RemoveAt(other.GetComponent<HayLoft>().previousPositions.Count - 1);
            feedCell.localRotation = Quaternion.identity;
            UpdateGridPositions();
            previousPositions.Add(cellPositions[currentR, currentC]);
            delay += 0.000001f;
            other.GetComponent<HayLoft>().ResetGridPositions();
            other.GetComponent<HayLoft>().DisplayCrudeStorageCounter();
            if (CheckMax())
                break;

            yield return null;
        }
        IsLoading = false;
        delay = 0;
    }
    IEnumerator LoadProductOnFarmer(Collider other)
    {
        if (other.GetComponent<ProductStack>().type == ProductType.Egg)
        {
            while (other.GetComponent<ProductStack>().eggsGenerated > 0 && !CheckMax())
            {
                eggCollected++;
                totalItems++;
                other.GetComponent<ProductStack>().eggsGenerated--;
                Transform product = other.transform.GetChild(other.transform.childCount - 1);
                DOTween.Complete(product);
                product.SetParent(this.transform);
                product.DOLocalJump(cellPositions[currentR, currentC], 3, 1, 0.5f).SetDelay(delay).SetEase(Ease.Linear);
                product.localRotation = Quaternion.identity;
                previousPositions.Add(cellPositions[currentR, currentC]);
                if ((other.GetComponent<ProductStack>().previousPositions.Count - 1) >= 0)
                    other.GetComponent<ProductStack>().previousPositions.RemoveAt(other.GetComponent<ProductStack>().previousPositions.Count - 1);
                delay += 0.000001f;
                UpdateGridPositions();
                other.GetComponent<ProductStack>().ResetGridPositions();
                if (CheckMax())
                    break;
                yield return null;

            }
            IsLoading = false;
            delay = 0;
        }
        else if (other.GetComponent<ProductStack>().type == ProductType.Milk)
        {
            while (other.GetComponent<ProductStack>().milkGenerated > 0 && !CheckMax())
            {
                milkCollected++;
                totalItems++;
                other.GetComponent<ProductStack>().milkGenerated--;
                Transform product = other.transform.GetChild(other.transform.childCount - 1);
                DOTween.Complete(product);
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
                if (CheckMax())
                    break;
                yield return null;
            }
            IsLoading = false;
            delay = 0;


        }

    }
    private void GetMoneyFromCounter(Collider other)
    {

        if (other.GetComponent<MoneyStack>().coinsStored <= 0)
        {
            IsLoading = false;
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
            DOTween.Complete(money);
            TweeningMoney.Add(money.gameObject);
            money.SetParent(coinPos.transform);
            money.DOLocalJump(coinPos.localPosition, 2, 1, 0.4f).SetDelay(delay).SetEase(Ease.OutSine).OnComplete(() =>
            {
                EZ_PoolManager.Despawn(money);
            });
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
