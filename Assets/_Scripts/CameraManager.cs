using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraManager : MonoBehaviour
{
    [SerializeField] Transform followTarget;
    [SerializeField] Vector3 upgradePosition;
    [SerializeField] Vector3 offset;
    bool IsUpgrading;
    GameManager GM;

    private void Start()
    {
        GM = GameManager.Instance;
    }

    private void OnEnable()
    {
        GameManager.OnGameStateChanged += SetTarget;
        GameManager.OnGameStateChanged += ChangeCameraToMenuPosition;
    }
    private void OnDisable()
    {
        GameManager.OnGameStateChanged -= SetTarget;
        GameManager.OnGameStateChanged -= ChangeCameraToMenuPosition;

    }
    private void LateUpdate()
    {
        if (!IsUpgrading)
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
    void ChangeCameraToMenuPosition(GameState state)
    {
        GameState CurrentState = state;
        if (CurrentState == GameState.Upgrading)
        {
            IsUpgrading = true;
            transform.DOLocalMove(upgradePosition, 0.5f).SetEase(Ease.Linear);
        }
        if (CurrentState == GameState.InGame)
        {
            StartCoroutine(ChangeCameraToGamePosition());
        }


    }

    IEnumerator ChangeCameraToGamePosition()
    {
        Tween t = transform.DOMove(followTarget.position + offset, 1f).SetEase(Ease.Linear);
        yield return new WaitWhile(() => t.IsPlaying());
        IsUpgrading = false;
    }
    private void FollowPlayer(Transform target)
    {
        transform.position = target.position + offset;
    }
}

