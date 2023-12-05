using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPreferencesSetter : MonoBehaviour
{
    public static LevelPreferencesSetter Instance;
    public int totalCrops;
    private void Awake()
    {
        Instance = this;
    }
}
