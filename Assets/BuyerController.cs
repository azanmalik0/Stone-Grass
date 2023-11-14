using SWS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyerController : MonoBehaviour
{
    [SerializeField] BuyerStack buyerStack;
    private void Start()
    {
        buyerStack = GetComponentInChildren<BuyerStack>();

    }

    public void DoAtWayPoint(int point)
    {
        switch (point)
        {
            case 7:
                buyerStack.ResetStack();
                break;

        }

    }

}
