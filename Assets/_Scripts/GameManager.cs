using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    GameState state;
    public static event Action<GameState> OnGameStateChanged;
    public GameObject tractorObject;
    public GameObject farmerObject;
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        UpdateGameState(GameState.Tractor);
    }

    public void UpdateGameState(GameState NewState)
    {
        this.state = NewState;

        switch (NewState)
        {
            case GameState.Tractor:
                farmerObject.SetActive(false);
                tractorObject.transform.position = farmerObject.transform.position;
                tractorObject.transform.rotation = farmerObject.transform.rotation;
                tractorObject.SetActive(true);
                break;
            case GameState.Farmer:
                tractorObject.SetActive(false);
                farmerObject.transform.position = tractorObject.transform.position;
                farmerObject.transform.rotation = tractorObject.transform.rotation;
                farmerObject.SetActive(true);
                break;
        }

        OnGameStateChanged?.Invoke(NewState);
    }

}
public enum GameState { Tractor, Farmer }