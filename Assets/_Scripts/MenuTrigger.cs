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
        else if (other.CompareTag("Shop Zone"))
        {
            GameManager.Instance.UpdateGameState(GameState.InShop);
        }
        else if (other.CompareTag("FarmUpgradeZone") && this.CompareTag("Farmer_Stack"))
        {
            GameManager.Instance.UpdateGameState(GameState.InFarmUpgrade);
        }
        else if (other.CompareTag("LevelZone") && this.CompareTag("Farmer_Stack"))
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
        else if (other.CompareTag("FarmUnlock") && this.CompareTag("Farmer_Stack"))
        {
            OnEnteringLockedZone?.Invoke(LockedAreas.Farm);
        }
        else if (other.CompareTag("BarnUnlock") && this.CompareTag("Farmer_Stack"))
        {
            OnEnteringLockedZone?.Invoke(LockedAreas.Barn);
        }
        else if (other.CompareTag("MarketUnlock") && this.CompareTag("Farmer_Stack"))
        {
            OnEnteringLockedZone?.Invoke(LockedAreas.Market);
        }
    }
}
public enum LockedAreas { Farm, Henhouse, Barn, Market }
