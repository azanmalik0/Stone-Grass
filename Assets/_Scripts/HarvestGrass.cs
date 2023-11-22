using DG.Tweening;
using PT.Garden;
using System;
using UnityEngine;

public class HarvestGrass : MonoBehaviour
{
    public static event Action OnCropHarvest;
    [SerializeField] GameObject hayCellPrefab;
    [SerializeField] Vector3 jumpOffset;
    [SerializeField] int requiredGrass;
    public int grassCut;
    [SerializeField] GameObject[] hayCellPrefab1;
    private void OnEnable()
    {
        GrassPatchActivator.OnGrassCut += GenerateHay;
    }
    private void OnDisable()
    {

        GrassPatchActivator.OnGrassCut -= GenerateHay;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Blue"))
        {


            hayCellPrefab = hayCellPrefab1[0];
        }
        else if (other.CompareTag("Pink"))
        {


            hayCellPrefab = hayCellPrefab1[1];
        }
        else if (other.CompareTag("Green"))
        {

            hayCellPrefab = hayCellPrefab1[2];

        }
        else if (other.CompareTag("Autumn"))
        {

            hayCellPrefab = hayCellPrefab1[3];

        }
    }
    //private void CutGrass(Collider other)
    //{
    //    grassCut++;
    //    OnCropHarvest?.Invoke();
    //    other.gameObject.SetActive(false);
    //    if (grassCut >= requiredGrass)
    //    {
    //        grassCut = 0;
    //        GameObject hayCell = Instantiate(hayCellPrefab, other.transform.position, Quaternion.identity);
    //        hayCell.transform.DOJump(other.transform.position + jumpOffset, 3, 1, 1);
    //    }

    //}
    float delay = 1;
    void GenerateHay()
    {
        grassCut++;
        if (grassCut >= requiredGrass)
        {
            grassCut = 0;
            //OnCropHarvest?.Invoke();
            GameObject hayCell = Instantiate(hayCellPrefab, transform.position, Quaternion.identity);
            hayCell.transform.DOJump(transform.position, 2, 1, 1);
           
           

        }
      
    }


}
