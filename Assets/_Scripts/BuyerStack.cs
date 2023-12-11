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
    public bool IsBuying;
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
                int randomValue = UnityEngine.Random.Range(1, 3);
                if (randomValue == 1)
                {
                    StartCoroutine(BuyProduct(eggsParent));

                }
                else if (randomValue == 2)
                {

                    StartCoroutine(BuyProduct(milkParent));
                }
            }
        }

    }
    private void OnTriggerExit(Collider other)
    {

        if (other.CompareTag("Market"))
        {
           // Debug.LogError("MArket");
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
    bool IsFirst;
    IEnumerator BuyProduct(GameObject productParent)
    {
        if (productParent == eggsParent)
        {

            IsBuying = true;
            buyerSpline.Pause();
            animator.SetBool("IsWalking", false);

            if (productParent.transform.childCount <= 0)
            {
                StartCoroutine(BuyProduct(milkParent));
            }
            if (productParent.transform.childCount <= 0 && transform.childCount > 0)
            {
                if (!IsFirst)
                {
                    IsFirst = true;
                    StartCoroutine(BuyProduct(milkParent));
                }
                else
                {
                    //Debug.LogError("Bought1");
                    OnProductBought?.Invoke();
                    yield return new WaitForSeconds(2);
                    buyerSpline.Resume();
                    animator.SetBool("IsWalking", true);
                }
            }
            else if (transform.childCount >= maxHayCapacity)
            {
                //Debug.LogError("Bought1");
                OnProductBought?.Invoke();
                yield return new WaitForSeconds(2);
                buyerSpline.Resume();
                animator.SetBool("IsWalking", true);

            }
            else if (productParent.transform.childCount > 0)
            {

                //Debug.LogError("Buying");
                Transform product = productParent.transform.GetChild(productParent.transform.childCount - 1);
                DOTween.Complete(product);
                product.SetParent(this.transform);
                product.DOLocalJump(cellPositions[currentC, currentR], 2, 1, 0.5f).SetDelay(delay).SetEase(Ease.OutSine).OnComplete(() => product.localRotation = Quaternion.identity);
                delay += 0.0001f;
                UpdateGridPositions();
                productParent.GetComponent<ProductMarketStack>().ResetGridPositions();
                productParent.GetComponent<ProductMarketStack>().eggsStored--;
                IsBuying = false;
            }
            else
            {
                IsBuying = false;
            }
        }
        if (productParent == milkParent)
        {

            IsBuying = true;
            buyerSpline.Pause();
            animator.SetBool("IsWalking", false);

            //if (productParent.transform.childCount <= 0)
            //{
            //    StartCoroutine(BuyProduct(eggsParent));
            //}

            if (productParent.transform.childCount <= 0 && transform.childCount > 0)
            {
                //Debug.LogError("Bought2");
                OnProductBought?.Invoke();
                yield return new WaitForSeconds(2);
                buyerSpline.Resume();
                animator.SetBool("IsWalking", true);
            }
            else if (transform.childCount >= maxHayCapacity)
            {
                if (!IsFirst)
                {
                    IsFirst = true;
                    StartCoroutine(BuyProduct(eggsParent));
                }
                else
                {
                    //Debug.LogError("Bought1");
                    OnProductBought?.Invoke();
                    yield return new WaitForSeconds(2);
                    buyerSpline.Resume();
                    animator.SetBool("IsWalking", true);
                }

            }
            else if (productParent.transform.childCount > 0)
            {

                //Debug.LogError("Buying");
                Transform product = productParent.transform.GetChild(productParent.transform.childCount - 1);
                DOTween.Complete(product);
                product.SetParent(this.transform);
                product.DOLocalJump(cellPositions[currentC, currentR], 2, 1, 0.5f).SetDelay(delay).SetEase(Ease.OutSine).OnComplete(() => product.localRotation = Quaternion.identity);
                delay += 0.0001f;
                UpdateGridPositions();
                productParent.GetComponent<ProductMarketStack>().ResetGridPositions();
                productParent.GetComponent<ProductMarketStack>().milkStored--;
                IsBuying = false;
            }
            else
            {
                IsBuying = false;
            }
        }
    }
}
