using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuTrigger : MonoBehaviour
{
    public static event Action<LockedAreas> OnEnteringLockedZone;
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
        if (other.CompareTag("FarmUpgradeZone") && this.CompareTag("Farmer_Stack"))
        {
            GameManager.Instance.UpdateGameState(GameState.InFarmUpgrade);
        }
        if (other.CompareTag("LevelZone") && this.CompareTag("Farmer_Stack"))
        {
            GameManager.Instance.UpdateGameState(GameState.InLevelMenu);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("HenhouseUnlock") && this.CompareTag("Farmer_Stack"))
        {
            OnEnteringLockedZone?.Invoke(LockedAreas.Henhouse);
        }
        if (other.CompareTag("FarmUnlock") && this.CompareTag("Farmer_Stack"))
        {
            OnEnteringLockedZone?.Invoke(LockedAreas.Farm);
        }
        if (other.CompareTag("BarnUnlock") && this.CompareTag("Farmer_Stack"))
        {
            OnEnteringLockedZone?.Invoke(LockedAreas.Barn);
        }
        if (other.CompareTag("MarketUnlock") && this.CompareTag("Farmer_Stack"))
        {
            OnEnteringLockedZone?.Invoke(LockedAreas.Market);
        }

    }
}
public enum LockedAreas { Farm, Henhouse, Barn, Market }
