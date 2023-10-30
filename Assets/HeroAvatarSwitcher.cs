using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroAvatarSwitcher : MonoBehaviour
{
    GameManager GM;

    private void Awake()
    {
        GM = GameManager.Instance;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameState CurrentState = GameManager.Instance.GetState();

            if (CurrentState == GameState.Tractor)
            {
                GM.UpdateGameState(GameState.Farmer);
            }
            else if (CurrentState == GameState.Farmer)
            {
                GM.UpdateGameState(GameState.Tractor);
            }

        }
    }
}
