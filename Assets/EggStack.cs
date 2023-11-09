using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EggStack : Stacker
{
    public static event Action OnTroughFull;
    bool IsLoading;
    public int feedCollected;
    float delay;
    public Text feedCollectedText;
    [SerializeField] GameObject chickenTimer;
    [SerializeField] GameObject feedCounter;
    private void Start()
    {
        CalculateCellPositions();
        feedCollectedText.text = feedCollected.ToString() + "/" + maxHayCapacity.ToString();
    }
    private void OnEnable()
    {
        Timer.OnTimeOut += Reset;
    }

    private void Reset()
    {
        chickenTimer.SetActive(false);
        feedCounter.SetActive(true);
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
        if (other.transform.childCount == 0)
        {
            IsLoading = false;
        }
        else if (other.transform.childCount > 0)
        {
            if (transform.childCount >= maxHayCapacity)
            {
                Debug.LogError("Max Capacity Reached");
                OnTroughFull?.Invoke();
                chickenTimer.SetActive(true);
                feedCounter.SetActive(false);

            }
            else
            {
                
                if (other.transform.GetChild(0).CompareTag("Crude"))
                {
                    IsLoading = true;
                    feedCollected++;
                    feedCollectedText.text = feedCollected.ToString() + "/" + maxHayCapacity.ToString();
                    Transform feedCell = other.transform.GetChild(0);
                    DOTween.Complete(feedCell);
                    feedCell.SetParent(this.transform);
                    feedCell.DOLocalJump(cellPositions[currentC, currentR], 2, 1, 0.5f).SetDelay(delay).SetEase(Ease.OutSine).OnComplete(() => feedCell.localRotation = Quaternion.identity);
                    delay += 0.0001f;
                    UpdateGridPositions();
                    other.GetComponent<FarmerStack>().ResetGridPositions();
                    IsLoading = false;
                }
                else
                {
                    IsLoading = false;

                }
            }
        }
    }
}



