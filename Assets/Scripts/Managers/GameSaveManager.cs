using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;


[System.Serializable]
public class GameState
{
    public int minLevel;
    // This class is responsible from storing the game state.
    public bool isGameCompleted;
    // A dictionary to save the level data, level:(isLocked, highest score)
    public Dictionary<int, Tuple<bool, int>> LevelsDictionary = new Dictionary<int, Tuple<bool, int>>();
}

public class GameSaveManager : MonoBehaviour
{
    // Singleton instance
    public static GameSaveManager Instance { get; private set; }
    private const string SaveFilePath = "Assets/Saves/save.json";

    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }


    public void SaveGameState(GameState gameState)
    {
        string json = JsonConvert.SerializeObject( gameState );
        // concatenate the json strings
        File.WriteAllText(SaveFilePath, json);
    }

    public GameState LoadGameState()
    {
        if (File.Exists(SaveFilePath))
        {
            string json = File.ReadAllText(SaveFilePath);
            return JsonConvert.DeserializeObject<GameState>(json);
        }

        return null; // or return a default game state
    }

    public void OnApplicationQuit()
    {
        // save the game state on quit
        Instance.SaveGameState(GameManager.Instance.gameState);
    }
}
