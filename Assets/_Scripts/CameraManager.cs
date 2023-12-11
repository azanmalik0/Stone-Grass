using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraManager : MonoBehaviour
{
    Transform followTarget;
    [SerializeField] Vector3 upgradePosition;
    [SerializeField] Vector3 shopPosition;
    [SerializeField] Vector3 offset;
    [SerializeField] Vector3 grassFieldOffset;
    [SerializeField] Vector3 platformOffset;
    [SerializeField] float smoothSpeed;
    [SerializeField] GameObject tractorObject;
    bool IsInMenu;
    GameManager GM;
    GameState CurrentState;

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
    private void FixedUpdate()
    {
        if (!IsInMenu)
            FollowPlayer(followTarget);
    }
    void SetTarget(GameState state)
    {
        CurrentState = state;
        if (CurrentState == GameState.Tractor)
            //followTarget = GM.tractorObject.transform;
            followTarget = tractorObject.transform;
        if (CurrentState == GameState.Farmer)
            followTarget = GM.farmerObject.transform;

    }
    void ChangeCameraToNewPosition(GameState state)
    {
        CurrentState = state;
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
        if (CurrentState == GameState.OnGrassField)
        {
            ChangeCameraToGrassFieldPosition();
        }
        if (CurrentState == GameState.OnPlatform)
        {
            ChangeCameraToPlatformPosition();
        }

    }
    void ChangeCameraToPlatformPosition()
    {
        transform.DOMoveY(17, 1f).SetEase(Ease.OutSine).SetDelay(1f).OnStart(() => offset = platformOffset);
        transform.DORotate(new Vector3(47.97f, 0, 0), 1f).SetEase(Ease.Linear);
    }
    void ChangeCameraToGrassFieldPosition()
    {
        transform.DOMoveY(12.7f, 1f).SetEase(Ease.OutSine).SetDelay(1f).OnStart(() => offset = grassFieldOffset);
        transform.DORotate(new Vector3(47.97f, 0, 0), 1f).SetEase(Ease.Linear);

    }
    IEnumerator ChangeCameraToGamePosition()
    {
        transform.DOMove(followTarget.position + platformOffset, 0.5f).SetEase(Ease.Linear);
        transform.DORotate(new Vector3(47.97f, 0, 0), 0.5f).SetEase(Ease.Linear);
        yield return new WaitForSeconds(0.5f);
        IsInMenu = false;
    }
    private void FollowPlayer(Transform target)
    {
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }

}

