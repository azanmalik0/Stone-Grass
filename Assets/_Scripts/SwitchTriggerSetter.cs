using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchTriggerSetter : MonoBehaviour
{
   [SerializeField] GameObject tractorTrigger;
   [SerializeField] GameObject farmerTrigger;

    private void OnEnable()
    {
        GameManager.OnGameStateChanged += SetTriggers;
    }
    private void OnDisable()
    {

        GameManager.OnGameStateChanged -= SetTriggers;
    }

    void SetTriggers(GameState state)
    {
        GameState CurrentState = state;
        if (CurrentState == GameState.Farmer)
        {
            farmerTrigger.SetActive(false);
            tractorTrigger.SetActive(true);
        }
        else if (CurrentState == GameState.Tractor)
        {
            farmerTrigger.SetActive(true);
            tractorTrigger.SetActive(false);

        }


    }

}
