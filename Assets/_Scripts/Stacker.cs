using Sirenix.OdinInspector;
using Sirenix.Reflection.Editor;
using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] protected Vector3 gridOffset = Vector3.zero;
    public Vector3[,] cellPositions;

    protected int currentR = 0;

    protected int currentC = 0;

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
            gridOffset.y -= 0.2f;
            currentC = maxColumns - 1;
            currentR = maxRows - 1;

        }
        CalculateCellPositions();

    }
    protected virtual void Load(Collider hay) { }
    protected virtual void Load(Transform hay) { }
    protected virtual IEnumerator LoadDelay(Collider hay) { yield return null; }
    protected virtual IEnumerator UnloadDelay(Collider hay) { yield return null; }
    protected virtual void CalculateCellPositions()
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

}
