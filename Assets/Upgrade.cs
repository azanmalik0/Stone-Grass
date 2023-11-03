using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade : MonoBehaviour
{
    public static event Action OnEnteringUpgradeZone;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("UpgradeZone"))
        {
            OnEnteringUpgradeZone?.Invoke();
        }
    }
}
