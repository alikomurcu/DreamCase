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
    public int currentLevel = 0;
    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        else
        {
            // set the instance
            Instance = this;
        }
        // if exist load the game state on awake
        gameState = GameSaveManager.Instance.LoadGameState();
        if (gameState == null || gameState.LevelsDictionary.Count == 0)
        {
            // create a new game state
            this.gameState = new GameState();
            this.gameState.LevelsDictionary = new Dictionary<int, Tuple<bool, int>>();
            for (int i=0; i < 10; i++)
            {
                this.gameState.LevelsDictionary.Add(i+1, new Tuple<bool, int>(false, 0));       // add first ten levels to the dictionary
            }
            this.gameState.isGameCompleted = false;
        }
        DontDestroyOnLoad(gameObject);

    }

    public void Start()
    {
        Debug.Log("Current level: " + currentLevel);
    }

    public void SetGrid(Grid grid)
    {
        this.grid = grid;
    }
}
