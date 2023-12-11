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
    AudioManager AM;

    public static event Action OnSellingHarvest;
    public static event Action<int> OnHayCollect;

    //==============================================
    [Title("Unloading References")]
    [SerializeField] Transform unloadTarget;
    [SerializeField] ParticleSystem boilerParticle;
    [SerializeField] GameObject PathDrawObject;

    //==============================================
    [Title("Hay Cell Prefabs")]
    [SerializeField] GameObject cornHayCellPrefab;
    [SerializeField] GameObject cornRedHayCellPrefab;
    [SerializeField] GameObject cornNightHayCellPrefab;
    [SerializeField] GameObject wheatHayCellPrefab;
    [SerializeField] GameObject sunHayCellPrefab;
    [Title("Hay Cell Materials")]
    [SerializeField] Material cornHayMaterial;
    [SerializeField] Material cornRedHayMaterial;
    [SerializeField] Material cornNightHayMaterial;
    [SerializeField] Material sunHayMaterial;
    [SerializeField] Material wheatHayMaterial;


    public int sunHayCollected;
    public int cornHayCollected;
    public int cornRedHayCollected;
    public int cornNightHayCollected;
    public int wheatHayCollected;
    public int totalHayCollected;

    //=================================================
    public int haySold;
    int index = 0;
    float delay = 0;
    bool IsFull;
    bool unloading;
    private void Awake()
    {
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
        AM = AudioManager.instance;
        SetGridYOffset(0.15f);
        if (totalHayCollected > 0)
        {
            LoadCornHayCollected();
        }
        CalculateCellPositions();
        UpdateMaxCarCapacity();
        CheckCapacityFull();
    }

    void LoadSunHayCollected()
    {
        if (sunHayCollected > 0)
        {
            for (int i = 0; i < sunHayCollected; i++)
            {
                GameObject cell = Instantiate(sunHayCellPrefab, this.transform);
                cell.transform.localPosition = previousPositions[index];
                index++;

            }
        }
        else
        {
            index = 0;
        }
    }
    //void LoadWheatHayCollected()
    //{
    //    if (wheatHayCollected > 0)
    //    {
    //        for (int i = 0; i < wheatHayCollected; i++)
    //        {
    //            GameObject cell = Instantiate(wheatHayCellPrefab, this.transform);
    //            cell.transform.localPosition = previousPositions[index];
    //            index++;
    //            if (i == wheatHayCollected - 1)
    //            {
    //                LoadSunHayCollected();
    //            }

    //        }
    //    }
    //    else
    //    {
    //        LoadSunHayCollected();
    //    }
    //}
    void LoadCornRedHayCollected()
    {
        if (cornRedHayCollected > 0)
        {
            for (int i = 0; i < cornRedHayCollected; i++)
            {
                GameObject cell = Instantiate(cornRedHayCellPrefab, this.transform);
                cell.transform.localPosition = previousPositions[index];
                index++;
                if (i == cornRedHayCollected - 1)
                {
                    LoadSunHayCollected();
                }
            }
        }
        else
        {
            LoadSunHayCollected();
        }
    }
    void LoadCornNightHayCollected()
    {
        if (cornNightHayCollected > 0)
        {
            for (int i = 0; i < cornNightHayCollected; i++)
            {
                GameObject cell = Instantiate(cornNightHayCellPrefab, this.transform);
                cell.transform.localPosition = previousPositions[index];
                index++;
                if (i == cornNightHayCollected - 1)
                {
                    LoadCornRedHayCollected();
                }
            }
        }
        else
        {
            LoadCornRedHayCollected();
        }
    }
    void LoadCornHayCollected()
    {
        if (cornHayCollected > 0)
        {
            for (int i = 0; i < cornHayCollected; i++)
            {
                GameObject cell = Instantiate(cornHayCellPrefab, this.transform);
                cell.transform.localPosition = previousPositions[index];
                index++;
                if (i == cornHayCollected - 1)
                {
                    LoadCornNightHayCollected();
                }
            }
        }
        else
        {
            LoadCornNightHayCollected();
        }
    }
    void UpdateMaxCarCapacity()
    {
        maxHayCapacity = TruckUpgradeManager.Instance.maxCarCapacity;
        CapacityBar.Instance._slider.maxValue = maxHayCapacity;
        CapacityBar.Instance.UpdateMaxCapacityUI();
    }
    void LoadOnTractor(Collider hay)
    {
        if (transform.childCount >= maxHayCapacity)
        {
            CheckCapacityFull();
        }
        else
        {
            totalHayCollected++;
            if (!AM.IsPlaying("Pickup Hay"))
                AM.Play("Pickup Hay");
            OnHayCollect?.Invoke(totalHayCollected);
            hay.transform.SetParent(this.transform);
            CheckHayType(hay.gameObject, true);
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
                IsFull = true;
                PathDrawObject.SetActive(true);
                //for (int i = 0; i < transform.childCount; i++)
                //{
                // transform.GetChild(0).GetComponent<MeshRenderer>().material.DOColor(Color.red, 0.5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
                cornHayMaterial.DOColor(Color.red, 0.5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
                wheatHayMaterial.DOColor(Color.red, 0.5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
                sunHayMaterial.DOColor(Color.red, 0.5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
                //}
            }

        }
        else
        {
            if (IsFull)
            {
                IsFull = false;
                PathDrawObject.SetActive(false);
                //for (int i = 0; i < transform.childCount; i++)
                // {
                //DOTween.Rewind(transform.GetChild(0).GetComponent<MeshRenderer>().material);
                DOTween.Rewind(cornHayMaterial);
                DOTween.Rewind(wheatHayMaterial);
                DOTween.Rewind(sunHayMaterial);

                //}


            }

        }

    }
    IEnumerator UnloadFromTruck()
    {
        while (unloading && transform.childCount > 0)
        {
            if (!AudioManager.instance.IsPlaying("SellHay"))
            {
                AudioManager.instance.Play("SellHay");
                VibrationManager.Vibrate();

            }
            AudioManager.instance.Play("GoldSack");
            totalHayCollected--;
            haySold++;
            GameObject hayCell = transform.GetChild(transform.childCount - 1).gameObject;
            CheckHayType(hayCell, false);
            hayCell.GetComponent<BoxCollider>().enabled = false;
            hayCell.transform.SetParent(null);
            hayCell.transform.DOJump(unloadTarget.position, 2, 1, 0.5f).SetDelay(delay).SetEase(Ease.OutSine).OnComplete(() =>
            {
                OnHayCollect?.Invoke(totalHayCollected);
                Destroy(hayCell);
                boilerParticle.Play();
                CurrencyManager.Instance.RecieveCoins(1);
                OnSellingHarvest?.Invoke();

            });
            if ((previousPositions.Count - 1) > 0)
                previousPositions.RemoveAt(previousPositions.Count - 1);
            delay += 0.000001f;
            ResetGridPositions();
            CheckCapacityFull();
            yield return null;

        }

    }
    private void CheckHayType(GameObject hayCell, bool Increment)
    {
        if (hayCell.CompareTag("HayCorn"))
        {
            if (Increment)
            {
                cornHayCollected++;
            }
            else
            {
                cornHayCollected--;
            }
        }
        if (hayCell.CompareTag("HayCornRed"))
        {
            if (Increment)
            {
                cornRedHayCollected++;
            }
            else
            {
                cornRedHayCollected--;
            }
        }
        if (hayCell.CompareTag("HayCornNight"))
        {
            if (Increment)
            {
                cornNightHayCollected++;
            }
            else
            {
                cornNightHayCollected--;
            }
        }
        else if (hayCell.CompareTag("HayWheat"))
        {
            if (Increment)
            {
                wheatHayCollected++;
            }
            else
            {
                wheatHayCollected--;
            }

        }
        else if (hayCell.CompareTag("HaySun"))
        {
            if (Increment)
            {
                sunHayCollected++;
            }
            else
            {
                sunHayCollected--;
            }

        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HayCorn"))
        {
            LoadOnTractor(other);
        }
        else if (other.CompareTag("HayCornRed"))
        {
            LoadOnTractor(other);
        }
        else if (other.CompareTag("HayCornNight"))
        {
            LoadOnTractor(other);
        }
        else if (other.CompareTag("HayWheat"))
        {
            LoadOnTractor(other);
        }
        else if (other.CompareTag("HaySun"))
        {
            LoadOnTractor(other);
        }
        else if (other.CompareTag("Unload"))
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
        }

    }

}
