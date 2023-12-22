using UnityEngine;
using Sirenix.OdinInspector;

public class RotateSaw : MonoBehaviour
{
   [ReadOnly] public float rotationSpeed;

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
