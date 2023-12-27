using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class Init : MonoBehaviour
{
    AdsManager adsManager;
    public GameObject PrivacyPolicyPanel;

    public LoadScene loadingPanel;
    
    
    private void Start()
    {
        adsManager = AdsManager.Instance;
        if (PlayerPrefs.GetInt("PPStr") == 0)
        {
            PrivacyPolicyPanel.SetActive(true);

        }
        else
        {
            PrivacyPolicyPanel.SetActive(false);
            loadingPanel.enabled = true;
            adsManager.RequestBannerAd();
            adsManager.RequestNonVideoInterstitialAd();
            adsManager.RequestRewardedAd();
            adsManager.ShowBannerAd();

        }
    }
    public void PrivacyPolicy()
    {
        //  AudioManager.instance.Play("Pop");
        Application.OpenURL("https://sites.google.com/view/biza-studio/home");
    }

    public void IAgreePrivacyPolicy()
    {

        PrivacyPolicyPanel.SetActive(false);
        PlayerPrefs.SetInt("PPStr", 1);
        loadingPanel.enabled = true;
        
        adsManager.RequestBannerAd();
        adsManager.RequestNonVideoInterstitialAd();
        adsManager.RequestRewardedAd();
        adsManager.ShowBannerAd();

        //  StartLoading = true;
        //if (!istOpen)
        //{
        //    Resources.UnloadUnusedAssets();
        //    GC.Collect();
        //}

    }
}
