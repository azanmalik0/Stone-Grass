using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarManager : MonoBehaviour
{
    public Slider progresSlider;
    public GameObject unlockedStar1;
    public GameObject unlockedStar2;
    public int totalCrops;
    int crops;


    private void OnEnable()
    {
        HarvestGrass.OnCropHarvest += UpdateProgressBar;
    }
    private void OnDisable()
    {
        HarvestGrass.OnCropHarvest -= UpdateProgressBar;

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
            unlockedStar1.SetActive(true);

        }
        if (progresSlider.value >= 1)
        {
            unlockedStar2.SetActive(true);

        }

    }
}
