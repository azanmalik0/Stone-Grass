using DG.Tweening;
using PT.Garden;
using System;
using UnityEngine;

public class HarvestGrass : MonoBehaviour
{
    public static HarvestGrass instance;
    GameObject hayCellPrefab;
    [SerializeField] Vector3 jumpOffset;
    [SerializeField] int requiredGrass;
    [SerializeField] GameObject[] hayCellvariants;
    public int grassCut;

    private void Awake()
    {
        instance = this;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Blue"))
        {


            hayCellPrefab = hayCellvariants[0];
        }
        else if (other.CompareTag("Pink"))
        {


            hayCellPrefab = hayCellvariants[1];
        }
        else if (other.CompareTag("Green"))
        {

            hayCellPrefab = hayCellvariants[2];

        }
        else if (other.CompareTag("Autumn"))
        {

            hayCellPrefab = hayCellvariants[3];

        }
        else if (other.CompareTag("Purple"))
        {

            hayCellPrefab = hayCellvariants[4];

        }
        else if (other.CompareTag("Red"))
        {

            hayCellPrefab = hayCellvariants[5];

        }
        else if (other.CompareTag("Orange"))
        {

            hayCellPrefab = hayCellvariants[6];

        }
    }

    public void GenerateHay(Vector3 pos)
    {
        grassCut++;
        if (grassCut >= requiredGrass)
        {
            grassCut = 0;
            //print("HayGenerate" + gameObject.name);
            if (hayCellPrefab != null)
            {
                Instantiate(hayCellPrefab, pos, Quaternion.identity);
            }

        }
    }


}
