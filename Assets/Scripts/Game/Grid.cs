using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    // This class is for the grid.
    // public members
    public int width;
    public int height;
    public Cell[,] grid;
    // Constructor
    public Grid(int width, int height)
    {
        this.width = width;
        this.height = height;
        this.grid = new Cell[width, height];
    }

    public void CreateGrid()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; i++)
            {
                Cell cell = GameManager.Instance.cellFactory.CreateCell("r");
                cell.gridPos = new Vector2(i, j);
                grid[i, j] = cell;
            }
        }
    }
}
