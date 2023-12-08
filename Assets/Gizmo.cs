using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gizmo : MonoBehaviour
{
    public Color color;
    void OnDrawGizmos()
    {
        // Draw a semitransparent red cube at the transforms position
        Gizmos.color = color;
        Gizmos.DrawCube(transform.position, new Vector3(4, 4, 4));
      
    }
}
