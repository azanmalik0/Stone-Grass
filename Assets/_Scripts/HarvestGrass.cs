using DG.Tweening;
using PT.Garden;
using System;
using UnityEngine;

public class HarvestGrass : MonoBehaviour
{
    [SerializeField] GameObject hayCellPrefab;
    [SerializeField] Vector3 jumpOffset;
    [SerializeField] int requiredGrass;
    [SerializeField] GameObject[] hayCellPrefab1;
    public int grassCut;
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

    void GenerateHay()
    {
        grassCut++;
        if (grassCut >= requiredGrass)
        {
            grassCut = 0;
            print("HayGenerate");
            GameObject hayCell = Instantiate(hayCellPrefab, transform.position, Quaternion.identity);
            hayCell.transform.DOJump(transform.position, 2, 1, 1);
        }
    }


}
