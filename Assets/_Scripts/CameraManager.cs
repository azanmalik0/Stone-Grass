using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraManager : MonoBehaviour
{
    [SerializeField] Transform followTarget;
    [SerializeField] Vector3 offset;
    GameManager GM;

    private void Start()
    {
        GM = GameManager.Instance;
    }

    private void OnEnable()
    {
        GameManager.OnGameStateChanged += SetTarget;
    }
    private void OnDisable()
    {
        GameManager.OnGameStateChanged -= SetTarget;

    }
    private void LateUpdate()
    {
        FollowPlayer(followTarget);
    }
    void SetTarget(GameState state)
    {
        GameState currentState = state;
        if (currentState == GameState.Tractor)
            followTarget = GM.tractorObject.transform;
        if (currentState == GameState.Farmer)
            followTarget = GM.farmerObject.transform;
    }
    private void FollowPlayer(Transform target)
    {
        transform.position = target.position + offset;
    }
}

