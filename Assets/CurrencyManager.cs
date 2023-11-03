using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI coinText;
    int coins;

    private void OnEnable()
    {
        HayStack.OnSellingHarvest += RecieveCoins;
    }

    void RecieveCoins()
    {
        coins = HayStack.instance.HaySold;
        coinText.text = coins.ToString();

    }
}
