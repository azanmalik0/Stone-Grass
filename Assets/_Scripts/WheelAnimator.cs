using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelAnimator : MonoBehaviour
{
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).DORotate(new(360, 0, 0), 2, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental);

        }

    }


}
