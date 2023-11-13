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
    [Title("References")]
    [SerializeField] GameObject produceTimer;
    [SerializeField] GameObject crudeCounter;
    [SerializeField] Text feedCollectedText;
    public int feedCollected;
    bool IsLoading;
    float delay;
    public float initialYOffset;
    int crudeCheckIndex = 1;
    private void Start()
    {
        initialYOffset = gridOffset.y;
        CalculateCellPositions();
        feedCollectedText.text = transform.childCount.ToString() + "/" + maxHayCapacity.ToString();
    }
    private void OnEnable()
    {
        Timer.OnTimeOut += Reset;
    }

    private void Reset()
    {
        produceTimer.SetActive(false);
        crudeCounter.SetActive(true);
    }

    private void OnDisable()
    {
        Timer.OnTimeOut -= Reset;

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Farmer_Stack"))
        {
            if (!IsLoading)
            {
                Debug.LogError("Farmer_Stack");
                LoadFeedOnTrough(other);
            }
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Farmer_Stack"))
        {
            IsLoading = false;
        }
    }
    private void LoadFeedOnTrough(Collider other)
    {
        if (transform.childCount >= maxHayCapacity)
        {
            Debug.LogError("Max Capacity Reached");
            OnTroughFull?.Invoke();
            produceTimer.SetActive(true);
            crudeCounter.SetActive(false);


        }
        else
        {

            if (other.transform.childCount == 0)
            {
                IsLoading = false;
            }
            else if (other.transform.childCount > 0)
            {
                if (crudeCheckIndex <= other.transform.childCount)
                {

                    if (other.transform.GetChild(other.transform.childCount - crudeCheckIndex).CompareTag("Crude"))
                    {
                        IsLoading = true;
                        Transform feedCell = other.transform.GetChild(other.transform.childCount - crudeCheckIndex);
                        DOTween.Complete(feedCell);
                        feedCell.SetParent(this.transform);
                        feedCollectedText.text = transform.childCount.ToString() + "/" + maxHayCapacity.ToString();
                        feedCell.DOLocalJump(cellPositions[currentC, currentR], 2, 1, 0.5f).SetDelay(delay).SetEase(Ease.OutSine).OnComplete(() => feedCell.localRotation = Quaternion.identity);
                        delay += 0.0001f;
                        UpdateGridPositions(initialYOffset);
                        other.GetComponent<FarmerStack>().ResetGridPositions(other.GetComponent<FarmerStack>().initialYOffset);
                        IsLoading = false;
                    }
                    else
                    {
                        crudeCheckIndex++;
                        IsLoading = false;

                    }
                }
                else
                {
                    crudeCheckIndex = 1;
                    IsLoading = false;
                }
            }
        }

    }

}



