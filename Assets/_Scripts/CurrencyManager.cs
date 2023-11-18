using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance;
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

    void RecieveCoins(int value)
    {
        coins = value;
        coinText.text = coins.ToString();

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
        // if (value > 0)
        // {
        coins -= value;
        coinText.text = coins.ToString();
        // }
    }
}
