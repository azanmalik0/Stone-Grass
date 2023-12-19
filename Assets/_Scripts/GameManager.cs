using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    GameState state;
    public static event Action<GameState> OnGameStateChanged;
    public GameObject tractorObject;
    public GameObject farmerObject;

    //===============================
    AudioManager AM;
    AdsManager adsManager;
    private void Awake()
    {
        Instance = this;
        Vibration.Init();
    }
    void Start()
    {
        AM = AudioManager.instance;
        adsManager = AdsManager.Instance;
        SetDefault();
        adsManager.ShowBannerAd();
        PlayBGM();
        Application.targetFrameRate = 60;
        UpdateGameState(GameState.Tractor);
        adsManager.LogEvent("game_launched");
    }
    void SetDefault()
    {
        if (!PlayerPrefs.HasKey("Vibration"))
        {
            PlayerPrefs.SetInt("Vibration", 1);
        }
        if (!PlayerPrefs.HasKey("Sound"))
        {
            PlayerPrefs.SetInt("Sound", 1);
        }
    }
    public void UpdateGameState(GameState NewState)
    {
        this.state = NewState;

        switch (NewState)
        {
            case GameState.Tractor:
                farmerObject.SetActive(false);
                tractorObject.transform.position = farmerObject.transform.position;
                tractorObject.transform.rotation = farmerObject.transform.rotation;
                tractorObject.SetActive(true);
                break;
            case GameState.Farmer:
                tractorObject.SetActive(false);
                farmerObject.transform.position = tractorObject.transform.position;
                farmerObject.transform.rotation = tractorObject.transform.rotation;
                farmerObject.SetActive(true);
                break;
        }
        OnGameStateChanged?.Invoke(NewState);
    }
    public void PlayBGM()
    {
        AM.Play("Ambient");
    }
#if UNITY_EDITOR
    private void OnApplicationQuit()
    {
        ES3AutoSaveMgr.Current.Save();
    }
#endif

}
public enum GameState { Tractor, Farmer, Upgrading, InGame, InShop, InFarmUpgrade, InLevelMenu, UnlockingArea, CuttingGrass, NotCuttingGrass, OnPlatform, OnGrassField }