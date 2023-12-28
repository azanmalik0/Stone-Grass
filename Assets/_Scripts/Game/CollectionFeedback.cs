using EZ_Pooling;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionFeedback : MonoBehaviour
{
    public static CollectionFeedback instance;
    [SerializeField] GameObject parentCanvas;
    [SerializeField] GameObject feedbackPrefab;

    private void Awake()
    {
        instance = this;
    }
    public void SpawnFeedbackPrefab()
    {
        //Transform prefab = EZ_PoolManager.Spawn(feedbackPrefab.transform, feedbackPrefab.transform.position, Quaternion.identity);
        Instantiate(feedbackPrefab,feedbackPrefab.transform.position,Quaternion.identity,parentCanvas.transform);
        //prefab.SetParent(parentCanvas.transform);
    }

}
