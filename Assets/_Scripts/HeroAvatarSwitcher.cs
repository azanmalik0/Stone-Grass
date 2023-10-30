using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

public class HeroAvatarSwitcher : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Farmer"))
        {
            GameManager.Instance.UpdateGameState(GameState.Tractor);

        }
        else if (other.CompareTag("Tractor"))
        {
            GameManager.Instance.UpdateGameState(GameState.Farmer);

        }
    }




}
