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
    protected Vector3[,] cellPositions;
    public int currentR = 0;
    public int currentC = 0;



    protected virtual void Load(Collider hay) { }
    protected virtual void Load(Transform hay) { }
    protected virtual void CalculateCellPositions()
    {

        cellPositions = new Vector3[maxRows, maxColumns];
        Vector3 startPosition = transform.localPosition + gridOffset - new Vector3((maxColumns - 1) * cellWidth / 2, 0, (maxRows - 1) * cellHeight / 2);

        for (int row = 0; row < maxRows; row++)
        {
            for (int col = 0; col < maxColumns; col++)
            {
                cellPositions[row, col] = startPosition + new Vector3(col * cellWidth, 0, row * cellHeight);
                print(cellPositions[row, col] + gameObject.name);

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
