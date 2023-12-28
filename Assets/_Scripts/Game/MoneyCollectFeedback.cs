using DG.Tweening;
using EZ_Pooling;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyCollectFeedback : MonoBehaviour
{
    RectTransform rectT;
    void Start()
    {
        rectT = GetComponent<RectTransform>();
        AnimateFeedback();
    }

    private void AnimateFeedback()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(rectT.DOAnchorPosY(rectT.anchoredPosition.y + 30, 0.5f).SetEase(Ease.Linear))
            .Insert(0.3f, transform.GetComponent<Text>().DOFade(0, 0.2f).SetEase(Ease.Linear))
            .Insert(0.3f, transform.GetComponent<UnityEngine.UI.Outline>().DOFade(0, 0.2f).SetEase(Ease.Linear))
            .OnComplete(() =>
            {
                Destroy(this.gameObject);
                //EZ_PoolManager.Despawn(this.transform);
            });
    }

}
