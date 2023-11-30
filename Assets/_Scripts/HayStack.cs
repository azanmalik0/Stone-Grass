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

    //==============================================
    [Title("Unloading References")]
    [SerializeField] Transform unloadTarget;
    [SerializeField] ParticleSystem boilerParticle;
    [SerializeField] GameObject PathDrawObject;

    //==============================================
    [Title("HayCellPrefabs")]
    [SerializeField] GameObject greenHayCellPrefab;
    [SerializeField] GameObject autumnHayCellPrefab;
    [SerializeField] GameObject pinkHayCellPrefab;
    [SerializeField] GameObject blueHayCellPrefab;
    [SerializeField] GameObject redHayCellPrefab;
    [SerializeField] GameObject purpleHayCellPrefab;
    [SerializeField] GameObject orangeHayCellPrefab;

    public int hayAutumnCollected;
    public int hayPinkCollected;
    public int hayBlueCollected;
    public int hayRedCollected;
    public int hayPurpleCollected;
    public int hayOrangeCollected;
    public int hayGreenCollected;
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
        SetGridYOffset(0.15f);
        if (totalHayCollected > 0)
        {
            LoadGreenHayCollected();
        }
        CalculateCellPositions();
        UpdateMaxCarCapacity();
    }

    void LoadOrangeHayCollected()
    {
        if (hayOrangeCollected > 0)
        {
            for (int i = 0; i < hayOrangeCollected; i++)
            {
                GameObject cell = Instantiate(orangeHayCellPrefab, this.transform);
                cell.transform.localPosition = previousPositions[index];
                index++;

            }
        }
        else
        {
            index = 0;
        }
    }
    void LoadPurpleHayCollected()
    {
        if (hayPurpleCollected > 0)
        {
            for (int i = 0; i < hayPurpleCollected; i++)
            {
                GameObject cell = Instantiate(purpleHayCellPrefab, this.transform);
                cell.transform.localPosition = previousPositions[index];
                index++;
                if (i == hayPurpleCollected - 1)
                {
                    LoadOrangeHayCollected();
                }

            }
        }
        else
        {
            LoadOrangeHayCollected();
        }
    }
    void LoadRedHayCollected()
    {
        if (hayRedCollected > 0)
        {

            for (int i = 0; i < hayRedCollected; i++)
            {
                GameObject cell = Instantiate(redHayCellPrefab, this.transform);
                cell.transform.localPosition = previousPositions[index];
                index++;
                if (i == hayRedCollected - 1)
                {
                    LoadPurpleHayCollected();
                }

            }
        }
        else
        {
            LoadPurpleHayCollected();
        }
    }
    void LoadBlueHayCollected()
    {
        if (hayBlueCollected > 0)
        {

            for (int i = 0; i < hayBlueCollected; i++)
            {
                GameObject cell = Instantiate(blueHayCellPrefab, this.transform);
                cell.transform.localPosition = previousPositions[index];
                index++;
                if (i == hayBlueCollected - 1)
                {
                    LoadRedHayCollected();
                }

            }
        }
        else
        {
            LoadRedHayCollected();
        }
    }
    void LoadPinkHayCollected()
    {
        if (hayPinkCollected > 0)
        {

            for (int i = 0; i < hayPinkCollected; i++)
            {
                GameObject cell = Instantiate(pinkHayCellPrefab, this.transform);
                cell.transform.localPosition = previousPositions[index];
                index++;
                if (i == hayPinkCollected - 1)
                {
                    LoadBlueHayCollected();
                }

            }
        }
        else
        {
            LoadBlueHayCollected();
        }
    }
    void LoadAutumnHayCollected()
    {
        if (hayAutumnCollected > 0)
        {
            for (int i = 0; i < hayAutumnCollected; i++)
            {
                GameObject cell = Instantiate(autumnHayCellPrefab, this.transform);
                cell.transform.localPosition = previousPositions[index];
                index++;
                if (i == hayAutumnCollected - 1)
                {
                    LoadPinkHayCollected();
                }

            }
        }
        else
        {
            LoadPinkHayCollected();
        }
    }
    void LoadGreenHayCollected()
    {
        if (hayGreenCollected > 0)
        {
            for (int i = 0; i < hayGreenCollected; i++)
            {
                GameObject cell = Instantiate(greenHayCellPrefab, this.transform);
                cell.transform.localPosition = previousPositions[index];
                index++;
                if (i == hayGreenCollected - 1)
                {
                    LoadAutumnHayCollected();
                }
            }
        }
        else
        {
            LoadAutumnHayCollected();
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
            // CheckForHayCollectedIncrement(hayTypeCollected);
            totalHayCollected++;
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
                IsFull = false;
                PathDrawObject.SetActive(false);
                for (int i = 0; i < transform.childCount; i++)
                {
                    DOTween.Rewind(transform.GetChild(i).GetComponent<MeshRenderer>().material);

                }


            }

        }

    }
    IEnumerator UnloadFromTruck()
    {
        //print("UnloadTruck");
        while (unloading && transform.childCount > 0)
        {

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

            });
            if ((previousPositions.Count - 1) > 0)
                previousPositions.RemoveAt(previousPositions.Count - 1);
            delay += 0.000001f;
            ResetGridPositions();
            CheckCapacityFull();
            yield return null;

        }
        OnSellingHarvest?.Invoke(haySold);

    }
    private void CheckHayType(GameObject hayCell, bool Increment)
    {
        if (hayCell.CompareTag("HayGreen"))
        {
            if (Increment)
            {
                hayGreenCollected++;
            }
            else
            {
                hayGreenCollected--;
            }
        }
        else if (hayCell.CompareTag("HayAutumn"))
        {
            if (Increment)
            {
                hayAutumnCollected++;
            }
            else
            {
                hayAutumnCollected--;
            }

        }
        else if (hayCell.CompareTag("HayPink"))
        {
            if (Increment)
            {
                hayPinkCollected++;
            }
            else
            {
                hayPinkCollected--;
            }

        }
        else if (hayCell.CompareTag("HayBlue"))
        {
            if (Increment)
            {
                hayBlueCollected++;
            }
            else
            {
                hayBlueCollected--;
            }

        }
        else if (hayCell.CompareTag("HayRed"))
        {
            if (Increment)
            {
                hayRedCollected++;
            }
            else
            {
                hayRedCollected--;
            }

        }
        else if (hayCell.CompareTag("HayPurple"))
        {
            if (Increment)
            {
                hayPurpleCollected++;
            }
            else
            {
                hayPurpleCollected--;
            }

        }
        else if (hayCell.CompareTag("HayOrange"))
        {
            if (Increment)
            {
                hayOrangeCollected++;
            }
            else
            {
                hayOrangeCollected--;
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HayGreen"))
        {
            LoadOnTractor(other);
        }
        else if (other.CompareTag("HayAutumn"))
        {
            LoadOnTractor(other);
        }
        else if (other.CompareTag("HayPink"))
        {
            LoadOnTractor(other);
        }
        else if (other.CompareTag("HayBlue"))
        {
            LoadOnTractor(other);
        }
        else if (other.CompareTag("HayRed"))
        {
            LoadOnTractor(other);
        }
        else if (other.CompareTag("HayPurple"))
        {
            LoadOnTractor(other);
        }
        else if (other.CompareTag("HayOrange"))
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
