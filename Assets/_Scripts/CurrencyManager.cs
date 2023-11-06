using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance;
    [SerializeField] TextMeshProUGUI coinText;
    int coins;

    public int Coins
    {
        get
        {
            return coins;
        }
        set
        {
            coins = value;
        }
    }

    private void OnEnable()
    {
        HayStack.OnSellingHarvest += RecieveCoins;
    }
    private void Awake()
    {
        Instance = this;
    }

    void RecieveCoins(int value)
    {
        coinText.text = value.ToString();

    }
}
