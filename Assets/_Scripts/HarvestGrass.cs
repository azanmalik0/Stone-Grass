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
    //================================================
    //[SerializeField] Vector3 jumpOffset;
    [SerializeField] int requiredGrass;
    [SerializeField] int grassCut;
    [Title("Particle References")]
    [SerializeField] ParticleSystem cornCollectParticle;
    [SerializeField] ParticleSystem sunCollectParticle;
    [Title("Hay Cell References")]
    [SerializeField] GameObject cornHayCellPrefab;
    [SerializeField] GameObject cornRedHayCellPrefab;
    [SerializeField] GameObject sunHayCellPrefab;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        cornCollectParticle=transform.GetChild(0).GetComponent<ParticleSystem>();
        sunCollectParticle = transform.GetChild(1).GetComponent<ParticleSystem>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Corn"))
        {
            cornCollectParticle.Play();
            GenerateHay(other, cornHayCellPrefab);
        }
        if (other.CompareTag("CornRed"))
        {
            cornCollectParticle.Play();
            GenerateHay(other, cornRedHayCellPrefab);
        }
        if (other.CompareTag("CornNight"))
        {
            cornCollectParticle.Play();
            GenerateHay(other, cornHayCellPrefab);
        }
        if (other.CompareTag("Sunflower"))
        {
            sunCollectParticle.Play();
            GenerateHay(other, sunHayCellPrefab);
        }
    }

    public void GenerateHay(Collider other, GameObject hayCell)
    {
        other.gameObject.SetActive(false);
        OnCropHarvest?.Invoke();
        grassCut++;
        if (grassCut >= requiredGrass)
        {
            grassCut = 0;
            Instantiate(hayCell, other.transform.position, Quaternion.identity);

        }
    }


}
