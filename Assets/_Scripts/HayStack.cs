using DG.Tweening;
using PT.Garden;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HayStack : Stacker
{
    public static HayStack instance;

    public static event Action<int> OnSellingHarvest;
    public static event Action<int> OnHayCollect;
    public static event Action OnReachingHayloft;

    //==============================================
    [Title("Unloading References")]
    [SerializeField] Transform unloadTarget;
    [SerializeField] ParticleSystem boilerParticle;
    [SerializeField] GameObject hayCellPrefab;
    [SerializeField] GameObject PathDrawObject;

    float delay = 0;
    bool unloading;
    //==============================================
    public float initialYOffset;
    int haySold;
    public int hayCollected;
    public int hayCollectedTmp;
    bool IsFull;
    public int HaySold { get => haySold; }
    public int HayCollected { get => hayCollected; }
    //==============================================
    [Title("Destinations")]

    [SerializeField] Transform hayloft;
    [SerializeField] Transform truckUpgrade;


    private void Awake()
    {
        SetGridYOffset(gridOffset.y);
        instance = this;
    }
    private void OnEnable()
    {
        TruckUpgradeManager.OnIncreasingCarCapacity += UpdateMaxCarCapacity;
    }
    private void OnDisable()
    {
        TruckUpgradeManager.OnIncreasingCarCapacity -= UpdateMaxCarCapacity;

    }
    private void Start()
    {

        ES3AutoSaveMgr.Current.Load();
        LoadHayCollected();
        CalculateCellPositions();
        UpdateMaxCarCapacity();
        ChangeHayColor();
    }
    private void LoadHayCollected()
    {
        if (hayCollected > 0)
        {

            for (int i = 0; i < hayCollected; i++)
            {
                GameObject cell = Instantiate(hayCellPrefab, this.transform);
                cell.transform.localPosition = previousPositions[i];

            }
        }
    }
    private void UpdateMaxCarCapacity()
    {
        maxHayCapacity = TruckUpgradeManager.Instance.maxCarCapacity;
        CapacityBar.Instance.UpdateMaxCapacityUI();
    }
    private void Update()
    {
        CheckCapacityFull();
    }

    void LoadOnTractor(Collider hay)
    {
        if (transform.childCount >= maxHayCapacity)
        {
            
        }
        else
        {
            hayCollected++;
            OnHayCollect?.Invoke(hayCollected);
            hay.transform.SetParent(this.transform);
            DOTween.Complete(hay.transform);
            hay.transform.DOLocalJump(cellPositions[currentR, currentC], 5, 1, 1f).SetEase(Ease.Linear);
            previousPositions.Add(cellPositions[currentR, currentC]);
            float randomAngle = UnityEngine.Random.Range(0, 360);
            hay.transform.DOLocalRotate(new Vector3(randomAngle, randomAngle, randomAngle), 1).SetEase(Ease.OutQuad).OnComplete(() => hay.transform.localRotation = Quaternion.identity);
            UpdateGridPositions();


        }

    }
    void CheckCapacityFull()
    {
        if (transform.childCount >= maxHayCapacity)
        {
            if (!IsFull)
            {
                print("Here1");
                IsFull = true;
                PathDrawObject.SetActive(true);
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).GetComponent<MeshRenderer>().material.DOColor(Color.red, 0.5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);

                }
            }

        }
        else
        {
            if (IsFull)
            {
                print("Here2");
                IsFull = false;
                PathDrawObject.SetActive(false);
                for (int i = 0; i < transform.childCount; i++)
                {
                    DOTween.Kill(transform.GetChild(i).GetComponent<MeshRenderer>().material);

                }
            }

        }

    }
    private void ChangeHayColor()
    {
        if (!IsFull)
        {
            IsFull = true;
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).GetComponent<MeshRenderer>().material.DOColor(Color.red, 0.5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);

            }
        }
    }
    IEnumerator UnloadFromTruck()
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
            //if ((previousPositions.Count - 1) > 0)
            previousPositions.RemoveAt(previousPositions.Count - 1);

            delay += 0.000001f;
            ResetGridPositions();
            yield return null;

        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hay"))
        {
            LoadOnTractor(other);
        }
        if (other.CompareTag("Unload"))
        {
            unloading = true;
            StartCoroutine(UnloadFromTruck());
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
    private void OnApplicationQuit()
    {
        ES3AutoSaveMgr.Current.Save();
    }


}
