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
        HayStack.OnSellingHarvest += RecieveCoins;
        FarmUpgradeManager.OnBuyingUpgrade += DeductCoins;
    }
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        coinText.text = coins.ToString();
    }

    void RecieveCoins(int value)
    {
        coins = value;
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
