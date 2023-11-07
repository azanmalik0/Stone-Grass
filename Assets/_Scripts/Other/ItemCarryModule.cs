using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using Random = UnityEngine.Random;
using UnityEngine.AI;

public abstract class ItemCarryModule : MonoBehaviour
{
    public bool hasRandomCapacity = false;
    public int holdingItemIndex;
    public Transform placeHolderTransform;
    public float yfactor = 0.4f;
    public GameObject indicator;
    public List<Transform> itemsContained;
    public int itemsCarried = 0;
    public int itemsholdingCapacity;
    protected float delayVal = 0.15f;
    protected float defaultDelayVal = 0;
    public float jumpSpeed = 0.13f;

    void OnEnable()
    {
        if (hasRandomCapacity)
            itemsholdingCapacity = Random.Range(1, 7);
    }
    void Start()
    {
        defaultDelayVal = delayVal;

    }
    void Update()
    {

    }
    public bool AddGarbageInStack(Transform item)
    {
        if (itemsCarried == 0)
            //holdingItemIndex = item.GetComponent<PickupItemModule>().itemIndex;
        itemsContained.Add(item);
        item.SetParent(placeHolderTransform.parent);
        item.DOLocalJump(new Vector3(placeHolderTransform.localPosition.x, 0.4f * itemsCarried, placeHolderTransform.localPosition.z), 2, 1, jumpSpeed, false);
        item.rotation = placeHolderTransform.rotation;
        itemsCarried++;
        return true;
    }
    public bool AddItemInStack(Transform item)
    {
        if (itemsCarried < itemsholdingCapacity)
        {

            if (itemsCarried == 0)
                //holdingItemIndex = item.GetComponent<PickupItemModule>().itemIndex;
            itemsContained.Add(item);
            item.SetParent(placeHolderTransform.parent);
            item.DOLocalJump(new Vector3(placeHolderTransform.localPosition.x, 0.4f * itemsCarried, placeHolderTransform.localPosition.z), 2, 1, jumpSpeed, false);
            item.rotation = placeHolderTransform.rotation;
            itemsCarried++;
            if (itemsCarried == itemsholdingCapacity)
            {
                if ((bool)indicator)
                    indicator.SetActive(true);
            }

            return true;
        }
        else
        {
            if (indicator != null)
            {
                indicator.SetActive(true);
            }
            return false;
        }
    }
    //public Transform GiveItemFromStack()
    //{
    //    if (itemsCarried > 0)
    //    {
    //        Transform tempItem = itemsContained[itemsContained.Count - 1];
    //        itemsContained.Remove(tempItem);

    //        if (indicator != null)
    //            indicator.SetActive(false);
    //        itemsCarried--;
    //        return tempItem;
    //    }
    //    else
    //    {
    //        holdingItemIndex = 0;
    //        if (gameObject.tag == "Player")
    //            //GetComponent<PlayerAddonsModule>().ChangePlayerState(0);
    //        return null;
    //    }
    //}
    public int GetHoldingItemIndex()
    {
        return holdingItemIndex;
    }

}
