using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public static event Action<AnimalType> OnTimeOut;
    public AnimalType animalType;
    public float TimeRemaining;
    float _timeRemaining;
    public bool timerIsRunning = false;
    [SerializeField] Text timeText;
    private void OnEnable()
    {
        _timeRemaining = TimeRemaining;
        timerIsRunning = true;
    }
    void Update()
    {

        if (timerIsRunning)
        {
            if (_timeRemaining > 0)
            {
                DisplayTime(_timeRemaining);
                _timeRemaining -= Time.deltaTime;
            }
            else
            {
                Debug.Log("Time has run out!");
                _timeRemaining = 0;
                timerIsRunning = false;
                OnTimeOut?.Invoke(animalType);
            }
        }
    }
    void DisplayTime(float timeToDisplay)
    {
        timeText.text = timeToDisplay.ToString("F0");
    }
}
