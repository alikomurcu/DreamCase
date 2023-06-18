using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


[System.Serializable]
public class GameManager : MonoBehaviour
{
    // This is a singleton class which handles the general game logic.
    public static GameManager Instance { get; private set; }
    public Grid grid;
    public CellFactory cellFactory = new CellFactory();
    public InputManager inputManager;
    public GameState gameState = new GameState();
    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            Debug.Log("I am called gamemanager awake");
            return;
        }
        else
        {
            // set the instance
            Instance = this;
            Debug.Log("Not so fast roach");
        }
        // if exist load the game state on awake
        gameState = GameSaveManager.Instance.LoadGameState();
        if (gameState == null)
        {
            // create a new game state
            this.gameState = new GameState();
            this.gameState.LevelsDictionary = new Dictionary<int, Tuple<bool, int>>();
            this.gameState.isGameCompleted = false;
        }
        DontDestroyOnLoad(gameObject);
        Debug.Log("Call me once");
    }
    
    public void SetGrid(Grid grid)
    {
        this.grid = grid;
    }
}
