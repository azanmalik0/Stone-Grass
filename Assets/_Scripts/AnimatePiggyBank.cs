using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatePiggyBank : MonoBehaviour
{
    Transform piggy;
    Transform coin;
    void Start()
    {
        piggy = transform.GetChild(0);
        coin = transform.GetChild(1);
        Animate();
    }
    void Animate()
    {
        piggy.DOLocalRotate(new Vector3(-90, 360, 0), 4f, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Incremental);
        //piggy.DOLocalMoveY(-0.632f, 2f).SetLoops(-1, LoopType.Yoyo);
    }


}
