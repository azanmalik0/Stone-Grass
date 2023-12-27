using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PickupManager : MonoBehaviour
{
    [SerializeField] GameObject pickupPrefab;
    [SerializeField] GameObject[] randomPoints;
    public float TimeRemaining;
    float _timeRemaining;
    public bool timerIsRunning = false;
    private void OnEnable()
    {
        _timeRemaining = TimeRemaining;
        timerIsRunning = true;
    }
    private void Start()
    {
    }
    void Update()
    {
        if (timerIsRunning)
        {
            if (_timeRemaining > 0)
            {
                _timeRemaining -= Time.deltaTime;
            }
            else
            {
                Debug.Log("Time has run out!");
                SpawnPickup();
                _timeRemaining = 0;
                timerIsRunning = false;
            }
        }
    }
    void SpawnPickup()
    {
        randomPoints = GameObject.FindGameObjectsWithTag("PickupPoint");
        int randomIndex = Random.Range(0, randomPoints.Length);
        Instantiate(pickupPrefab, randomPoints[randomIndex].transform.position, Quaternion.identity);
        Debug.Log("Pickup Instantiated");

    }

}
