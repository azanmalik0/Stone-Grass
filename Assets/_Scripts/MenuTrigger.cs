using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("UpgradeZone"))
        {
            GameManager.Instance.UpdateGameState(GameState.Upgrading);
        }
        if (other.CompareTag("Shop Zone"))
        {
            GameManager.Instance.UpdateGameState(GameState.InShop);
        }
        if (other.CompareTag("FarmUpgradeZone") && this.CompareTag("Farmer_Stack") )
        {
            GameManager.Instance.UpdateGameState(GameState.InFarmUpgrade);
        }
        if (other.CompareTag("LevelZone") && this.CompareTag("Farmer_Stack"))
        {
            GameManager.Instance.UpdateGameState(GameState.InLevelMenu);
        }
    }
}
