using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

public class HarvestGrass : MonoBehaviour
{
    public static HarvestGrass instance;
    AudioManager AM;
    public static event Action OnCropHarvest;
    //================================================
    [SerializeField] Vector3 jumpOffset;
    [SerializeField] int requiredGrass;
    [SerializeField] int grassCut;
    [Title("Particle References")]
    [SerializeField] ParticleSystem cornCollectParticle;
    [SerializeField] ParticleSystem cornRedCollectParticle;
    [SerializeField] ParticleSystem cornYellowCollectParticle;
    [SerializeField] ParticleSystem cornNightCollectParticle;
    [SerializeField] ParticleSystem sunCollectParticle;
    [Title("Hay Cell References")]
    [SerializeField] GameObject cornHayCellPrefab;
    [SerializeField] GameObject cornRedHayCellPrefab;
    [SerializeField] GameObject cornYellowHayCellPrefab;
    [SerializeField] GameObject cornNightHayCellPrefab;
    [SerializeField] GameObject sunHayCellPrefab;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        AM = AudioManager.instance;
        cornCollectParticle = transform.GetChild(0).GetComponent<ParticleSystem>();
        sunCollectParticle = transform.GetChild(1).GetComponent<ParticleSystem>();
        cornRedCollectParticle = transform.GetChild(3).GetComponent<ParticleSystem>();
        cornNightCollectParticle = transform.GetChild(4).GetComponent<ParticleSystem>();
        cornYellowCollectParticle = transform.GetChild(5).GetComponent<ParticleSystem>();
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
            cornRedCollectParticle.Play();
            GenerateHay(other, cornRedHayCellPrefab);
        }
        if (other.CompareTag("CornNight"))
        {
            cornNightCollectParticle.Play();
            GenerateHay(other, cornNightHayCellPrefab);
        }
        if (other.CompareTag("CornYellow"))
        {
            cornYellowCollectParticle.Play();
            GenerateHay(other, cornYellowHayCellPrefab);
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
            GameObject Cell = Instantiate(hayCell, new Vector3(other.transform.position.x, -0.149f, other.transform.position.z), Quaternion.identity);
            if (!AM.IsPlaying("Pickup Hay"))
            {
                AM.Play("Pickup Hay");
                VibrationManager.SpecialVibrate(SpecialVibrationTypes.Pop);
            }
            float randomAngle = UnityEngine.Random.Range(0, 360);
            Cell.transform.DORotate(new Vector3(randomAngle, randomAngle, randomAngle), 1).SetEase(Ease.OutQuad);
            Cell.transform.DOJump(new Vector3(other.transform.position.x, -0.149f, other.transform.position.z) + jumpOffset, 5, 1, 1f).SetEase(Ease.Linear);

        }
    }


}
