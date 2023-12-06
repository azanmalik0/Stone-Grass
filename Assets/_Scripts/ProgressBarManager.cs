using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarManager : MonoBehaviour
{
    public static event Action OnFirstStarUnlock;
    public static event Action OnSecondStarUnlock;
    public Slider progresSlider;
    public GameObject unlockedStar1;
    public GameObject unlockedStar2;
    public int totalCrops;
    public int crops;
    bool UnlockOnce1;
    bool UnlockOnce2;

    private void Start()
    {
        crops = PlayerPrefs.GetInt($"Crops{LevelMenuManager.Instance.currentLevel}");
        totalCrops = LevelPreferencesSetter.Instance.totalCrops;
        float progress = (float)crops / totalCrops;
        progresSlider.value = progress;
        CheckStarUnlock();
    }

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
        SaveSliderProgress();
        float progress = (float)crops / totalCrops;
        progresSlider.value = progress;
        CheckStarUnlock();
    }

    private void SaveSliderProgress()
    {
        PlayerPrefs.SetInt($"Crops{LevelMenuManager.Instance.currentLevel}", crops);
    }

    private void CheckStarUnlock()
    {
        if (progresSlider.value >= 0.5f)
        {
            if (!UnlockOnce1)
            {
                unlockedStar1.SetActive(true);
                OnFirstStarUnlock?.Invoke();
                UnlockOnce1 = true;

            }

        }
        if (progresSlider.value >= 1)
        {
            if (!UnlockOnce2)
            {
                unlockedStar2.SetActive(true);
                OnSecondStarUnlock?.Invoke();
                UnlockOnce2 = true;

            }

        }

    }
}
