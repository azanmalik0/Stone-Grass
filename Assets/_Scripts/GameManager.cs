using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    GameState state;
    public GameObject tractorObject;
    public GameObject farmerObject;
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        UpdateGameState(GameState.Farmer);
    }

    public void UpdateGameState(GameState state)
    {
        this.state = state;
        print(this.state);

        switch (this.state)
        {
            case GameState.Tractor:
                tractorObject.SetActive(true);
                farmerObject.SetActive(false);
                break;
            case GameState.Farmer:
                tractorObject.SetActive(false);
                farmerObject.SetActive(true);
                break;
        }
    }

    public GameState GetState()
    {
        return this.state;
        print(this.state);
    }
}
public enum GameState { Tractor, Farmer }