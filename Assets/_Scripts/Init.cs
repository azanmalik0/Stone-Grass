using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Init : MonoBehaviour
{
    AdsManager adsManager;
    void Start()
    {
        adsManager=AdsManager.Instance;
        adsManager.RequestBannerAd();
        adsManager.RequestNonVideoInterstitialAd();
        adsManager.RequestRewardedAd();
        adsManager.ShowBannerAd();
        
    }
}
