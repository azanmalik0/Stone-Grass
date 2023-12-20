using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [Title("Navmesh Path Draw References")]
    [SerializeField] GameObject HayFullPathDraw;
    [SerializeField] public GameObject UpgradePathDraw;
    public List<GameObject> UnloadingHaycells = new();

    //==============================================
    [Title("Hay Cell Prefabs")]
    [SerializeField] GameObject cornHayCellPrefab;
    [SerializeField] GameObject cornRedHayCellPrefab;
    [SerializeField] GameObject cornNightHayCellPrefab;
    [SerializeField] GameObject cornYellowHayCellPrefab;
    [SerializeField] GameObject wheatHayCellPrefab;
    [SerializeField] GameObject sunHayCellPrefab;
    [Title("HayCell Materials")]
    [SerializeField] Material[] hayCellMaterials;
    public int sunHayCollected;
    public int cornHayCollected;
    public int cornRedHayCollected;
    public int cornNightHayCollected;
    public int cornYellowHayCollected;
    public int wheatHayCollected;
    public int totalHayCollected;

    //=================================================
    public int haySold;
    int index = 0;
    int n = 0;
    public float delay = 0;
    bool IsFull;
    bool unloading;
    bool FirstTimeUnloading;
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
        OnHayCollect?.Invoke(totalHayCollected);
        CheckCapacityFull();
    }
    #region LoadingData
    void LoadSunHayCollected()
    {
        if (sunHayCollected > 0)
        {
            for (int i = 0; i < sunHayCollected; i++)
            {
                GameObject cell = Instantiate(sunHayCellPrefab, this.transform);
                cell.GetComponent<BoxCollider>().enabled = false;
                cell.transform.localPosition = previousPositions[index];
                index++;
                if (i == sunHayCollected - 1)
                {
                    CheckCapacityFull();
                }

            }
        }
        else
        {
            index = 0;
        }
    }
    void LoadCornRedHayCollected()
    {
        if (cornRedHayCollected > 0)
        {
            for (int i = 0; i < cornRedHayCollected; i++)
            {
                GameObject cell = Instantiate(cornRedHayCellPrefab, this.transform);
                cell.GetComponent<BoxCollider>().enabled = false;
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
                cell.GetComponent<BoxCollider>().enabled = false;
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
    void LoadCornYellowHayCollected()
    {
        if (cornYellowHayCollected > 0)
        {
            for (int i = 0; i < cornYellowHayCollected; i++)
            {
                GameObject cell = Instantiate(cornYellowHayCellPrefab, this.transform);
                cell.GetComponent<BoxCollider>().enabled = false;
                cell.transform.localPosition = previousPositions[index];
                index++;
                if (i == cornYellowHayCollected - 1)
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
    void LoadCornHayCollected()
    {
        if (cornHayCollected > 0)
        {
            for (int i = 0; i < cornHayCollected; i++)
            {
                GameObject cell = Instantiate(cornHayCellPrefab, this.transform);
                cell.GetComponent<BoxCollider>().enabled = false;
                cell.transform.localPosition = previousPositions[index];
                index++;
                if (i == cornHayCollected - 1)
                {
                    LoadCornYellowHayCollected();
                }
            }
        }
        else
        {
            LoadCornYellowHayCollected();
        }
    }
    #endregion
    void UpdateMaxCarCapacity()
    {
        maxHayCapacity = TruckUpgradeManager.Instance.maxCarCapacity;
        CapacityBar.Instance._slider.maxValue = maxHayCapacity;
        CapacityBar.Instance.UpdateMaxCapacityUI();
    }
    void LoadOnTractor(Collider hay)
    {
        if (totalHayCollected >= maxHayCapacity)
        {
            CheckCapacityFull();
        }
        else
        {
            totalHayCollected++;
            CheckCapacityFull();
            //if (!AM.IsPlaying("Pickup Hay"))
            //{
            //    AM.Play("Pickup Hay");
            //    VibrationManager.SpecialVibrate(SpecialVibrationTypes.Pop);
            //}
            OnHayCollect?.Invoke(totalHayCollected);
            hay.transform.SetParent(this.transform);
            hay.GetComponent<BoxCollider>().enabled = false;
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
        if (totalHayCollected >= maxHayCapacity)
        {
            if (!IsFull)
            {
                IsFull = true;
                UpgradePathDraw.SetActive(false);
                HayFullPathDraw.SetActive(true);
                for (int i = 0; i < hayCellMaterials.Length; i++)
                    hayCellMaterials[i].DOColor(Color.red, 0.5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);


            }

        }
        else
        {
            if (IsFull)
            {
                IsFull = false;
                HayFullPathDraw.SetActive(false);
                RevertMaterialColour();

            }

        }

    }
    IEnumerator UnloadFromTruck()
    {
        while (unloading && totalHayCollected > 0)
        {
            if (!FirstTimeUnloading)
                FirstTimeUnloading = true;

            if (!AudioManager.instance.IsPlaying("SellHay"))
            {
                AudioManager.instance.Play("SellHay");
                VibrationManager.Vibrate();

            }
            AudioManager.instance.Play("GoldSack");
            //============================================
            totalHayCollected--;
            haySold++;
            GameObject hayCell = transform.GetChild(transform.childCount - 1).gameObject;
            UnloadingHaycells.Add(hayCell);
            CheckHayType(hayCell, false);
            hayCell.transform.SetParent(null);
            hayCell.transform.DOJump(unloadTarget.position, 2, 1, 0.5f).SetDelay(delay).SetEase(Ease.Linear).OnComplete(() =>
            {
                Destroy(UnloadingHaycells[n]);
                //UnloadingHaycells[n].SetActive(false);
                n++;
                boilerParticle.Play();
            });
            delay += 0.000001f;
            OnHayCollect?.Invoke(totalHayCollected);
            CurrencyManager.Instance.RecieveCoins(1);
            OnSellingHarvest?.Invoke();
            if ((previousPositions.Count - 1) >= 0)
                previousPositions.RemoveAt(previousPositions.Count - 1);
            ResetGridPositions();
            RevertMaterialColour();
            CheckCapacityFull();
            yield return null;

        }

        delay = 0;
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
        else if (hayCell.CompareTag("HayCornYellow"))
        {
            if (Increment)
            {
                cornYellowHayCollected++;
            }
            else
            {
                cornYellowHayCollected--;
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
        else if (other.CompareTag("HayCornYellow"))
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
            //UnloadFromTruck();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Unload"))
        {
            if (PlayerPrefs.GetInt("FirstTimeUnloading") == 0 && (LevelMenuManager.Instance.currentLevel == 0) && FirstTimeUnloading)
            {
                //Debug.LogError("UpgradePath");
                UpgradePathDraw.SetActive(true);

            }
            unloading = false;
        }

    }
    private void OnApplicationQuit()
    {
        print("OnApplicationQuit" + gameObject.name);
        RevertMaterialColour();
    }
    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            print("OnApplicationPause" + gameObject.name);
            RevertMaterialColour();
        }
    }
    private void OnDestroy()
    {
        print("OnDestroy" + gameObject.name);
        RevertMaterialColour();
    }
    public void RevertMaterialColour()
    {
        for (int i = 0; i < hayCellMaterials.Length; i++)
            DOTween.Rewind(hayCellMaterials[i]);
    }
}
