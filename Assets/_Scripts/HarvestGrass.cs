using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestGrass : MonoBehaviour
{
    public static event Action OnCropHarvest;
    [SerializeField] GameObject hayCellPrefab;
    [SerializeField] Vector3 jumpOffset;
    [SerializeField] int requiredGrass;
    public int grassCut;
    public Transform TTmp;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wheat"))
        {
            CutGrass(other);

        }
    }
    private void CutGrass(Collider other)
    {
        grassCut++;
        OnCropHarvest?.Invoke();
        other.gameObject.SetActive(false);
        if (grassCut >= requiredGrass)
        {
            grassCut = 0;
            GameObject hayCell = Instantiate(hayCellPrefab, other.transform.position, Quaternion.identity);
            hayCell.transform.DOJump(other.transform.position + jumpOffset, 3, 1, 1);
        }

    }
}
