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
    public AnimalType animalType;
    public static event Action<AnimalType> OnTroughFull;
    public enum TroughType { Chicken, Cow }
    public TroughType troughType;
    [Title("References")]
    [SerializeField] GameObject produceTimer;
    [SerializeField] GameObject crudeCounter;
    [SerializeField] Text crudeTroughCapacityText;
    public int feedStored;
    bool IsLoading;
    float delay;
    public float initialYOffset;
    int crudeCheckIndex = 1;
    [SerializeField] GameObject feedPrefab;

    private void Start()
    {
        SetGridYOffset(0.42f);
        CalculateCellPositions();
        LoadFeedCollected();
        DisplayCrudeTroughCounter();
    }
    private void LoadFeedCollected()
    {
        for (int i = 0; i < feedStored; i++)
        {
            GameObject cell = Instantiate(feedPrefab, this.transform);
            cell.transform.localPosition = previousPositions[i];
            if (i == feedStored - 1)
            {
                if (feedStored >= maxHayCapacity)
                {
                    if (animalType == AnimalType.Chicken)
                        OnTroughFull?.Invoke(AnimalType.Chicken);
                    if (animalType == AnimalType.Cow)
                        OnTroughFull?.Invoke(AnimalType.Cow);
                    produceTimer.SetActive(true);
                    crudeCounter.SetActive(false);
                }
            }
        }
    }
    public void DisplayCrudeTroughCounter()
    {
        if (troughType == TroughType.Chicken)
        {
            maxHayCapacity = FarmUpgradeManager.Instance.maxChickenTrayCapacity;
            crudeTroughCapacityText.text = $"{feedStored}/{maxHayCapacity}";

        }
        if (troughType == TroughType.Cow)
        {

            maxHayCapacity = FarmUpgradeManager.Instance.maxCowTrayCapacity;
            crudeTroughCapacityText.text = $"{feedStored}/{maxHayCapacity}";
        }
    }
    private void OnEnable()
    {
        Timer.OnTimeOut += ResetObjects;
        FarmUpgradeManager.OnIncreasingTrayCapcaity += DisplayCrudeTroughCounter;
    }
    private void OnDisable()
    {
        Timer.OnTimeOut -= ResetObjects;
        FarmUpgradeManager.OnIncreasingTrayCapcaity -= DisplayCrudeTroughCounter;

    }
    private void ResetObjects(AnimalType animal)
    {
        if (animal == animalType)
        {
            IsLoading = false;
            produceTimer.SetActive(false);
            crudeCounter.SetActive(true);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Farmer_Stack"))
        {
            if (!IsLoading)
            {
                IsLoading = true;
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
        if (feedStored >= maxHayCapacity)
        {
            if (animalType == AnimalType.Chicken)
                OnTroughFull?.Invoke(AnimalType.Chicken);
            if (animalType == AnimalType.Cow)
                OnTroughFull?.Invoke(AnimalType.Cow);
            produceTimer.SetActive(true);
            crudeCounter.SetActive(false);

        }
        else if (other.GetComponent<FarmerStack>().feedCollected <= 0)
        {

            other.GetComponent<FarmerStack>().RefreshGrid();
            IsLoading = false;
        }
        else if (other.GetComponent<FarmerStack>().feedCollected > 0)
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
                    feedStored++;
                    other.GetComponent<FarmerStack>().feedCollected--;
                    Transform crudeCell = other.transform.GetChild(other.transform.childCount - crudeCheckIndex);
                    //DOTween.Complete(crudeCell);
                    crudeCell.SetParent(this.transform);
                    DisplayCrudeTroughCounter();
                    if ((other.GetComponent<FarmerStack>().previousPositions.Count - 1) > 0)
                        other.GetComponent<FarmerStack>().previousPositions.RemoveAt(other.GetComponent<FarmerStack>().previousPositions.Count - 1);
                    crudeCell.transform.GetChild(0).localScale = new(1.1f, 1.1f, 1.1f);
                    crudeCell.DOLocalJump(cellPositions[currentC, currentR], 2, 1, 0.2f).SetDelay(delay).SetEase(Ease.OutSine);
                    crudeCell.localRotation = Quaternion.identity;
                    previousPositions.Add(cellPositions[currentC, currentR]);
                    UpdateGridPositions();
                    delay += 0.0001f;
                    other.GetComponent<FarmerStack>().ResetGridPositions();
                    other.GetComponent<FarmerStack>().totalItems--;
                    other.GetComponent<FarmerStack>().farmerCapacityFullText.gameObject.SetActive(false);
                    //other.GetComponent<FarmerStack>().CheckMax();
                    IsLoading = false;

                }
            }

        }

    }
}
public enum AnimalType { Cow, Chicken }



