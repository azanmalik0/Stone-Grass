using DG.Tweening;
using PT.Garden;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HayStack : SerializedMonoBehaviour
{
    public static HayStack instance;

    public static event Action<int> OnSellingHarvest;
    public static event Action<int> OnHayCollect;
    [Title("Grid Preferences")]
    public int maxRows;
    public int maxColumns;
    public float cellWidth = 1.0f;
    public float cellHeight = 1.0f;
    public Vector3 gridOffset = Vector3.zero;
    //==============================================
    [Title("Unloading References")]
    [SerializeField] Transform unloadTarget;
    [SerializeField] ParticleSystem boilerParticle;
    float delay = 0;
    bool unloading;
    //==============================================
    [Title("Stack Preferences")]
    [TableMatrix]
    public Vector3[,] cellPositions;

    int currentR = 0;
    int currentC = 0;
    int haySold;
    public int hayCollected;
    public int maxHayCapacity;
    public int HaySold { get => haySold; }

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        CalculateCellPositions();

    }
    private void CalculateCellPositions()
    {

        cellPositions = new Vector3[maxRows, maxColumns];
        Vector3 startPosition = transform.localPosition + gridOffset - new Vector3((maxColumns - 1) * cellWidth / 2, 0, (maxRows - 1) * cellHeight / 2);

        for (int row = 0; row < maxRows; row++)
        {
            for (int col = 0; col < maxColumns; col++)
            {
                cellPositions[row, col] = startPosition + new Vector3(col * cellWidth, 0, row * cellHeight);
                print(cellPositions[row, col]);
                print(row + " " + col);
            }
        }

    }
    void StackHay(Collider hay)
    {
        if (hayCollected >= maxHayCapacity)
        {
            Debug.LogError("MaxCapacityReached");

        }
        else
        {
            hayCollected++;
            OnHayCollect?.Invoke(hayCollected);

            hay.transform.SetParent(this.transform);

            DOTween.Complete(hay.transform);
            hay.transform.DOLocalJump(cellPositions[currentR, currentC], 5, 1, 1f).SetEase(Ease.Linear).OnComplete(() => Debug.LogError("Completed"));
            //print(cellPositions[2, 2]);
            hay.transform.localRotation = Quaternion.identity;

            currentC++;
            if (currentC >= maxColumns)
            {
                currentC = 0;
                currentR++;

                if (currentR >= maxRows)
                {
                    Debug.LogError("StackComplete");
                    RepositionStack(false);
                }
            }
        }

    }
    private void RepositionStack(bool Reverse)
    {
        if (!Reverse)
        {

            gridOffset.y += 0.2f;
            currentC = 0;
            currentR = 0;
        }
        else
        {
            gridOffset.y -= 0.2f;
            currentC = maxColumns - 1;
            currentR = maxRows - 1;

        }
        CalculateCellPositions();

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
            hayCell.transform.DOMove(unloadTarget.position, 0.5f).SetDelay(delay).SetEase(Ease.Linear).OnComplete(() =>
                    {
                        Destroy(hayCell);
                        boilerParticle.Play();
                        OnSellingHarvest?.Invoke(haySold);
                    }
                    );
            delay += 0.0001f;

            currentC--;

            if (currentC < 0)
            {
                currentC = maxColumns - 1;
                currentR--;

                if (currentR < 0)
                {
                    Debug.LogError("Niche");
                    RepositionStack(true);
                }

            }

            yield return null;
        }
        if (transform.childCount <= 0)
        {
            currentC = 0;
            currentR = 0;
        }

        print("OUT");
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hay"))
        {
            StackHay(other);
        }
        if (other.CompareTag("Unload"))
        {
            unloading = true;
            Debug.LogError("True");
            StartCoroutine(Unload());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Unload"))
        {
            unloading = false;
            Debug.LogError("False");
        }

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Matrix4x4 oldGizmosMatrix = Gizmos.matrix;
        Gizmos.matrix = transform.localToWorldMatrix;

        Vector3 startPosition = gridOffset - new Vector3((maxColumns - 1) * cellWidth / 2, 0, (maxRows - 1) * cellHeight / 2);

        for (int row = 0; row < maxRows; row++)
        {
            for (int col = 0; col < maxColumns; col++)
            {
                Vector3 position = startPosition + new Vector3(col * cellWidth, 0, row * cellHeight);
                Gizmos.DrawWireCube(position, new Vector3(cellWidth, 0.1f, cellHeight));
            }
        }

        Gizmos.matrix = oldGizmosMatrix;
    }
}
