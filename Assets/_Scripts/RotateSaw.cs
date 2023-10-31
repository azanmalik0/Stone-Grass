using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSaw : MonoBehaviour
{
    void Start()
    {
        transform.DORotate(new Vector3(0, 360, 0), 0.5f, RotateMode.FastBeyond360).SetRelative(true).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);

    }


}
