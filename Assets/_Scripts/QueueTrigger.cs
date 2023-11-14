using SWS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueTrigger : MonoBehaviour
{
    [SerializeField] Animator animator;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Buyer"))
        {
            GetComponentInParent<splineMove>().Pause();
            animator.SetBool("IsWalking", false);
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Buyer"))
        {
            GetComponentInParent<splineMove>().Resume();
            animator.SetBool("IsWalking", true);
        }

    }
}
