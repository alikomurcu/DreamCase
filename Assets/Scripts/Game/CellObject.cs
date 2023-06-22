using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellObject : MonoBehaviour
{
    // cell object is like a pointer to abstract class Cell
    // because we can not attach an abstract class to an object
    public Cell cell;

    // add dont destroy on load
    public void Awake()
    {
        DontDestroyOnLoad(this);
    }
}

public abstract class Cell
{
    /*
    * This is an absract class for the cells of the grid.
    */
    // Note that this array should contain the cell prefabs in the order of red, green, blue, yellow.
    public List<GameObject> cellPrefabList;
    public GameObject cellPrefab;        // the prefab of the cell
    public Vector2 gridPos;            // the position of the cell in the grid
    
    public abstract int Score { get; }
    
    public GameObject getCellPrefab()
    {
        // returns the cell prefab
        return cellPrefab;
    }

    public GameObject InstantiateCell(Vector3 position)
    {
        // instantiates the cell prefab
        GameObject newObject =  GameObject.Instantiate(cellPrefab, position, Quaternion.identity);
        newObject.name = position.ToString();
        return newObject;
    }
}

public class CellFactory
{
    // This class is for creating the cells of the grid with different types.
    // Uses Factory Design Pattern
    public Cell CreateCell(string color)
    {
        if (color == "r")
        {
            return new RedCell();
        }
        else if (color == "g")
        {
            return new GreenCell();
        }
        else if (color == "b")
        {
            return new BlueCell();
        }
        else if (color == "y")
        {
            return new YellowCell();
        }
        else
        {
            return null;
        }
    }
}

/*
 * Note that strategy pattern is used for the types of cells.
 * Maybe in the future, we will add more types of cells
 * or ve may include another aspects/properties for the cells.
 * So, we can easily add new types of cells without changing the existing code.
 */
public class RedCell : Cell
{
    // constructor
    public RedCell()
    {
        cellPrefab = GameManager.Instance.cellPrefabList[0];
    }
    public override int Score => 100;
}

public class GreenCell : Cell
{
    public GreenCell()
    {
        cellPrefab = GameManager.Instance.cellPrefabList[1];
    }
    public override int Score => 150;
}

public class BlueCell : Cell
{
    public BlueCell()
    {
        cellPrefab = GameManager.Instance.cellPrefabList[2];
    }
    public override int Score => 200;
}

public class YellowCell : Cell
{
    public YellowCell()
    {
        cellPrefab = GameManager.Instance.cellPrefabList[3];
    }
    public override int Score => 250;

}