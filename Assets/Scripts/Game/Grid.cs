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

    public void CreateGrid(int width, int height, string[] gridColors)
    {
        // This method is responsible for creating the grid.
        // Note that the grid is created from the bottom left corner to the top right corner.
        // Note also that, the grid is not instantiated in the scene, it is just a data structure.
        this.width = width;
        this.height = height;
        this.grid = new Cell[width, height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Cell cell = GameManager.Instance.cellFactory.CreateCell(gridColors[j*width+i]);
                cell.gridPos = new Vector2(i, j);
                grid[i, j] = cell;
            }
        }
    }
    
    
}
