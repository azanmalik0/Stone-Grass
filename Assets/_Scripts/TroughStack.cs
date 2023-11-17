using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TroughStack : Stacker
{

    public static event Action OnTroughFull;
    public enum TroughType { Chicken, Cow }
    public TroughType troughType;
    [Title("References")]
    [SerializeField] GameObject produceTimer;
    [SerializeField] GameObject crudeCounter;
    [SerializeField] Text crudeTroughCapacityText;
    public int feedCollected;
    bool IsLoading;
    float delay;
    public float initialYOffset;
    int crudeCheckIndex = 1;
    [SerializeField] GameObject feedPrefab;
    private void Awake()
    {
        SetGridYOffset(gridOffset.y);

    }
    private void Start()
    {
        CalculateCellPositions();
        ES3AutoSaveMgr.Current.Load();
        LoadFeedCollected();
        DisplayCrudeTroughCounter();
    }
    private void LoadFeedCollected()
    {
        for (int i = 0; i < feedCollected; i++)
        {
            GameObject cell = Instantiate(feedPrefab, this.transform);
            cell.transform.localPosition = previousPositions[i];

        }
    }

    private void DisplayCrudeTroughCounter()
    {
        if (troughType == TroughType.Chicken)
        {
            maxHayCapacity = FarmUpgradeManager.Instance.maxChickenTrayCapacity;
            crudeTroughCapacityText.text = $"{transform.childCount}/{maxHayCapacity}";

        }
        if (troughType == TroughType.Cow)
        {

            maxHayCapacity = FarmUpgradeManager.Instance.maxCowTrayCapacity;
            crudeTroughCapacityText.text = $"{transform.childCount}/{maxHayCapacity}";
        }
    }

    private void OnEnable()
    {
        Timer.OnTimeOut += Reset;
        FarmUpgradeManager.OnIncreasingTrayCapcaity += DisplayCrudeTroughCounter;
    }

    private void OnDisable()
    {
        Timer.OnTimeOut -= Reset;
        FarmUpgradeManager.OnIncreasingTrayCapcaity -= DisplayCrudeTroughCounter;

    }
    private void Reset()
    {
        produceTimer.SetActive(false);
        crudeCounter.SetActive(true);
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Farmer_Stack"))
        {
            if (!IsLoading)
            {
                LoadFeedOnTrough(other);
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
    private void LoadFeedOnTrough(Collider other)
    {
        if (transform.childCount >= maxHayCapacity)
        {
            OnTroughFull?.Invoke();
            produceTimer.SetActive(true);
            crudeCounter.SetActive(false);

        }
        else if (other.transform.childCount > 0)
        {
            if (crudeCheckIndex > other.transform.childCount)
            {
                crudeCheckIndex = 1;
                IsLoading = false;
            }
            else
            {
                if (!other.transform.GetChild(other.transform.childCount - crudeCheckIndex).CompareTag("Crude"))
                {
                    crudeCheckIndex++;
                    IsLoading = false;
                }
                else
                {
                    feedCollected++;
                    other.GetComponent<FarmerStack>().feedCollected--;
                    IsLoading = true;
                    Transform crudeCell = other.transform.GetChild(other.transform.childCount - crudeCheckIndex);
                    DOTween.Complete(crudeCell);
                    crudeCell.SetParent(this.transform);
                    DisplayCrudeTroughCounter();
                    crudeCell.DOLocalJump(cellPositions[currentC, currentR], 2, 1, 0.5f).SetDelay(delay).SetEase(Ease.OutSine).OnComplete(() => crudeCell.localRotation = Quaternion.identity);
                    previousPositions.Add(cellPositions[currentC, currentR]);
                    if ((other.GetComponent<FarmerStack>().previousPositions.Count - 1) > 0)
                        other.GetComponent<FarmerStack>().previousPositions.RemoveAt(other.GetComponent<FarmerStack>().previousPositions.Count - 1);
                    delay += 0.0001f;
                    UpdateGridPositions();
                    other.GetComponent<FarmerStack>().ResetGridPositions();
                    other.GetComponent<FarmerStack>().RefreshGrid();
                    IsLoading = false;

                }
            }

        }

    }
    private void OnApplicationQuit()
    {
        ES3AutoSaveMgr.Current.Save();
    }

}



