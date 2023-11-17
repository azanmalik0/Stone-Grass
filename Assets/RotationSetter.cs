using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationSetter : MonoBehaviour
{
    public static RotationSetter Instance;
   // [SerializeField] RotateSaw[] rotateSawScripts;
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
        RotationSpeed = value;
        //for (int i = 0; i < rotateSawScripts.Length; i++)
        //{
        //    rotateSawScripts[i].rotationSpeed = RotationSpeed;


        //}

    }
}
