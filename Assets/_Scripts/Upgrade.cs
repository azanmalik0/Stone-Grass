using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("UpgradeZone"))
        {
            GameManager.Instance.UpdateGameState(GameState.Upgrading);
        }
    }
}
