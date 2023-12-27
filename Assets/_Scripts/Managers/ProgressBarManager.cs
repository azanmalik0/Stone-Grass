using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarManager : MonoBehaviour
{
    public static event Action<int> OnFirstStarUnlock;
    public static event Action OnSecondStarUnlock;
    public static event Action<int> OnThirdStarUnlock;
    //=================================================
    [Title("Star UI Objects")]
    public GameObject unlockedStar1;
    public GameObject unlockedStar2;
    public GameObject unlockedStar3;
    [Title("Particle References")]
    [SerializeField] ParticleSystem star1Particle;
    [SerializeField] ParticleSystem star2Particle;
    [SerializeField] ParticleSystem star3Particle;
    [Title("General")]
    public Slider progresSlider;
    public int totalCrops;
    public int crops;
    AudioManager AM;
    //===================
    bool UnlockOnce1;
    bool UnlockOnce2;
    bool UnlockOnce3;
    private void Start()
    {
        AM = AudioManager.instance;
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
                AM.Play("LevelUnlocked");
                star1Particle.Play();
                OnFirstStarUnlock?.Invoke(100);
                UnlockOnce1 = true;

            }

        }
        if (progresSlider.value >= 0.7f)
        {
            if (!UnlockOnce2)
            {
                unlockedStar2.SetActive(true);
                AM.Play("LevelUnlocked");
                star2Particle.Play();
                OnSecondStarUnlock?.Invoke();
                UnlockOnce2 = true;

            }

        }
        if (progresSlider.value >= 1)
        {
            if (!UnlockOnce3)
            {
                unlockedStar3.SetActive(true);
                AM.Play("LevelUnlocked");
                star3Particle.Play();
                OnThirdStarUnlock?.Invoke(300);
                //Debug.LogError("ThirdStarInvoke===>");
                UnlockOnce3 = true;

            }

        }

    }
}
