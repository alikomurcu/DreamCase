using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LevelParser : MonoBehaviour
{
    // Singleton 
    public static LevelParser Instance { get; private set; }
    
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
    // read from the file and create the grid
    public Grid ParseLevel(string levelName)
    {
        // read the file
        string[] lines = File.ReadAllLines("Assets/Levels/RM_A" + levelName);
        // get the level number
        string[] levelstr = lines[0].Split(' ');
        int level = int.Parse(levelstr[1]);
        // get the width
        string[] widthstr = lines[1].Split(' ');
        int width = int.Parse(widthstr[1]);
        // get the height
        string[] heightstr = lines[2].Split(' ');
        int height = int.Parse(heightstr[1]);
        // get the number of moves
        string[] movesstr = lines[3].Split(' ');
        int moves = int.Parse(movesstr[1]);
        // create the grid
        Grid grid = new Grid(width, height);
        // fill the grid
        string[] gridstr = lines[4].Split(' ');
        string[] gridcolors = gridstr[1].Split(',');
        
        for (int j = 0; j < height; j++)
        {
            for (int i = 0; i < width; i++)
            {
                Cell cell = GameManager.Instance.cellFactory.CreateCell(gridcolors[j*width+i]);
                cell.gridPos = new Vector2(i, j);
                grid.grid[i, j] = cell;
            }
        }

        return grid;
    }
}
