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
    public static event Action OnMoneyCollect;
    public static FarmerStack Instance;
    [SerializeField] Transform coinPos;
    bool IsLoading;
    float delay;
    [SerializeField] Text farmerCapacityFullText;
    public int feedCollected;
    public int milkCollected;
    public int eggCollected;
    [SerializeField] GameObject feedPrefab;
    [SerializeField] GameObject eggPrefab;
    [SerializeField] GameObject milkPrefab;

    private void Awake()
    {
        SetGridYOffset(gridOffset.y);
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
        CalculateCellPositions();
        ES3AutoSaveMgr.Current.Load();
        LoadStack();
        UpdateMaxFarmerCapacity();
        RefreshGrid();
    }
    private void LoadStack()
    {

        for (int i = 0; i < feedCollected; i++)
        {
            GameObject cell = Instantiate(feedPrefab, this.transform);
            cell.transform.localPosition = previousPositions[i];

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
                LoadFeedOnFarmer(other);
            }


        }
        if (other.CompareTag("LS_ProductShelf"))
        {
            if (!IsLoading)
            {
                LoadProductOnFarmer(other);
            }
        }

        if (other.CompareTag("Market"))
        {
            if (!IsLoading)
            {
                GetMoneyFromCounter(other);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("HayLoft"))
        {
            IsLoading = false;
            RefreshGrid();
        }
        if (other.CompareTag("LS_ProductShelf"))
        {
            IsLoading = false;
            RefreshGrid();
        }
        if (other.CompareTag("Market"))
        {
            IsLoading = false;
        }
    }
    void LoadFeedOnFarmer(Collider other)
    {
        if (transform.childCount >= maxHayCapacity)
        {
            RefreshGrid();
            AnimateMax();

        }
        else if (other.transform.childCount == 0)
        {
            IsLoading = false;
            RefreshGrid();
        }
        else if (other.transform.childCount > 0)
        {
            feedCollected++;
            other.GetComponent<HayLoft>().feedStored--;
            IsLoading = true;
            Transform feedCell = other.transform.GetChild(other.transform.childCount - 1);
            DOTween.Complete(feedCell);
            feedCell.SetParent(this.transform);
            if ((other.GetComponent<HayLoft>().previousPositions.Count - 1) > 0)
                other.GetComponent<HayLoft>().previousPositions.RemoveAt(other.GetComponent<HayLoft>().previousPositions.Count - 1);
            feedCell.DOLocalJump(cellPositions[currentC, currentR], 2, 1, 0.5f).SetDelay(delay).SetEase(Ease.OutSine).OnComplete(() => feedCell.localRotation = Quaternion.identity);
            previousPositions.Add(cellPositions[currentC, currentR]);
            delay += 0.0001f;
            UpdateGridPositions();
            //RefreshGrid();
            other.GetComponent<HayLoft>().ResetGridPositions();
            IsLoading = false;
        }
    }

    private void AnimateMax()
    {
        farmerCapacityFullText.gameObject.SetActive(true);
        if (!DOTween.IsTweening(farmerCapacityFullText))
            farmerCapacityFullText.DOColor(new Color32(255, 255, 255, 0), 0.5f).SetLoops(5, LoopType.Yoyo).SetEase(Ease.InOutSine).OnComplete(() => farmerCapacityFullText.gameObject.SetActive(false));
    }

    private void GetMoneyFromCounter(Collider other)
    {

        if (other.transform.childCount == 0)
        {
            IsLoading = false;
            delay = 0;
        }
        else if (other.transform.childCount > 0)
        {
            OnMoneyCollect?.Invoke();
            IsLoading = true;
            Transform money = other.transform.GetChild(other.transform.childCount - 1);
            DOTween.Complete(money);
            money.SetParent(this.transform);
            money.DOLocalMove(coinPos.position, 0.5f).SetDelay(delay).SetEase(Ease.OutSine).OnComplete(() => Destroy(money.gameObject));
            delay += 0.01f;
            other.GetComponent<MoneyStack>().ResetGridPositions();
            IsLoading = false;
        }
    }
    public void LoadProductOnFarmer(Collider other)
    {
        if (transform.childCount > maxHayCapacity)
        {

            RefreshGrid();
            farmerCapacityFullText.text = "MAX";
            //Debug.LogError("MaxCapacityReached");

        }
        if (other.transform.childCount == 0)
        {
            IsLoading = false;
            RefreshGrid();
        }
        else if (other.transform.childCount > 0)
        {
            if (other.GetComponent<ProductStack>().type == ProductType.Egg)
            {
                eggCollected++;
                other.GetComponent<ProductStack>().eggsGenerated--;

            }
            if (other.GetComponent<ProductStack>().type == ProductType.Milk)
            {

                milkCollected++;
                other.GetComponent<ProductStack>().milkGenerated--;
            }
            IsLoading = true;
            Transform product = other.transform.GetChild(other.transform.childCount - 1);
            DOTween.Complete(product);
            product.SetParent(this.transform);
            product.DOLocalJump(cellPositions[currentC, currentR], 2, 1, 0.5f).SetDelay(delay).SetEase(Ease.OutSine).OnComplete(() => product.localRotation = Quaternion.identity);
            previousPositions.Add(cellPositions[currentC, currentR]);
            if ((other.GetComponent<ProductStack>().previousPositions.Count - 1) > 0)
                other.GetComponent<ProductStack>().previousPositions.RemoveAt(other.GetComponent<ProductStack>().previousPositions.Count - 1);
            delay += 0.0001f;
            UpdateGridPositions();
            RefreshGrid();
            other.GetComponent<ProductStack>().ResetGridPositions();
            IsLoading = false;
        }
    }
    private void OnApplicationQuit()
    {
        ES3AutoSaveMgr.Current.Save();
    }


}
