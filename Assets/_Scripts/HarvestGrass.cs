using DG.Tweening;
using PT.Garden;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class HarvestGrass : MonoBehaviour
{
    public static event Action OnCropHarvest;
    [SerializeField] GameObject hayCellPrefab;
    [SerializeField] Vector3 jumpOffset;
    [SerializeField] int requiredGrass;
    public int grassCut;
    private void OnEnable()
    {
        GrassPatchActivator.OnGrassCut += GenerateHay;
    }
    private void OnDisable()
    {

        GrassPatchActivator.OnGrassCut -= GenerateHay;
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Wheat"))
    //    {
    //        CutGrass(other);

    //    }
    //}
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
