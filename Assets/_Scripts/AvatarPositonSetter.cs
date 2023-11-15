using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarPositonSetter : MonoBehaviour
{
    [SerializeField] Transform upgradeZonePosition;
    [SerializeField] Transform shopZonePosition;
    private void OnEnable()
    {
        GameManager.OnGameStateChanged += ChangeAvatarPosition;
    }
    private void OnDisable()
    {
        GameManager.OnGameStateChanged -= ChangeAvatarPosition;

    }
    void ChangeAvatarPosition(GameState state)
    {
        GameState CurrentState = state;
        if (CurrentState == GameState.Upgrading)
        {
            transform.DOLocalMove(upgradeZonePosition.localPosition, 0.5f).SetEase(Ease.Linear);
            transform.DOLocalRotate(upgradeZonePosition.localEulerAngles, 0.5f).SetEase(Ease.Linear);
        }
        if (CurrentState == GameState.InShop)
        {
            transform.DOLocalMove(shopZonePosition.localPosition, 0.5f).SetEase(Ease.Linear);
            transform.DOLocalRotate(shopZonePosition.localEulerAngles, 0.5f).SetEase(Ease.Linear);
        }


    }
}
