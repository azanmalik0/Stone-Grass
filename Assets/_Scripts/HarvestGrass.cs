using DG.Tweening;
using PT.Garden;
using Sirenix.OdinInspector;
using System;
using System.ComponentModel;
using UnityEngine;

public class HarvestGrass : MonoBehaviour
{
    public static HarvestGrass instance;
    public static event Action OnCropHarvest;
    [SerializeField] GameObject hayCellPrefab;
    [SerializeField] Vector3 jumpOffset;
    [SerializeField] int requiredGrass;
    public int grassCut;

    private void Awake()
    {
        instance = this;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Corn"))
        {
            GenerateHay(other);
        }
        if (other.CompareTag("Wheat"))
        {
            GenerateHay(other);
        }
        if (other.CompareTag("Sunflower"))
        {
            GenerateHay(other);
        }
    }

    public void GenerateHay(Collider other)
    {
        other.gameObject.SetActive(false);
        OnCropHarvest?.Invoke();
        grassCut++;
        if (grassCut >= requiredGrass)
        {
            grassCut = 0;
            Instantiate(hayCellPrefab, other.transform.position, Quaternion.identity);

        }
    }


}
