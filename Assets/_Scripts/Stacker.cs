using DG.Tweening;
using Sirenix.OdinInspector;
using Sirenix.Reflection.Editor;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Stacker : MonoBehaviour
{
    [Title("General Preferences")]
    public int maxHayCapacity;
    [Title("Grid Preferences")]
    [SerializeField] protected int maxRows;
    [SerializeField] protected int maxColumns;
    [SerializeField] protected float cellWidth = 1.0f;
    [SerializeField] protected float cellHeight = 1.0f;
    [SerializeField] protected Vector3 gridOffset;
    [HideInInspector] public float InitialYOffset;
    public Vector3[,] cellPositions;
    protected int currentR = 0;
    protected int currentC = 0;


    private void Start()
    {
        CalculateCellPositions();
    }
    protected void SetGridYOffset(float initial)
    {
        InitialYOffset = initial;
    }
    protected virtual void RepositionStack(bool Reverse)
    {
        if (!Reverse)
        {

            gridOffset.y += 0.2f;
            currentC = 0;
            currentR = 0;
        }
        else
        {
            if (gridOffset.y > InitialYOffset)
                gridOffset.y -= 0.2f;
            else
                gridOffset.y -= InitialYOffset;
            currentC = maxColumns - 1;
            currentR = maxRows - 1;


        }
        CalculateCellPositions();

    }
    protected void CalculateCellPositions()
    {

        cellPositions = new Vector3[maxRows, maxColumns];
        Vector3 startPosition = transform.localPosition + gridOffset - new Vector3((maxColumns - 1) * cellWidth / 2, 0, (maxRows - 1) * cellHeight / 2);

        for (int row = 0; row < maxRows; row++)
        {
            for (int col = 0; col < maxColumns; col++)
            {
                cellPositions[row, col] = startPosition + new Vector3(col * cellWidth, 0, row * cellHeight);

            }
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
    public void UpdateGridPositions()
    {
        currentC++;
        if (currentC >= maxColumns)
        {
            currentC = 0;
            currentR++;

            if (currentR >= maxRows)
            {
                RepositionStack(false);
            }
        }
    }
    public void ResetGridPositions()
    {
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
    }
    public void RefreshGrid()
    {
        currentC = 0;
        currentR = 0;
        gridOffset.y = InitialYOffset;
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).DOLocalMove(cellPositions[currentC, currentR], 0.2f).SetEase(Ease.OutSine);
            UpdateGridPositions();


        }
    }


}
