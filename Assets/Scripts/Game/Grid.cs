using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public enum SwipeDirection
{
    Up,
    Down,
    Left,
    Right
}
public class Grid
{
    // This class is for the grid. An note that it is not MonoBehaviour.
    // public members
    public int width;
    public int height;
    public GameObject[,] grid;  // Contains CellObjects
    
    /*
     * The x
     */
    private float xOffset, yOffset, cellScale;
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
        float gridSizeMax = width > height ? width : height;
        cellScale = 5.0f/gridSizeMax;

        xOffset = -0.5f + width / 2.0f;
        yOffset = -1.0f + height / 2.0f;
        
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Cell cell = GameManager.Instance.cellFactory.CreateCell(gridColors[j*width+i]);     // this is cell class
                // multiplying with cellScale to make the grid responsive
                // i - xoffset and j - yoffset to make the grid centered at origin
                Vector3 position = new Vector3(cellScale*(i - xOffset), cellScale*(j - yOffset), 0);
                // instantiate the cell, this is the instantiated object in the scene, so that we will move it when swipe
                GameObject cellInScene = cell.InstantiateCell(position);
                cellInScene.transform.localScale *= cellScale;
                cell.gridPos = new Vector2(i, j);
                grid[i, j] = cellInScene;
            }
        }
    }

    public void FindSwipedCell(Vector2 worldPosition)
    {
        // make sure that CreateGrid method is called before this method
        // so that, celScale, xOffset, yOffset are initialized
        
        // set min and max values for i and j, meaning x and y coordinates of the cells in the grid, (REFER line 49 in this file)
        // so that we can check if the swipe is in the grid or not
        // furthermore, we will use these values to find the cell that is swiped
        // the reason of -1 and width/height is that, we want to encapsulate the grid with a rectangle, i.e., bounding box
        float mini = cellScale * (-1 - xOffset) + cellScale / 2.0f;
        float maxi = cellScale * ((width) - xOffset) - cellScale / 2.0f;
        float minj = cellScale * (-1 - yOffset) + cellScale / 2.0f;
        float maxj = cellScale * ((height) - yOffset) - cellScale / 2.0f;
        Debug.Log("mini " + mini + " maxi " + maxi + " minj " + minj + " maxj " + maxj);
        float x = worldPosition.x;
        float y = worldPosition.y;
        
        if (x < mini || x > maxi || y < minj || y > maxj)
        {
            // the swipe is not in the grid
            return;
        }
        // the swipe is in the grid at the below lines
        int xIndex = (int) ((x - mini) / (cellScale));
        int yIndex = (int) ((y - minj) / (cellScale));
        Debug.Log("xIndex: " + xIndex + " yIndex: " + yIndex);
    }


    // Swipe operations
    public void Swipe(Vector2 position, SwipeDirection direction)
    {
        if (direction == SwipeDirection.Up)
        {
            
        }
        else if (direction == SwipeDirection.Down)
        {
            
        }
        else if (direction == SwipeDirection.Left)
        {
            
        }
        else if (direction == SwipeDirection.Right)
        {
            
        }
    }

    private void SwipeUp()
    {
        
    }
    
    private void SwipeDown()
    {
        
    }
    
    private void SwipeLeft()
    {
        
    }
    
    private void SwipeRight()
    {
        
    }
}
