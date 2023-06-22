using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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
    // create a dictionart key=int, value=bool
    public Dictionary<int, bool> completedRowsDict = new Dictionary<int, bool>();
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
        this.completedRowsDict = new Dictionary<int, bool>();
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
                cellInScene.GetComponent<CellObject>().cell = cell;     // bind cell 
                cell.gridPos = new Vector2(i, j);
                grid[i, j] = cellInScene;
            }
        }
    }

    public void CheckRow(int row)
    {
        /*
         * This method is responsible for checking if the row is full or not.
         * Make sure that calling this ,method after the animations are finished.
         */
        for(int i=0; i<width-1; i++)
        {
            if (grid[i, row].GetComponent<CellObject>().cell.GetType() != grid[i+1, row].GetComponent<CellObject>().cell.GetType())
            {
                return;
            }
        }
        // if we are here, it means that the row includes same type of cells
        // so we should make them ticks and increment the score
        for (int i = 0; i < width; i++)
        {
            // destroy the cell and create a tick cell
            Cell tickCell = GameManager.Instance.cellFactory.CreateCell("t");
            GameObject tickInScene = tickCell.InstantiateCell(grid[i, row].transform.position);
            // destroy the cell
            GameObject.Destroy(grid[i, row]);
            grid[i, row] = tickInScene;
        }

        int cellScore = grid[0, row].GetComponent<CellObject>().cell.Score;
        GameManager.Instance.currentScore += cellScore*width;   // increment the score
        // add the row to the completed rows dictionary, if all rows are completed, level is completed
        completedRowsDict.Add(row, true);
        if (completedRowsDict.Count == height)
        {
            // LEVEL COMPLETED
            GameManager.Instance.LevelFinished();
        }

    }

    public Vector2 FindSwipedCell(Vector2 worldPosition)
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
            return new Vector2(-1, -1);
        }
        // the swipe is in the grid at the below lines
        int xIndex = (int) ((x - mini) / (cellScale));
        int yIndex = (int) ((y - minj) / (cellScale));
        return new Vector2(xIndex, yIndex);
    }


    // Swipe operations
    public void Swipe(Vector2 gridPos, SwipeDirection direction)
    {
        if (direction == SwipeDirection.Up)
        {
            SwipeUp(gridPos);
        }
        else if (direction == SwipeDirection.Down)
        {
            SwipeDown(gridPos);
        }
        else if (direction == SwipeDirection.Left)
        {
            SwipeLeft(gridPos);
        }
        else if (direction == SwipeDirection.Right)
        {
            SwipeRight(gridPos);
        }
        PrintGrid();
    }

    private void SwipeUp(Vector2 current)
    {
        Vector2 up = new Vector2(current.x, current.y + 1);
        if (IsInGrid(up) && !IsTickCell(up) && !IsTickCell(current))
        {
            // swap the cells in the grid, swap up with current(current)
            (grid[(int) current.x, (int) current.y], grid[(int) up.x, (int) up.y]) = (grid[(int) up.x, (int) up.y], grid[(int) current.x, (int) current.y]);
            // move the cells
            // note that current is now the cell that was up
            // note also that up is now the cell that was current
            grid[(int) current.x, (int) current.y].GetComponent<CellObject>().MoveCell(new Vector2(0, -cellScale), (int)current.y);   // move current up
            grid[(int) up.x, (int) up.y].GetComponent<CellObject>().MoveCell(new Vector2(0, cellScale), (int)up.y);  // move up to down
        }
    }
    
    private void SwipeDown(Vector2 current)
    {
        Vector2 down = new Vector2(current.x, current.y - 1);
        if (IsInGrid(down) && !IsTickCell(down) && !IsTickCell(current))
        {
            // swap the cells in the grid, swap down with current(current)
            (grid[(int) current.x, (int) current.y], grid[(int) down.x, (int) down.y]) = (grid[(int) down.x, (int) down.y], grid[(int) current.x, (int) current.y]);
            // move the cells
            grid[(int) current.x, (int) current.y].GetComponent<CellObject>().MoveCell(new Vector2(0, cellScale), (int)current.y);   // move current down
            grid[(int) down.x, (int) down.y].GetComponent<CellObject>().MoveCell(new Vector2(0, -cellScale), (int)down.y);  // move down to up
        }
    }
    
    private void SwipeLeft(Vector2 current)
    {
        Vector2 left = new Vector2(current.x - 1, current.y);
        if (IsInGrid(left) && !IsTickCell(left) && !IsTickCell(current))
        {
            // swap the cells in the grid, swap left with current(current)
            (grid[(int) current.x, (int) current.y], grid[(int) left.x, (int) left.y]) = (grid[(int) left.x, (int) left.y], grid[(int) current.x, (int) current.y]);
            // move the cells
            grid[(int) current.x, (int) current.y].GetComponent<CellObject>().MoveCell(new Vector2(cellScale, 0), (int)current.y);   // move current left
            grid[(int) left.x, (int) left.y].GetComponent<CellObject>().MoveCell(new Vector2(-cellScale, 0), (int)left.y);  // move left to right
        }
    }
    
    private void SwipeRight(Vector2 current)
    {
        Vector2 right = new Vector2(current.x + 1, current.y);
        if (IsInGrid(right) && !IsTickCell(right) && !IsTickCell(current))
        {
            // swap the cells in the grid, swap right with current(current)
            (grid[(int) current.x, (int) current.y], grid[(int) right.x, (int) right.y]) = (grid[(int) right.x, (int) right.y], grid[(int) current.x, (int) current.y]);
            // move the cells
            grid[(int) current.x, (int) current.y].GetComponent<CellObject>().MoveCell(new Vector2(-cellScale, 0), (int)current.y);   // move current right
            grid[(int) right.x, (int) right.y].GetComponent<CellObject>().MoveCell(new Vector2(cellScale, 0), (int)right.y);  // move right to left
        }
    }

    public void PrintGrid()
    {
        //print the grid
        string s = "";
        for(int i=height-1; i>-1; i--)
        {
            for(int j=0; j<width; j++)
            {
                s += grid[j, i].GetComponent<CellObject>().cell.GetType().ToString()[0] + " ";
            }

            s += "\n";
        }
        Debug.Log(s);
    }

    // helpers
    private bool IsInGrid(Vector2 gridPos)
    {
        return gridPos.x >= 0 && gridPos.x < width && gridPos.y >= 0 && gridPos.y < height;
    }
    
    private bool IsTickCell(Vector2 gridPos)
    {
        return grid[(int) gridPos.x, (int) gridPos.y].GetComponent<CellObject>().cell.GetType() == typeof(TickCell);
    }
    private IEnumerator Wait06(int row1, int row2)
    {
        yield return new WaitForSeconds(0.6f);
        CheckRow(row1);
        CheckRow(row2);
    }
}
