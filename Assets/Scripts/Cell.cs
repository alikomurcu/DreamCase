using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CellFactory
{
    // This class is for the cells of the grid.
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

public abstract class Cell : MonoBehaviour
{
    // This class is for the cells of the grid.
    public Vector2 gridPos;
    public abstract int Score { get; }
    // Constructor
    protected Cell()
    {
        ;
    }

    public void SwipeRight()
    {
    }

    public void SwipeLeft()
    {
    }
}

public class RedCell : Cell
{
    public override int Score => 100;
}

public class GreenCell : Cell
{
    public override int Score => 150;
}

public class BlueCell : Cell
{
    public override int Score => 200;
}

public class YellowCell : Cell
{
    public override int Score => 250;

}