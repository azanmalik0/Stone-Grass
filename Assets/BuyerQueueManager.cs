using SWS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyerQueueManager : MonoBehaviour
{
    public static BuyerQueueManager instance;
    public Queue<GameObject> buyerQueue = new();
    public Transform counterTransform;
    public bool IsCounterAvailable;

    private void Awake()
    {
        instance = this;
    }

    public void EnqueueBuyer(GameObject buyer)
    {
        buyerQueue.Enqueue(buyer);
        TryProcessQueue();
    }

    private void TryProcessQueue()
    {
        if (buyerQueue.Count > 0)
        {
            GameObject currentBuyer = buyerQueue.Peek();

            if (IsCounterAvailable)
            {
                currentBuyer.GetComponent<splineMove>().Resume();
                currentBuyer.GetComponentInChildren<Animator>().SetBool("IsWalking", true);
            }
            else
            {
                currentBuyer.GetComponent<splineMove>().Pause();
                currentBuyer.GetComponentInChildren<Animator>().SetBool("IsWalking", false);
            }
        }
    }

}
