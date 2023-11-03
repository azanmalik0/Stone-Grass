using DG.Tweening;
using PT.Garden;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HayStack : MonoBehaviour
{
    public static HayStack instance;

    public static event Action OnSellingHarvest;
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
    [SerializeField] int maxStacks;
    int stacksAdded;
    bool maxStackReached;

    public Vector3[,] cellPositions;

    int currentR = 0;
    int currentC = 0;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        cellPositions = new Vector3[maxRows, maxColumns];
        CalculateCellPositions();

    }
    private void CalculateCellPositions()
    {

        Vector3 startPosition = transform.localPosition + gridOffset - new Vector3((maxColumns - 1) * cellWidth / 2, 0, (maxRows - 1) * cellHeight / 2);

        for (int row = 0; row < maxRows; row++)
        {
            for (int col = 0; col < maxColumns; col++)
            {
                cellPositions[row, col] = startPosition + new Vector3(col * cellWidth, 0, row * cellHeight);
            }
        }

    }
    void StackHay(Collider hay)
    {

        if (!maxStackReached)
        {

            hay.transform.SetParent(this.transform);

            int _row = currentR;
            int _col = currentC;
            DOTween.Complete(hay.transform);
            hay.transform.DOLocalJump(cellPositions[_row, _col], 5, 1, 1f).SetEase(Ease.Linear).OnComplete(() => Debug.LogError("Completed"));
            hay.transform.localRotation = Quaternion.identity;

            currentC++;
            if (currentC >= maxColumns)
            {
                currentC = 0;
                currentR++;

                if (currentR >= maxRows)
                {
                    Debug.LogError("StackComplete");
                    RepositionStack();
                }
            }
        }




    }
    private void RepositionStack()
    {
        stacksAdded++;
        if (stacksAdded >= maxStacks)
        {
            maxStackReached = true;
            Debug.LogError("Max Stacks Reached");
        }
        else
        {

            gridOffset.y += 0.2f;
            currentC = 0;
            currentR = 0;
            CalculateCellPositions();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hay"))
        {
            //boilerParticle.Stop();
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
            //StopCoroutine(Unload());
            Debug.LogError("False");
        }

    }
    int haySold;
    public int HaySold { get => haySold; }
    IEnumerator Unload()
    {

        while (unloading && transform.childCount > 0)
        {
            print("In");
            GameObject hayCell = transform.GetChild(transform.childCount - 1).gameObject;
            haySold++;
            hayCell.GetComponent<BoxCollider>().enabled = false;
            hayCell.transform.DOMove(unloadTarget.position, 0.5f).SetDelay(delay).SetEase(Ease.Linear).OnComplete(() =>
            {
                Destroy(hayCell);
                boilerParticle.Play();
                OnSellingHarvest?.Invoke();
            }
            );
            hayCell.transform.SetParent(null, true);
            delay += 0.0001f;
            yield return null;
        }
        print("OUT");
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
