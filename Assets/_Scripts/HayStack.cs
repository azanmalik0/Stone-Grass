using DG.Tweening;
using PT.Garden;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HayStack : Stacker
{
    public static HayStack instance;

    public static event Action<int> OnSellingHarvest;
    public static event Action<int> OnHayCollect;

    //==============================================
    [Title("Unloading References")]
    [SerializeField] Transform unloadTarget;
    [SerializeField] ParticleSystem boilerParticle;
    float delay = 0;
    bool unloading;
    //==============================================

    int haySold;
    [HideInInspector]
    int hayCollected;
    public int HaySold { get => haySold; }
    public int HayCollected { get => hayCollected; }

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        CalculateCellPositions();
    }

    protected override void Load(Collider hay)
    {
        if (hayCollected >= maxHayCapacity)
        {
            //Debug.LogError("MaxCapacityReached");

        }
        else
        {
            hayCollected++;
            OnHayCollect?.Invoke(hayCollected);
            hay.transform.SetParent(this.transform);
            DOTween.Complete(hay.transform);
            hay.transform.DOLocalJump(cellPositions[currentR, currentC], 5, 1, 1f).SetEase(Ease.Linear);
            float randomAngle = UnityEngine.Random.Range(0, 360);
            hay.transform.DOLocalRotate(new Vector3(randomAngle, randomAngle, randomAngle), 1).SetEase(Ease.OutQuad).OnComplete(() => hay.transform.localRotation = Quaternion.identity);


            currentC++;
            if (currentC >= maxColumns)
            {
                currentC = 0;
                currentR++;

                if (currentR >= maxRows)
                {
                    //Debug.LogError("StackComplete");
                    RepositionStack(false);
                }
            }
        }

    }
    IEnumerator Unload()
    {

        while (unloading && transform.childCount > 0)
        {
            hayCollected--;
            haySold++;
            OnHayCollect?.Invoke(hayCollected);
            GameObject hayCell = transform.GetChild(transform.childCount - 1).gameObject;
            hayCell.GetComponent<BoxCollider>().enabled = false;
            hayCell.transform.SetParent(null);
            hayCell.transform.DOJump(unloadTarget.position, 2, 1, 0.5f).SetDelay(delay).SetEase(Ease.OutSine).OnComplete(() =>
            {
                Destroy(hayCell);
                boilerParticle.Play();
                OnSellingHarvest?.Invoke(haySold);
            });

            delay += 0.000001f;

            currentC--;

            if (currentC < 0)
            {
                currentC = maxColumns - 1;
                currentR--;

                if (currentR < 0)
                {
                    RepositionStack(true);
                }

            }

            yield return null;
        }
        //print("OUT");
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hay"))
        {
            Load(other);
        }
        if (other.CompareTag("Unload"))
        {
            unloading = true;
            //Debug.LogError("True");
            StartCoroutine(Unload());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Unload"))
        {
            unloading = false;
            //Debug.LogError("False");
        }

    }


}
