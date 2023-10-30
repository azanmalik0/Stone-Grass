using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] Vector3 offset;
    GameManager GM;

    private void Start()
    {
        GM = GameManager.Instance;
    }

    void LateUpdate()
    {
        if (GM.GetState() == GameState.Tractor)
            FollowPlayer(GM.tractorObject.transform);
        if (GM.GetState() == GameState.Farmer)
            FollowPlayer(GM.farmerObject.transform);
    }

    private void FollowPlayer(Transform target)
    {
        transform.position = target.position + offset;
    }
}

