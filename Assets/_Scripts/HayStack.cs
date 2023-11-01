using DG.Tweening;
using PT.Garden;
using Sirenix.OdinInspector;
using Sirenix.Reflection.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HayStack : MonoBehaviour
{
    [Title("Grid Preferences")]
    public int maxRows;
    public int maxColumns;
    public float cellWidth = 1.0f;
    public float cellHeight = 1.0f;
    public Vector3 gridOffset = Vector3.zero;
    //==============================================
    public Vector3[,] cellPositions;
    int currentR = 0;
    int currentC = 0;

    private void Awake()
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
            }
        }

    }
    void StackHay(Collider hay)
    {
        hay.transform.SetParent(this.transform);

        int row = currentR;
        int col = currentC;

        hay.transform.DOLocalJump(cellPositions[row, col], 5, 1, 1f).SetEase(Ease.Linear);
        hay.transform.localRotation = Quaternion.identity;

        currentC++;
        if (currentC >= maxColumns)
        {
            currentC = 0;
            currentR++;

            if (currentR >= maxRows)
            {
                RepositionStack();
            }
        }




    }
    private void RepositionStack()
    {
        gridOffset.y += 0.2f;
        CalculateCellPositions();
        currentC = 0;
        currentR = 0;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hay"))
        {
            StackHay(other);
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
