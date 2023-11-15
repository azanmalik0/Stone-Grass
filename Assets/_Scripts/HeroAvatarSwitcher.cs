using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

public class HeroAvatarSwitcher : MonoBehaviour
{
    [SerializeField] ParticleSystem farmer_SwitchSmokeParticle;
    [SerializeField] ParticleSystem truck_SwitchSmokeParticle;
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Farmer"))
        {
            GameManager.Instance.UpdateGameState(GameState.Tractor);
            truck_SwitchSmokeParticle.Play();

        }
        else if (other.CompareTag("Tractor"))
        {
            GameManager.Instance.UpdateGameState(GameState.Farmer);
            farmer_SwitchSmokeParticle.Play();

        }
    }




}
