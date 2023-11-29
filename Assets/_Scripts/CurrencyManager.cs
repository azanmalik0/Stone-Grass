using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance;
    public static event Action OnCurrencyRecieve;
    [SerializeField] Text coinText;
    public static int coins;
    private void OnEnable()
    {
        FarmUpgradeManager.OnBuyingUpgrade += DeductCoins;
    }
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        //PlayerPrefs.SetInt("Coins",10000);
        coins = PlayerPrefs.GetInt("Coins");
        coinText.text = coins.ToString();
    }

    public void RecieveCoins(int value)
    {
        coins += value;
        PlayerPrefs.SetInt("Coins", coins);
        coinText.text = coins.ToString();
        OnCurrencyRecieve?.Invoke();

    }
    public static bool CheckRequiredCoins(int required)
    {
        if (coins >= required)
        {
            return true;
        }
        else
            return false;
    }

    public void DeductCoins(int value)
    {
        coins -= value;
        PlayerPrefs.SetInt("Coins", coins);
        //Debug.LogError("Coins after deduction = > " + PlayerPrefs.GetInt("Coins"));
        coinText.text = coins.ToString();

    }
    public static void UpdateAffordabilityStatus(Text text, int value)
    {
        if (!CheckRequiredCoins(value))
            text.color = Color.red;
        else
            text.color = Color.black;

    }
}
