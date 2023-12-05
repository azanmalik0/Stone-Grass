using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassAnimation : MonoBehaviour
{
    Quaternion originalRot;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Sunflower") || other.gameObject.CompareTag("Wheat") || other.gameObject.CompareTag("Corn"))
        {
            if (other.gameObject.activeInHierarchy)
            {

                originalRot = other.transform.rotation;
                other.transform.DOLookAt(transform.position,0.01f);
                other.transform.DOLocalRotate(new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z+15f), 0.01f).SetEase(Ease.Linear);
            }

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Sunflower") || other.gameObject.CompareTag("Wheat") || other.gameObject.CompareTag("Corn"))
        {

            if (other.gameObject.activeInHierarchy)
                other.transform.DOLocalRotate(new Vector3(originalRot.x,originalRot.y, originalRot.z), 0.5f).SetEase(Ease.OutBack);

        }
    }



}
