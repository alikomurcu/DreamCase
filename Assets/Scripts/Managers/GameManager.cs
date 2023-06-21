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
    // it is a factory for creating the cells of the grid.
    public CellFactory cellFactory = new CellFactory();
    // Note that this array should contain the cell prefabs in the order of red, green, blue, yellow.
    [SerializeField] public List<GameObject> cellPrefabList = new List<GameObject>(4);
    // handles the input of the player such as swipes and taps.
    public InputManager inputManager;
    // handles the game state such as current level, score, etc.
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
        /*
         *  quick note about following line of code,
         * we are now inside of Awake method of GameManager however in the below line
         * we will call the singleton instance of GameSaveManager which may not be created yet,
         * therefore it may lead to a null reference exception.
         * To handle this, I got one quick solution:
            * Go Project Settings -> Script Execution Order
            * Make sure that GameSaveManager is executed before GameManager
            * Hence, there will be no null reference exception.
         */
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

    public void SetGrid()
    {
        // make sure before calling this method, currentLevel is set
        // set grid according to current level
        this.grid = LevelParser.Instance.ParseLevel(currentLevel);
        
    }
}
