using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSetParent : MonoBehaviour
{
    [ContextMenu("MakeGlobal")]
    public void MakeGlobal()
    {
        transform.SetParent(null);
    }
}
