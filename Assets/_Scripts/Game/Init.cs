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

        if (PlayerPrefs.GetInt("PPStr") == 1)
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
        PlayerPrefs.SetInt("UserConsent", 1);

    }
    public void EnablePP()
    {
        PrivacyPolicyPanel.SetActive(true);
    }
    public void AcceptConsent()
    {
        IAgreePrivacyPolicy();
    }
}
