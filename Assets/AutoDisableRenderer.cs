using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDisableRenderer : MonoBehaviour
{
    private void OnEnable()
    {
        LevelInstantiator.OnBakingNavmesh += DisableRenderer;
    }

    void DisableRenderer()
    {
        GetComponent<MeshRenderer>().enabled = false;
    }
    private void OnDisable()
    {
        
        LevelInstantiator.OnBakingNavmesh -= DisableRenderer;
    }
}
