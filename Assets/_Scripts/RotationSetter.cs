using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationSetter : MonoBehaviour
{
    public static RotationSetter Instance;
    public int RotationSpeed;

    private void Awake()
    {
        Instance = this;
    }
    private void OnEnable()
    {
        TruckUpgradeManager.OnIncreasingRotationSpeed += SetRotationSpeed;

    }
    private void OnDisable()
    {

        TruckUpgradeManager.OnIncreasingRotationSpeed -= SetRotationSpeed;
    }
    void SetRotationSpeed(int value)
    {
        RotationSpeed += value;
    }
}
