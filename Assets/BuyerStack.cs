using DG.Tweening;
using SWS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyerStack : Stacker
{
    bool IsBuying;
    float delay = 0;
    [SerializeField] GameObject eggsParent;
    [SerializeField] GameObject milkParent;
    [SerializeField] Animator animator;
    [SerializeField] GameObject moneyPrefab;
    [SerializeField] splineMove buyerSpline;

    void Start()
    {
        CalculateCellPositions();
        SetGridYOffset(gridOffset.y);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Market"))
        {
            StartCoroutine(BuyProduct(eggsParent, other));
            StartCoroutine(BuyProduct(milkParent, other));
        }

    }
    private void OnTriggerExit(Collider other)
    {

        if (other.CompareTag("Market"))
        {
            RefreshGrid();
            delay = 0;
        }
    }
    IEnumerator BuyProduct(GameObject productParent, Collider other)
    {
        Debug.LogError("Buy");
        IsBuying = true;
        buyerSpline.Pause();
        animator.Play("Idle");

        while (IsBuying && productParent.transform.childCount > 0 && transform.childCount <= maxHayCapacity)
        {
            Transform product = productParent.transform.GetChild(productParent.transform.childCount - 1);
            DOTween.Complete(product);
            product.SetParent(this.transform);
            product.DOLocalJump(cellPositions[currentC, currentR], 2, 1, 0.5f).SetDelay(delay).SetEase(Ease.OutSine).OnComplete(() => product.localRotation = Quaternion.identity);
            delay += 0.0001f;
            UpdateGridPositions();
            productParent.GetComponent<ProductMarketStack>().ResetGridPositions();
            yield return null;
        }
        //buyerSpline.Resume();
        //if (transform.childCount > maxHayCapacity)
        //{
        //    Instantiate(moneyPrefab, Vector3.zero, Quaternion.identity);
        //}





        //if (productParent.transform.childCount == 0 || transform.childCount > maxHayCapacity)
        //{
        //    delay = 0;
        //    RefreshGrid();
        //}
        //else if (productParent.transform.childCount > 0)
        //{
        //    Transform product = productParent.transform.GetChild(productParent.transform.childCount - 1);
        //    DOTween.Complete(product);
        //    product.SetParent(this.transform);
        //    product.DOLocalJump(cellPositions[currentC, currentR], 2, 1, 0.5f).SetDelay(delay).SetEase(Ease.OutSine).OnComplete(() => product.localRotation = Quaternion.identity);
        //    Instantiate(moneyPrefab)
        //    delay += 0.0001f;
        //    UpdateGridPositions();
        //    productParent.GetComponent<ProductMarketStack>().ResetGridPositions();
        //    transform.parent.GetComponentInParent<splineMove>().Resume();
        //}
    }
}
