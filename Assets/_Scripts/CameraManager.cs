using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraManager : MonoBehaviour
{
    [SerializeField] Transform followTarget;
    [SerializeField] Vector3 upgradePosition;
    [SerializeField] Vector3 shopPosition;
    [SerializeField] Vector3 offset;
    bool IsInMenu;
    GameManager GM;

    private void Start()
    {
        GM = GameManager.Instance;
    }

    private void OnEnable()
    {
        GameManager.OnGameStateChanged += SetTarget;
        GameManager.OnGameStateChanged += ChangeCameraToNewPosition;
    }
    private void OnDisable()
    {
        GameManager.OnGameStateChanged -= SetTarget;
        GameManager.OnGameStateChanged -= ChangeCameraToNewPosition;

    }
    private void LateUpdate()
    {
        if (!IsInMenu)
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
    void ChangeCameraToNewPosition(GameState state)
    {
        GameState CurrentState = state;
        if (CurrentState == GameState.Upgrading)
        {
            IsInMenu = true;
            transform.DOLocalMove(upgradePosition, 0.5f).SetEase(Ease.Linear);
        }
        if (CurrentState == GameState.InShop)
        {
            IsInMenu = true;
            transform.DOLocalMove(shopPosition, 0.5f).SetEase(Ease.Linear);
        }
        if (CurrentState == GameState.InGame)
        {
            StartCoroutine(ChangeCameraToGamePosition());
        }
        if (CurrentState == GameState.UnlockingArea)
        {
            IsInMenu = true;

        }



    }
    IEnumerator ChangeCameraToGamePosition()
    {
        Tween tm = transform.DOMove(followTarget.position + offset, 0.5f).SetEase(Ease.Linear);
        Tween tr = transform.DORotate(new Vector3(47.97f, 0, 0), 0.5f).SetEase(Ease.Linear);
        //yield return new WaitUntil(() => tm.IsComplete());
        yield return new WaitForSeconds(0.5f);
        IsInMenu = false;
    }

    private void FollowPlayer(Transform target)
    {
        transform.position = target.position + offset;
    }
}

