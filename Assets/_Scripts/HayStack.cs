using DG.Tweening;
using PT.Garden;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HayStack : MonoBehaviour
{
    [SerializeField] GameObject hayPrefab;
    [SerializeField] Transform hayTargetPosition;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(2);
        StackHay();
    }

    void Update()
    {


    }

    void StackHay()
    {
        //Instantiate(hayPrefab, hayTargetPosition.position, Quaternion.identity);
        hayPrefab.transform.DOJump(hayTargetPosition.position,5,1, 1f).SetEase(Ease.Linear);
    }
}
