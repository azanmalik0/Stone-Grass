using System;
using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using UnityEngine;
using Firebase;
using Firebase.Analytics;
using Firebase.Extensions;
using System.Threading.Tasks;
using GoogleMobileAds.Ump.Api;
using DG.Tweening.Core.Easing;
//using BubbleShooterKit;
//using Google.Play.Review;



public class AdsManager : MonoBehaviour
{

    DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;

    public static AdsManager Instance;
    public bool isShowTestAds;
    public string AdmobAppId;
    public string bannerAdUnitId;
    public AdPosition bannerAdPosition;
    private BannerView _bannerView;
    [HideInInspector]
    public bool isBannerAdShown;

    //public string rectBannerAdUnitId;
    //public AdPosition rectBannerAdPosition;
    //private BannerView _rectBannerView;

    public string nonVideoInterstitialAdUnitId;
    private InterstitialAd _nonVideoInterstitialAd;

    public string videoInterstitialAdUnitId;
    private InterstitialAd _videoInterstitialAd;

    public string rewardedAdUnitId;
    public RewardedAd _rewardedAd;

    //public string rewardedInterstitialAdUnitId;
    //private RewardedInterstitialAd _rewardedInterstitialAd;

    public string appOpenAdUnitId;
    private AppOpenAd _appOpenAd;
    // Start is called before the first frame update
    private bool _isInitialized;

    public RectTransform rectTransform;

    [Header("Firebase Section")]
    [Space]
    public string FCM_testTopicId;
    public FloatValues[] _FloatValues;
    [Space]
    public StringValues[] _StringValues;
    [Space]
    public BoleanValues[] _BoleanValues;
    bool isFCMInitialized = false;
    public static System.DateTime m_StartTime;

    [HideInInspector]
    public bool sendPlayAdLogEvent, isLowEndDevice;

    //private ReviewManager _reviewManager;
    //private PlayReviewInfo _playReviewInfo;
    [HideInInspector]
    public string RewardString;
    public AdsManager()
    {
        m_StartTime = System.DateTime.Now;
    }

    public static float realtimeSinceStartup
    {
        get
        {
            var timeSpan = System.DateTime.Now.Subtract(m_StartTime);
            return (float)timeSpan.TotalSeconds;

        }
    }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

        }
        else
       if (Instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {

        

        if (isShowTestAds)
        {
            bannerAdUnitId = "ca-app-pub-3940256099942544/6300978111";
            //rectBannerAdUnitId = "ca-app-pub-3940256099942544/6300978111";
            nonVideoInterstitialAdUnitId = "ca-app-pub-3940256099942544/1033173712";
            videoInterstitialAdUnitId = "ca-app-pub-3940256099942544/1033173712";
            rewardedAdUnitId = "ca-app-pub-3940256099942544/5224354917";
            //rewardedInterstitialAdUnitId = "ca-app-pub-3940256099942544/5354046379";
            appOpenAdUnitId = "ca-app-pub-3940256099942544/3419835294";
        }


        MobileAds.RaiseAdEventsOnUnityMainThread = true;
        //These followings are for testing GDPR consent
        var debugSettings = new ConsentDebugSettings
        {
            // Geography appears as in EEA for debug devices.
            DebugGeography = DebugGeography.EEA,
            TestDeviceHashedIds = new List<string>
     {
          "8AF033739916D58267D650CBDC019A8C",
          "8AF033739916D58267D650CBDC019A8B",
          "C2CC467A27A0F512A5D4EF27D32E0B87",
          "BA2FB27B70746321DF40BE552FAF2718",
          "4CFEF353B4A2E5075F3CF7F0CFE95987",
          "5D1F8648993E6EFB6B6299814F9D2228"
     }
        };
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            // Here false means users are not under age of consent.
            ConsentRequestParameters request = new ConsentRequestParameters
            {
                TagForUnderAgeOfConsent = false,
                ConsentDebugSettings = debugSettings,
            };

            // Check the current consent information status.
            ConsentInformation.Update(request, OnConsentInfoUpdated);
        }
        else
        {
            InitializeAds();
        }

        

        try
        {
            FireBaseInitilization();
        }
        catch (Exception ex)
        {
            Debug.Log("Exception Generated" + ex);
        }
    }
    void OnConsentInfoUpdated(FormError consentError)
    {
        if (consentError != null)
        {
            // Handle the error.
            Debug.Log("Consent Error    " + consentError.Message);
            InitializeAds();
            return;
        }

        if (ConsentInformation.PrivacyOptionsRequirementStatus == PrivacyOptionsRequirementStatus.Required &&
            ConsentInformation.ConsentStatus != GoogleMobileAds.Ump.Api.ConsentStatus.Obtained)
        {
            Debug.Log("Obtaining Consent........");
            ConsentForm.LoadAndShowConsentFormIfRequired((FormError formError) =>
            {
                if (formError == null)
                {
                    PlayerPrefs.SetInt("UserConsent", 1);
                }
                InitializeAds();
            });
        }
        else
        {
            Debug.Log("Consent Not Required");
            InitializeAds();
        }
    }

    void InitializeAds()
    {

        MobileAds.Initialize((InitializationStatus initstatus) =>
        {
            if (initstatus == null)
            {
                return;
            }
            _isInitialized = true;
            RequestNonVideoInterstitialAd();
            if (!PlayerPrefs.HasKey("UserConsent"))
            {
                Debug.Log("UserConsent Required PP");

                FindObjectOfType<Init>().EnablePP();

            }
            else
            {
                FindObjectOfType<Init>().AcceptConsent();
            }
            RequestNonVideoInterstitialAd();
        });
    }



    ///**************************************************************************BannerAd*******************************************
    /// ****************************************************************************************************************************   
    #region BannerAd
    public void RequestBannerAd()
    {
        if (_bannerView != null)
        {
            _bannerView.Destroy();
            _bannerView = null;
        }

        _bannerView = new BannerView(bannerAdUnitId, AdSize.Banner, bannerAdPosition);
        ListenToBannerAdEvents();
        var adRequest = new AdRequest();
        _bannerView.LoadAd(adRequest);
    }

    public void ShowBannerAd()
    {
        if (_bannerView != null)
        {
            _bannerView.Show();
        }
    }
    public void HideBannerAd()
    {
        if (_bannerView != null)
        {
            _bannerView.Hide();
            //if (getRemoteValueBool(4))
            //{
            rectTransform.gameObject.SetActive(false);
            //}
        }
    }


    public void SetSize(RectTransform source, float width, float height)
    {
        if (width <= 100)
        {
            width += 5;
            height += 5;
        }
        else
        {
            width += 10;
            height += 10;
        }
        source.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        source.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
    }

    private void ListenToBannerAdEvents()
    {
        _bannerView.OnBannerAdLoaded += () =>
        {
            rectTransform.gameObject.SetActive(true);
            SetSize(rectTransform, _bannerView.GetWidthInPixels(), _bannerView.GetHeightInPixels());
        };
        _bannerView.OnBannerAdLoadFailed += (LoadAdError error) =>
        {

        };

        _bannerView.OnAdPaid += (AdValue adValue) =>
        {

        };

        _bannerView.OnAdImpressionRecorded += () =>
        {
            isBannerAdShown = true;
            //if (getRemoteValueBool(4))
            //{
            rectTransform.gameObject.SetActive(true);
            //}

        };
        _bannerView.OnAdClicked += () =>
        {
            //myAdsManager.instance.NotShowAppOpenAd = true;
        };
        _bannerView.OnAdFullScreenContentOpened += () =>
        {

        };
    }

    #endregion

    ///**************************************************************************RectBannerAd*******************************************
    /// ****************************************************************************************************************************

    #region RectBannerAd
    /*
    public void RequestRectBannerAd()
    {
        if (_rectBannerView != null)
        {
            _rectBannerView.Destroy();
            _rectBannerView = null;
        }

        _rectBannerView = new BannerView(rectBannerAdUnitId, AdSize.MediumRectangle, rectBannerAdPosition);       
        ListenToRectBannerAdEvents();
        var adRequest = new AdRequest();
        _rectBannerView.LoadAd(adRequest);
        _rectBannerView.Hide();
    }


    public void ShowRectBannerAd()
    {
        if (_rectBannerView != null)
        {
            _rectBannerView.Show();
        }
    }
    public void HideRectBannerAd()
    {
        if (_rectBannerView != null)
        {
            _rectBannerView.Hide();
        }
    }

    private void ListenToRectBannerAdEvents()
    {
        _rectBannerView.OnBannerAdLoaded += () =>
        {

        };
        _rectBannerView.OnBannerAdLoadFailed += (LoadAdError error) =>
        {

        };

        _rectBannerView.OnAdPaid += (AdValue adValue) =>
        {

        };

        _rectBannerView.OnAdImpressionRecorded += () =>
        {

        };

        _rectBannerView.OnAdFullScreenContentOpened += () =>
        {

        };
    }
    */
    #endregion


    ///**************************************************************************NonVideoInterstitialAd*******************************************
    /// ****************************************************************************************************************************

    #region NonVideoInterstitialAd
    public void RequestNonVideoInterstitialAd()
    {
        if (_nonVideoInterstitialAd != null)
        {
            _nonVideoInterstitialAd.Destroy();
            _nonVideoInterstitialAd = null;
        }

        var adRequest = new AdRequest();

        InterstitialAd.Load(nonVideoInterstitialAdUnitId, adRequest, (InterstitialAd ad, LoadAdError error) =>
        {
            if (error != null)
            {
                return;
            }

            if (ad == null)
            {
                return;
            }
            _nonVideoInterstitialAd = ad;
            RegisterNonVideoInterstitialEventHandlers(ad);
        });
    }

    public void ShowNonVideoInterstitialAd()
    {
        if (_nonVideoInterstitialAd != null && _nonVideoInterstitialAd.CanShowAd())
        {
            _nonVideoInterstitialAd.Show();
        }
        else
        {
            RequestNonVideoInterstitialAd();
        }
    }

    public bool isNonVideoInterstitialAdLoaded()
    {
        return _nonVideoInterstitialAd != null && _nonVideoInterstitialAd.CanShowAd();
    }

    private void RegisterNonVideoInterstitialEventHandlers(InterstitialAd ad)
    {
        ad.OnAdFullScreenContentOpened += () =>
        {

        };

        ad.OnAdFullScreenContentClosed += () =>
        {
            RequestNonVideoInterstitialAd();
            //LogEvent("play_ad_show_done");
        };

        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {

        };
    }

    #endregion

    ///**************************************************************************VideoInterstitialAd*******************************************
    /// ****************************************************************************************************************************

    #region VideoInterstitialAd
    public void RequestVideoInterstitialAd()
    {

        if (_videoInterstitialAd != null)
        {
            _videoInterstitialAd.Destroy();
            _videoInterstitialAd = null;
        }

        var adRequest = new AdRequest();

        InterstitialAd.Load(videoInterstitialAdUnitId, adRequest, (InterstitialAd ad, LoadAdError error) =>
        {
            if (error != null)
            {
                return;
            }

            if (ad == null)
            {
                return;
            }
            _videoInterstitialAd = ad;
            RegisterVideoInterstitialEventHandlers(ad);
        });
    }

    public void ShowVideoInterstitialAd()
    {

        if (_videoInterstitialAd != null && _videoInterstitialAd.CanShowAd())
        {
            _videoInterstitialAd.Show();
        }
        else
        {
            RequestVideoInterstitialAd();
        }
    }

    public bool isVideoInterstitialAdLoaded()
    {
        return _videoInterstitialAd != null && _videoInterstitialAd.CanShowAd();
    }

    private void RegisterVideoInterstitialEventHandlers(InterstitialAd ad)
    {
        ad.OnAdFullScreenContentOpened += () =>
        {

        };

        ad.OnAdFullScreenContentClosed += () =>
        {
            RequestVideoInterstitialAd();
        };

        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            // RequestVideoInterstitialAd();
        };
    }

    #endregion


    ///**************************************************************************Rewarded InterstitialAd*******************************************
    /// ****************************************************************************************************************************

    #region RewardedInterstitialAd
    /*
    public void RequestRewardedInterstitialAd()
    {

        if (_rewardedInterstitialAd != null)
        {
            _rewardedInterstitialAd.Destroy();
            _rewardedInterstitialAd = null;
        }

        var adRequest = new AdRequest();

        RewardedInterstitialAd.Load(rewardedInterstitialAdUnitId, adRequest, (RewardedInterstitialAd ad, LoadAdError error) =>
        {
            if (error != null)
            {
                return;
            }

            if (ad == null)
            {
                return;
            }

            _rewardedInterstitialAd = ad;

            RegisterRewardedInterstitialAdEventHandlers(ad);

        });
    }


    public void ShowRewardedInterstitialAd()
    {
        if (_rewardedInterstitialAd != null && _rewardedInterstitialAd.CanShowAd())
        {

            _rewardedInterstitialAd.Show((Reward reward) =>
            {
                Debug.Log(String.Format("Rewarded ad granted a reward: {0} {1}",
                                        reward.Amount,
                                        reward.Type));
            });
        }
        else
        {
            Debug.LogError("Rewarded ad is not ready yet.");
        }
    }

    public bool isRewardedInterstitialAdLoaded()
    {
        return _rewardedInterstitialAd != null && _rewardedInterstitialAd.CanShowAd();
    }

    private void RegisterRewardedInterstitialAdEventHandlers(RewardedInterstitialAd ad)
    {

        ad.OnAdFullScreenContentOpened += () =>
        {

        };

        ad.OnAdFullScreenContentClosed += () =>
        {
            RequestRewardedInterstitialAd();
        };

        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            RequestRewardedInterstitialAd();
        };
    }
    */
    #endregion

    ///**************************************************************************VideoInterstitialAd*******************************************
    /// ****************************************************************************************************************************

    #region RewardedAd

    public void RequestRewardedAd()
    {

        if (_rewardedAd != null)
        {
            _rewardedAd.Destroy();
            _rewardedAd = null;
        }

        var adRequest = new AdRequest();

        RewardedAd.Load(rewardedAdUnitId, adRequest, (RewardedAd ad, LoadAdError error) =>
        {
            if (error != null)
            {
                return;
            }

            if (ad == null)
            {
                return;
            }

            _rewardedAd = ad;

            RegisterRewardedAdEventHandlers(ad);

        });
    }


    public void ShowRewardedAd(string rewardUpgrade)
    {
        if (_rewardedAd != null && _rewardedAd.CanShowAd())
        {
            _rewardedAd.Show((Reward reward) =>
            {
                //Grant Reward Here
                GrantReward(rewardUpgrade);
            });
        }
        else
        {

        }
    }

    public void GrantReward(string rewardUpgrade)
    {
        if (rewardUpgrade == "AddChickens")
        {
            FarmUpgradeManager.Instance.AddChickens();
        }
        if (rewardUpgrade == "AddCows")
        {
            FarmUpgradeManager.Instance.AddCows();
        }
        if (rewardUpgrade == "ChickenTray")
        {
            FarmUpgradeManager.Instance.IncreaseChickenTrayCapacity(FarmUpgradeManager.Instance.incrementChickenTrayCapacity);
        }
        if (rewardUpgrade == "CowTray")
        {
            FarmUpgradeManager.Instance.IncreaseCowTrayCapacity(FarmUpgradeManager.Instance.incrementCowTrayCapacity);
        }
        if (rewardUpgrade == "IncreaseFarmerCapacity")
        {
            FarmUpgradeManager.Instance.IncreaseFarmerCapacity(FarmUpgradeManager.Instance.incrementFarmerCapacity);
        }
        if (rewardUpgrade == "IncreaseStorageCapacity")
        {
            FarmUpgradeManager.Instance.IncreaseStorageCapacity(FarmUpgradeManager.Instance.incrementStorageCapacity);
        }
        if (rewardUpgrade == "IncreaseRotationSpeed")
        {
            TruckUpgradeManager.Instance.IncreaseRotationSpeed(TruckUpgradeManager.Instance.incrementRotationSpeed);
        }
        if (rewardUpgrade == "AddWheels")
        {
            TruckUpgradeManager.Instance.AddWheels();
        }
        if (rewardUpgrade == "AddSawBlades")
        {
            TruckUpgradeManager.Instance.AddBlades();
        }
        if (rewardUpgrade == "IncreaseCarCapacity")
        {
            TruckUpgradeManager.Instance.IncreaseCarCapacity(TruckUpgradeManager.Instance.incrementCarCapacity);
        }


    }

    public bool isRewardedAdLoaded()
    {
        return _rewardedAd != null && _rewardedAd.CanShowAd();
    }

    private void RegisterRewardedAdEventHandlers(RewardedAd ad)
    {

        ad.OnAdFullScreenContentOpened += () =>
        {

        };

        ad.OnAdFullScreenContentClosed += () =>
        {
            RequestRewardedAd();
        };

        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            // RequestRewardedAd();
        };
    }
    #endregion



    ///**************************************************************************AppOpenAd*******************************************
    /// ****************************************************************************************************************************

    //#region AppOpenAD

    //private bool isShowingAd;
    //public void RequestAppOpenAd()
    //{
    //    if (_appOpenAd != null)
    //    {
    //        _appOpenAd.Destroy();
    //        _appOpenAd = null;
    //    }

    //    var adRequest = new AdRequest();
    //    AppOpenAd.Load(appOpenAdUnitId, ScreenOrientation.Landscape, adRequest,
    //        (AppOpenAd ad, LoadAdError error) =>
    //        {

    //            if (error != null || ad == null)
    //            {
    //                Debug.Log("app open ad failed to load an ad " +
    //                               "with error : " + error);
    //                return;
    //            }
    //            _appOpenAd = ad;
    //            RegisterAppOpenEventHandlers(ad);
    //        });
    //}

    //public bool IsAppOpenAdAvailable
    //{
    //    get
    //    {
    //        return _appOpenAd != null
    //               && _appOpenAd.CanShowAd();

    //    }
    //}

    //void OnApplicationPause(bool isPaused)
    //{
    //    if (!isPaused)
    //    {
    //        StartCoroutine(ShowAppOpenAd());
    //    }
    //}


    //public IEnumerator ShowAppOpenAd()
    //{
    //    yield return new WaitForSeconds(0.1f);
    //    if (!IsAppOpenAdAvailable || isShowingAd || myAdsManager.instance.NotShowAppOpenAd)
    //    {
    //        myAdsManager.instance.NotShowAppOpenAd = false;
    //        yield break;
    //    }
    //    if (_appOpenAd != null && _appOpenAd.CanShowAd())
    //    {
    //        Debug.Log("Showing app open ad.");
    //        isShowingAd = true;
    //        _appOpenAd.Show();

    //    }
    //    else
    //    {
    //        Debug.Log("App open ad is not ready yet.");
    //    }
    //}

    //private void RegisterAppOpenEventHandlers(AppOpenAd ad)
    //{     
    //    ad.OnAdFullScreenContentOpened += () =>
    //    {

    //    };

    //    ad.OnAdFullScreenContentClosed += () =>
    //    {
    //        isShowingAd = false;
    //        //RequestAppOpenAd();
    //    };

    //    ad.OnAdFullScreenContentFailed += (AdError error) =>
    //    {
    //        isShowingAd = false;
    //        //RequestAppOpenAd();
    //    };
    //}
    //#endregion


    #region firebase Initilization 

    private void FireBaseInitilization()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                var app = Firebase.FirebaseApp.DefaultInstance;


                //Dictionary<string, object> defaults = new Dictionary<string, object>();
                //if (!_isArrayNullOrEmpty(_FloatValues))
                //{
                //    foreach (var item in _FloatValues)
                //    {
                //        defaults.Add(item.Name, item.defaultValue);

                //    }
                //}

                //if (!_isArrayNullOrEmpty(_StringValues))
                //{
                //    foreach (var item in _StringValues)
                //    {
                //        defaults.Add(item.Name, item.defaultValue);
                //    }
                //}

                //if (!_isArrayNullOrEmpty(_BoleanValues))
                //{
                //    foreach (var item in _BoleanValues)
                //    {
                //        defaults.Add(item.Name, item.defaultValue);
                //    }
                //}


                //Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.SetDefaultsAsync(defaults);

                //InitializeFCM();
                //FetchDataAsync();

                // Set a flag here to indicate whether Firebase is ready to use by your app.
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.

            }
        });
    }

    /// <summary>
    ///trigger firebase(Analytics) custom log Event with single parameter
    public void LogEvent(string name)
    {
        try
        {
            FirebaseAnalytics.LogEvent(name);

        }
        catch (Exception ex)
        {

        }

    }


    #endregion


    //#region firebase RemoteConfig

    //public void DisplayAllKeys()
    //{

    //    System.Collections.Generic.IEnumerable<string> keys =
    //        Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.Keys;
    //    foreach (string key in keys)
    //    {

    //    }

    //    keys = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetKeysByPrefix("config_test_s");
    //    foreach (string key in keys)
    //    {

    //    }
    //}
    //private bool _isArrayNullOrEmpty(StringValues[] stringValues)
    //{
    //    return (stringValues == null || stringValues.Length == 0);
    //}

    //private bool _isArrayNullOrEmpty(FloatValues[] floatValues)
    //{
    //    return (floatValues == null || floatValues.Length == 0);
    //}
    //private bool _isArrayNullOrEmpty(BoleanValues[] boleanValues)
    //{
    //    return (boleanValues == null || boleanValues.Length == 0);
    //}

    //public Task FetchDataAsync()
    //{
    //    // DebugLog("Fetching data...");
    //    // FetchAsync only fetches new data if the current data is older than the provided
    //    // timespan.  Otherwise it assumes the data is "recent enough", and does nothing.
    //    // By default the timespan is 12 hours, and for production apps, this is a good
    //    // number.  For this example though, it's set to a timespan of zero, so that
    //    // changes in the console will always show up immediately.
    //    System.Threading.Tasks.Task fetchTask = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.FetchAsync(
    //        TimeSpan.Zero);
    //    return fetchTask.ContinueWith(FetchComplete);
    //}

    //void FetchComplete(Task fetchTask)
    //{
    //    get_FB_Data();

    //    if (fetchTask.IsCanceled)
    //    {

    //    }
    //    else if (fetchTask.IsFaulted)
    //    {

    //    }
    //    else if (fetchTask.IsCompleted)
    //    {



    //    }

    //    var info = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.Info;
    //    switch (info.LastFetchStatus)
    //    {
    //        case Firebase.RemoteConfig.LastFetchStatus.Success:
    //            Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.ActivateAsync();
    //            get_FB_Data();

    //            break;
    //        case Firebase.RemoteConfig.LastFetchStatus.Failure:
    //            switch (info.LastFetchFailureReason)
    //            {
    //                case Firebase.RemoteConfig.FetchFailureReason.Error:

    //                    break;
    //                case Firebase.RemoteConfig.FetchFailureReason.Throttled:

    //                    break;
    //            }
    //            break;
    //        case Firebase.RemoteConfig.LastFetchStatus.Pending:

    //            break;
    //    }
    //}


    //void get_FB_Data()
    //{
    //    if (dependencyStatus == DependencyStatus.Available)
    //    {
    //        try
    //        {
    //            if (!_isArrayNullOrEmpty(_FloatValues))
    //            {
    //                foreach (var item in _FloatValues)
    //                {
    //                    item.value = (float)Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue(item.Name).DoubleValue;
    //                }
    //            }


    //            if (!_isArrayNullOrEmpty(_StringValues))
    //            {
    //                foreach (var item in _StringValues)
    //                {

    //                    item.value = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue(item.Name).StringValue;
    //                }
    //            }

    //            if (!_isArrayNullOrEmpty(_BoleanValues))
    //            {
    //                foreach (var item in _BoleanValues)
    //                {
    //                    item.value = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue(item.Name).BooleanValue;
    //                }
    //            }


    //        }
    //        catch (Exception ex)
    //        {

    //        }



    //    }
    //}



    //#endregion

    //#region firebase Cloud Messaging

    //// Setup message event handlers.
    //void InitializeFCM()
    //{
    //    Firebase.Messaging.FirebaseMessaging.MessageReceived += OnMessageReceived;
    //    Firebase.Messaging.FirebaseMessaging.TokenReceived += OnTokenReceived;
    //    if (isShowTestAds)
    //    {
    //        Firebase.Messaging.FirebaseMessaging.SubscribeAsync(FCM_testTopicId).ContinueWithOnMainThread(task => {
    //            LogTaskCompletion(task, "SubscribeAsync");

    //        });
    //    }

    //    Debug.Log("Firebase Messaging Initialized");

    //    // This will display the prompt to request permission to receive
    //    // notifications if the prompt has not already been displayed before. (If
    //    // the user already responded to the prompt, thier decision is cached by
    //    // the OS and can be changed in the OS settings).
    //    //Firebase.Messaging.FirebaseMessaging.RequestPermissionAsync().ContinueWithOnMainThread(
    //    //  task =>
    //    //  {
    //    //      LogTaskCompletion(task, "RequestPermissionAsync");
    //    //  }
    //    //);
    //    isFCMInitialized = true;
    //}

    //public virtual void OnMessageReceived(object sender, Firebase.Messaging.MessageReceivedEventArgs e)
    //{
    //    Debug.Log("Received a new message");
    //    var notification = e.Message.Notification;
    //    if (notification != null)
    //    {
    //        Debug.Log("title: " + notification.Title);
    //        Debug.Log("body: " + notification.Body);
    //        var android = notification.Android;
    //        if (android != null)
    //        {
    //            Debug.Log("android channel_id: " + android.ChannelId);
    //        }
    //    }
    //    if (e.Message.From.Length > 0)
    //        Debug.Log("from: " + e.Message.From);
    //    if (e.Message.Link != null)
    //    {
    //        Debug.Log("link: " + e.Message.Link.ToString());
    //    }
    //    if (e.Message.Data.Count > 0)
    //    {
    //        Debug.Log("data:");
    //        foreach (System.Collections.Generic.KeyValuePair<string, string> iter in
    //                 e.Message.Data)
    //        {
    //            Debug.Log("  " + iter.Key + ": " + iter.Value);
    //        }
    //    }
    //}

    //public virtual void OnTokenReceived(object sender, Firebase.Messaging.TokenReceivedEventArgs token)
    //{
    //    Debug.Log("Received Registration Token: " + token.Token);
    //}

    //// Log the result of the specified task, returning true if the task
    //// completed successfully, false otherwise.
    //protected bool LogTaskCompletion(Task task, string operation)
    //{
    //    bool complete = false;
    //    if (task.IsCanceled)
    //    {
    //        Debug.Log(operation + " canceled.");
    //    }
    //    else if (task.IsFaulted)
    //    {
    //        Debug.Log(operation + " encounted an error.");
    //        foreach (Exception exception in task.Exception.Flatten().InnerExceptions)
    //        {
    //            string errorCode = "";
    //            Firebase.FirebaseException firebaseEx = exception as Firebase.FirebaseException;
    //            if (firebaseEx != null)
    //            {
    //                errorCode = String.Format("Error.{0}: ",
    //                  ((Firebase.Messaging.Error)firebaseEx.ErrorCode).ToString());
    //            }
    //            Debug.Log(errorCode + exception.ToString());
    //        }
    //    }
    //    else if (task.IsCompleted)
    //    {
    //        Debug.Log(operation + " completed");
    //        complete = true;
    //    }
    //    return complete;
    //}

    //// End our messaging session when the program exits.
    //public void OnDestroy()
    //{
    //    Firebase.Messaging.FirebaseMessaging.MessageReceived -= OnMessageReceived;
    //    Firebase.Messaging.FirebaseMessaging.TokenReceived -= OnTokenReceived;
    //}

    //public bool isFCMinitialized()
    //{
    //    return isFCMInitialized;
    //}

    //#endregion

    public bool getRemoteValueBool(int index)
    {
        // for testing purpose
        //if (index == 1)
        //{
        //    return true;
        //}
        // for testing purpose
        return _BoleanValues[index].value;
    }

    public float getRemoteValueFloat(int index)
    {
        // for testing purpose
        //if (index == 3)
        //{
        //   return 5; //1//2//3
        //}
        // for testing purpose
        return _FloatValues[index].value;
    }

    public string getRemoteValueString(int index)
    {

        return _StringValues[index].value;
    }

    //public void callInappReview()
    //{
    //    StartCoroutine(requestReviews());

    //}

    //IEnumerator requestReviews()
    //{
    //    //print("Review start");
    //    _reviewManager = new ReviewManager();

    //    //Request a ReviewInfo Object 
    //    var requestFlowOperation = _reviewManager.RequestReviewFlow();
    //    yield return requestFlowOperation;
    //    if (requestFlowOperation.Error != ReviewErrorCode.NoError)
    //    {
    //        // Log error. For example, using requestFlowOperation.Error.ToString().
    //        yield break;
    //    }
    //    _playReviewInfo = requestFlowOperation.GetResult();


    //    //Launch the InApp Review Flow
    //    var launchFlowOperation = _reviewManager.LaunchReviewFlow(_playReviewInfo);
    //    yield return launchFlowOperation;
    //    _playReviewInfo = null; // Reset the object
    //    if (launchFlowOperation.Error != ReviewErrorCode.NoError)
    //    {
    //        // Log error. For example, using requestFlowOperation.Error.ToString().
    //        yield break;
    //    }
    //    // The flow has finished. The API does not indicate whether the user
    //    // reviewed or not, or even whether the review dialog was shown. Thus, no
    //    // matter the result, we continue our app flow.
    //}
    public void SSTOOL(GameObject obj)
    {
        if (!SSTools.call)
        {
            DontDestroyOnLoad(obj);
            SSTools.call = true;
            StartCoroutine(Dis(obj));
        }
    }

    IEnumerator Dis(GameObject obj)
    {
        yield return new WaitForSecondsRealtime(1f);
        Destroy(obj);
        SSTools.call = false;
    }


}

[Serializable]
public class FloatValues
{
    public string Name;
    [HideInInspector]
    public float value;
    public float defaultValue;
}

[Serializable]
public class StringValues
{
    public string Name;
    [HideInInspector]
    public string value;
    public string defaultValue;
}

[Serializable]
public class BoleanValues
{
    public string Name;
    [HideInInspector]
    public bool value;
    public bool defaultValue;
}

