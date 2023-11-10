using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TroughStack : Stacker
{
    public static event Action OnTroughFull;
    bool IsLoading;
    public int feedCollected;
    float delay;
    public Text feedCollectedText;
    [SerializeField] GameObject chickenTimer;
    [SerializeField] GameObject feedCounter;
    public float initialYOffset;
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
        //GetComponent<BoxCollider>().enabled = true;
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
    public int n = 1;
    private void LoadFeedOnTrough(Collider other)
    {
        if (transform.childCount >= maxHayCapacity)
        {
            Debug.LogError("Max Capacity Reached");
            //GetComponent<BoxCollider>().enabled = false;
            OnTroughFull?.Invoke();
            chickenTimer.SetActive(true);
            feedCounter.SetActive(false);


        }
        else
        {

            if (other.transform.childCount == 0)
            {
                IsLoading = false;
            }
            else if (other.transform.childCount > 0)
            {
                if (n <= other.transform.childCount)
                {

                    if (other.transform.GetChild(other.transform.childCount - n).CompareTag("Crude"))
                    {
                        IsLoading = true;
                        Transform feedCell = other.transform.GetChild(other.transform.childCount - n);
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
                        n++;
                        IsLoading = false;

                    }
                }
                else
                {
                    n=1;
                    IsLoading = false;
                }
            }
        }

    }

}



