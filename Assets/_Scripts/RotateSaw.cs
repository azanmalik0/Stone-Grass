using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SWS.splineMove;

public class RotateSaw : MonoBehaviour
{
    public float rotationSpeed;
    private void OnEnable()
    {
    }
    void Update()
    {
        Rotate();
    }

    void Rotate()
    {
        rotationSpeed = RotationSetter.Instance.RotationSpeed;
        float rotationAmount = rotationSpeed * Time.deltaTime;
        transform.Rotate(new Vector3(0, 360, 0), rotationAmount);

    }




}
