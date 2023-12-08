using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gizmo : MonoBehaviour
{
    public Color color;
    void OnDrawGizmos()
    {
        Gizmos.color = color;
        Gizmos.DrawCube(transform.position, new Vector3(4, 4, 4));
      
    }
}
