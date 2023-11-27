using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarManager : MonoBehaviour
{
    //public static event Action OnFirstStarUnlock;
  //  public static event Action OnSecondStarUnlock;
    public Slider progresSlider;
    public GameObject unlockedStar1;
    public GameObject unlockedStar2;
    public int totalCrops;
    int crops;
    bool UnlockOnce;


    private void OnEnable()
    {
        //HarvestGrass.OnCropHarvest += UpdateProgressBar;
    }
    private void OnDisable()
    {
        //HarvestGrass.OnCropHarvest -= UpdateProgressBar;

    }
    void UpdateProgressBar()
    {
        crops++;
        float progress = (float)crops / totalCrops;
        progresSlider.value = progress;
        CheckStarUnlock();
    }

    private void CheckStarUnlock()
    {
        if (progresSlider.value >= 0.5f)
        {
            if (!UnlockOnce)
            {
                unlockedStar1.SetActive(true);
              //  OnFirstStarUnlock?.Invoke();
                UnlockOnce = true;

            }

        }
        if (progresSlider.value >= 1)
        {
            if (!UnlockOnce)
            {
                unlockedStar2.SetActive(true);
             //   OnSecondStarUnlock?.Invoke();
                UnlockOnce = true;

            }

        }

    }
}
