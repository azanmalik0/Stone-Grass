using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SWS.splineMove;

public class RotateSaw : MonoBehaviour
{
    public static RotateSaw instance;
    public float rotationSpeed;
    private void Awake()
    {
        instance = this;
    }
    private void OnEnable()
    {
        TruckUpgradeManager.OnIncreasingRotationSpeed += IncreaseSpeed;
    }
    private void OnDisable()
    {
        TruckUpgradeManager.OnIncreasingRotationSpeed -= IncreaseSpeed;

    }
    void Update()
    {
        Rotate();
    }

    void Rotate()
    {
        float rotationAmount = rotationSpeed * Time.deltaTime;
        transform.Rotate(new Vector3(0, 360, 0), rotationAmount);

    }
    void IncreaseSpeed(int newSpeed)
    {
        rotationSpeed += newSpeed;

    }



}
