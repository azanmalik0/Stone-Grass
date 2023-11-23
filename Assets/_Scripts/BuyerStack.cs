using DG.Tweening;
using SWS;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BuyerStack : Stacker
{
    public static event Action OnProductBought;
    bool IsBuying;
    float delay = 0;
    [SerializeField] GameObject eggsParent;
    [SerializeField] GameObject milkParent;
    [SerializeField] Animator animator;
    [SerializeField] GameObject moneyPrefab;
    [SerializeField] splineMove buyerSpline;

    private void Awake()
    {
        SetGridYOffset(gridOffset.y);

    }
    void Start()
    {
        CalculateCellPositions();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Market"))
        {
            if (!IsBuying)
            {
                StartCoroutine(BuyProduct(eggsParent));
                StartCoroutine(BuyProduct(milkParent));
            }
        }

    }
    private void OnTriggerExit(Collider other)
    {

        if (other.CompareTag("Market"))
        {
            //RefreshGrid();
            delay = 0;
        }
    }

    public void ResetStack()
    {
        IsBuying = false;
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
            ResetGridPositions();

        }

    }
    IEnumerator BuyProduct(GameObject productParent)
    {
        IsBuying = true;
        buyerSpline.Pause();
        animator.SetBool("IsWalking", false);

        if (transform.childCount >= maxHayCapacity)
        {
            Debug.LogError("Bought");
            OnProductBought?.Invoke();
            yield return new WaitForSeconds(2);
            buyerSpline.Resume();
            animator.SetBool("IsWalking", true);

        }

        else if (productParent.transform.childCount == 0)
        {
            animator.SetBool("IsWalking", false);
            IsBuying = false;
        }
        else if (productParent.transform.childCount > 0 && transform.childCount <= maxHayCapacity)
        {
            Transform product = productParent.transform.GetChild(productParent.transform.childCount - 1);
            DOTween.Complete(product);
            product.SetParent(this.transform);
            product.DOLocalJump(cellPositions[currentC, currentR], 2, 1, 0.5f).SetDelay(delay).SetEase(Ease.OutSine).OnComplete(() => product.localRotation = Quaternion.identity);
            delay += 0.0001f;
            UpdateGridPositions();
            productParent.GetComponent<ProductMarketStack>().ResetGridPositions();
            IsBuying = false;
        }
    }
}
