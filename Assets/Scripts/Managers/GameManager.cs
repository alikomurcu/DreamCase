using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


[System.Serializable]
public class GameManager : MonoBehaviour
{
    // This is a singleton class which handles the general game logic.
    public static GameManager Instance { get; private set; }
    public Grid grid;
    // it is a factory for creating the cells of the grid.
    public CellFactory cellFactory = new CellFactory();
    // Note that this array should contain the cell prefabs in the order of red, green, blue, yellow.
    [SerializeField] public List<GameObject> cellPrefabList;
    // handles the input of the player such as swipes and taps.
    public InputManager inputManager;
    // handles the game state such as current level, score, etc.
    public GameState gameState = new GameState();
    public int currentLevel = 0;
    public int currentScore = 0;
    public int currentMoveCount = 0;
    public bool prevLevelHighestScore = false;
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
            this.gameState.minLevel = 1;
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
    
    public void SetGridInactive()
    {
        // set grid inactive
        foreach (GameObject child in grid.grid)
        {
            child.SetActive(false);
        }
    }
    
    public void DestroyGrid()
    {
        foreach (GameObject child in grid.grid)
        {
            Destroy(child.gameObject);
        }
        grid.grid = null;
    }
    
    public void HandleSwipe(Vector2 position, SwipeDirection direction)
    {
        Vector2 gridPos = grid.FindSwipedCell(position);
        grid.Swipe(gridPos, direction);
        
    }

    public void SetMoveCount(int move)
    {
        currentMoveCount = move;
    }

    public void DecrementMoveCount()
    {
        currentMoveCount--;
        GameUI.Instance.RenderCount();
        if (currentMoveCount == 0)
        {
            // game over
            LevelFinished();
        }
    }
    public void IncrementScore(int score)
    {
        currentScore += score;
        GameUI.Instance.RenderScore();
    }
    public void LevelFinished(string message = null)
    {
        if (message == "no")
        {
            GameUI.Instance.RenderNoPossibleMoves();
            StartCoroutine("Wait1AndLevelFinish", 3);
        }
        else
        {
            StartCoroutine("Wait1AndLevelFinish");
        }
    }
    private IEnumerator Wait1AndLevelFinish(int waitTime = 1)
    {
        // Wait for 1 second and then finish the level
        // This wait is for animations to be completed
        yield return new WaitForSeconds(1f);
        // min level is the level that player can play
        gameState.minLevel = currentLevel + 1 > gameState.minLevel ? currentLevel + 1 : gameState.minLevel;
        if (gameState.minLevel > 10)
        {
            DownloadNewLevel();
        }

        if(currentScore > gameState.LevelsDictionary[currentLevel].Item2)
        {
            gameState.LevelsDictionary[currentLevel] = new Tuple<bool, int>(true, currentScore);
            // save the game state
            GameSaveManager.Instance.SaveGameState(gameState);
            currentScore = 0;
            prevLevelHighestScore = true;
            DestroyGrid();
            GameUI.Instance.CongratsPopup();
        }
        else
        {
            DestroyGrid();
            SceneManager.LoadScene("MainScene");
            currentScore = 0;
            prevLevelHighestScore = false;
        }
    }

    public void DownloadNewLevel()
    {
        Dictionary<int, string> levelURLs = new Dictionary<int, string>();
        levelURLs.Add(11, "https://row-match.s3.amazonaws.com/levels/RM_A11");
        levelURLs.Add(12, "https://row-match.s3.amazonaws.com/levels/RM_A12");
        levelURLs.Add(13, "https://row-match.s3.amazonaws.com/levels/RM_A13");
        levelURLs.Add(14, "https://row-match.s3.amazonaws.com/levels/RM_A14");
        levelURLs.Add(15, "https://row-match.s3.amazonaws.com/levels/RM_A15");
        levelURLs.Add(16, "https://row-match.s3.amazonaws.com/levels/RM_B1");
        levelURLs.Add(17, "https://row-match.s3.amazonaws.com/levels/RM_B2");
        levelURLs.Add(18, "https://row-match.s3.amazonaws.com/levels/RM_B3");
        levelURLs.Add(19, "https://row-match.s3.amazonaws.com/levels/RM_B4");
        levelURLs.Add(20, "https://row-match.s3.amazonaws.com/levels/RM_B5");
        levelURLs.Add(21, "https://row-match.s3.amazonaws.com/levels/RM_B6");
        levelURLs.Add(22, "https://row-match.s3.amazonaws.com/levels/RM_B7");
        levelURLs.Add(23, "https://row-match.s3.amazonaws.com/levels/RM_B8");
        levelURLs.Add(24, "https://row-match.s3.amazonaws.com/levels/RM_B9");
        levelURLs.Add(25, "https://row-match.s3.amazonaws.com/levels/RM_B10");
        
        string currentLevelURL = levelURLs[gameState.minLevel];
        // split currentLevelURL to get the level name
        string splitURL = currentLevelURL.Split('/')[4];
        string path ="Assets/Levels/" + splitURL;
        // download the level
        StartCoroutine(GetText(path, currentLevelURL));
    }
    
    IEnumerator GetText(string file_name, string url)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.Send();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                System.IO.File.WriteAllText(file_name, www.downloadHandler.text);
                gameState.LevelsDictionary.Add(gameState.minLevel, new Tuple<bool, int>(false, 0));
            }
        }
    }

}
