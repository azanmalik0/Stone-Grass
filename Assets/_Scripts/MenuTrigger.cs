using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuTrigger : MonoBehaviour
{
    [SerializeField] Collider truckUpgradeZoneCollider;
    [SerializeField] Collider shopZoneCollider;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("UpgradeZone"))
        {
            GameManager.Instance.UpdateGameState(GameState.Upgrading);
            shopZoneCollider.enabled = false;
        }
        else if (other.CompareTag("Shop Zone"))
        {
            GameManager.Instance.UpdateGameState(GameState.InShop);
            truckUpgradeZoneCollider.enabled = false;
        }
        else if (other.CompareTag("FarmUpgradeZone") && this.CompareTag("Farmer_Stack"))
        {
            GameManager.Instance.UpdateGameState(GameState.InFarmUpgrade);
        }
        else if (other.CompareTag("LevelZone") && this.CompareTag("Farmer_Stack"))
        {
            GameManager.Instance.UpdateGameState(GameState.InLevelMenu);
        }
        else if (other.CompareTag("GrassField"))
        {
            GameManager.Instance.UpdateGameState(GameState.OnGrassField);
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("UpgradeZone"))
        {
            shopZoneCollider.enabled = true;
        }
        else if (other.CompareTag("Shop Zone"))
        {
            truckUpgradeZoneCollider.enabled = true;
        }
        else if (other.CompareTag("GrassField"))
        {
            GameManager.Instance.UpdateGameState(GameState.OnPlatform);
        }

    }



}

