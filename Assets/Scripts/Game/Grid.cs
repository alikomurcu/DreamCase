using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Grid
{
    // This class is for the grid. An note that it is not MonoBehaviour.
    // public members
    public int width;
    public int height;
    public GameObject[,] grid;  // Contains CellObjects
    
    // add dont destroy on load

    public void CreateGrid(int width, int height, string[] gridColors)
    {
        // This method is responsible for creating the grid.
        // Note that the grid is created from the bottom left corner to the top right corner.
        // Note also that, the grid is not instantiated in the scene, it is just a data structure.
        this.width = width;
        this.height = height;
        this.grid = new GameObject[width, height];

        // set a responsive grid size according to grid size
        // choose the greater one
        float gridSize = width > height ? width : height;
        float cellScale = 5.0f/width;

        float xOfsett = -0.5f + width / 2.0f;
        float yOfsett = -1.0f + height / 2.0f;
        
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Cell cell = GameManager.Instance.cellFactory.CreateCell(gridColors[j*width+i]);     // this is cell class
                Vector3 position = new Vector3(cellScale*(i - xOfsett), cellScale*(j - yOfsett), 0);
                // instantiate the cell
                GameObject cellInScene = cell.InstantiateCell(position);
                cellInScene.transform.localScale *= cellScale;
                cell.gridPos = new Vector2(i, j);
                grid[i, j] = cellInScene;
            }
        }
    }

    public void SwipeRight()
    {
        
    }

    public void SwipeLeft()
    {
        
    }
    
    public void SwipeUp()
    {
        
    }
    
    public void SwipeDown()
    {
        
    }

}
