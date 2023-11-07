 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.CodeDom.Compiler;
public class FoodMachineModule : ItemCarryModule
{   
    AudioSource machineAudioSource;
    int validItemIndex=1;
    public Animator foodMachineAnimator;
    public ParticleSystem smokeParticle;
    public Renderer grillRenderer;
    public float CookingInterval=0.2f;
    float defaultCookingInterval=0;
    public Transform foodItem;
    bool isMachineWorking=false;
    public Transform[] targetPoisition;
    Transform targetTrasform;
    void Start()
    {
        defaultCookingInterval=CookingInterval;
    }

    // Update is called once per frame
    void Update()
    {
        if(itemsCarried<itemsholdingCapacity)
        {
            if(!isMachineWorking)
            StartMachine();

            if(CookingInterval>0)
            {
                CookingInterval-=Time.deltaTime;
            }
            else
            {
                GenerateItemInStack();
                CookingInterval=defaultCookingInterval;
            }
        }
        else
        {
            if(isMachineWorking)
            StopMachine();
        }
    }
    public void StartMachine()
    {   
        isMachineWorking=true;
        grillRenderer.material.SetColor("_BaseColor", Color.red);
        foodMachineAnimator.SetBool("On",true);
        smokeParticle.Play();
        indicator.SetActive(false);
        machineAudioSource.Play();
    }
    public void StopMachine()
    {
        isMachineWorking=false;
        grillRenderer.material.SetColor("_BaseColor", Color.grey);
        foodMachineAnimator.SetBool("On",false);
        smokeParticle.Stop();
        indicator.SetActive(true);
        machineAudioSource.Stop();
    }
    public void GenerateItemInStack()
    {   
        if(targetTrasform!=null)
        {
            if(itemsCarried>1)
            return;
        }
        Vector3 tempPosition;
        int stackIndex=itemsCarried%2;
        
            yfactor=0.4f*(itemsCarried/2);

            if(yfactor==0)
            yfactor=0.15f;
           
           tempPosition=new Vector3(targetPoisition[stackIndex].position.x,yfactor,targetPoisition[1].position.z);
           //Transform tempItem= EZ_PoolManager.Spawn(foodItem,tempPosition,targetPoisition[0].rotation);
           //itemsContained.Add(tempItem);
        itemsCarried++;

        
    }
    void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))
        { 
        if(itemsCarried>0)
        {   
            targetTrasform=other.transform;
            defaultCookingInterval=0.6f;
            if(delayVal>0)
            delayVal-=Time.deltaTime;
            else
            {
                //if(other.GetComponent<PlayerAddonsModule>().holdingItemIndex==validItemIndex || other.GetComponent<PlayerAddonsModule>().holdingItemIndex==0)
                //  {
                //    bool haveCapacity= other.GetComponent<PlayerAddonsModule>().AddItemInStack(itemsContained[itemsContained.Count-1]);
                //    if(haveCapacity)
                //    {
                //    other.GetComponent<PlayerAddonsModule>().ChangePlayerState(1);
                //    GiveItemFromStack();
                //    }
                //  }
                //delayVal=defaultDelayVal;
            }
        }
    }
    
    }
    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            targetTrasform=null; 
            defaultCookingInterval=1;
        }
    }
}
