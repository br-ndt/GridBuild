using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridNode
{
    #region Fields/Properties
    private Grid<GridNode> grid;
    
    public int x 
    {
        get;
        private set;
    }
    public int y
    {
        get;
        private set;
    }
    public int gCost;
    public int hCost;
    public int fCost;
    public bool isWalkable
    {
        get;
        private set;
    }
    private Transform groundTransform;
    private Transform placedTransform;
    private Transform ceilingTransform;
    public List<GridNode> neighbors { get; private set; }
    public GridNode prevNode;
    #endregion

    #region Constructor
    public GridNode(Grid<GridNode> grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
        isWalkable = true;
        neighbors = new List<GridNode>();
    }
    #endregion

    public bool GetNeighborList()
    {
        neighbors.Clear();
        if(x - 1 >= 0)
        {
            //LEFT
            neighbors.Add(grid.GetGridObject(x - 1, y));
            //LEFTDOWN
            if(y - 1 >= 0) neighbors.Add(grid.GetGridObject(x - 1, y - 1));
            //LEFTUP
            if(y + 1 < grid.GetHeight()) neighbors.Add(grid.GetGridObject(x - 1, y + 1));
        }
        if(x + 1 < grid.GetWidth())
        {
            //RIGHT
            neighbors.Add(grid.GetGridObject(x + 1, y));
            //RIGHTDOWN
            if(y - 1 >= 0) neighbors.Add(grid.GetGridObject(x + 1, y - 1));
            //RIGHTUP
            if(y + 1 < grid.GetHeight()) neighbors.Add(grid.GetGridObject(x + 1, y + 1));
        }
        //DOWN
        if(y - 1 >= 0) neighbors.Add(grid.GetGridObject(x, y - 1));
        //UP
        if(y + 1 < grid.GetHeight()) neighbors.Add(grid.GetGridObject(x, y + 1));

        if(neighbors.Count > 0) return true;
        else return false;
    }

    #region Getters
    public Grid<GridNode> GetGrid()
    {
        return grid;
    }

    public Transform GetTransform()
    {
        return placedTransform;
    }

    public Transform GetTransform(string layer = "")
    {
        switch(layer)
        {
            case "ground":
                return groundTransform;
            case "ceiling":
                return ceilingTransform;
            case "placed":
            default:
                return GetTransform();
        }
    }

    public override string ToString()
    {
        return x + ", " + y + "\n";
    }
    #endregion

    #region Pathfinding
    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }

    public void SetIsWalkable(bool value)
    {
        isWalkable = value;
    }
    #endregion

    #region Building
    public bool PlacementAvailable()
    {
        return placedTransform == null;
    }

    public void SetTransform(Transform transform)
    {
        placedTransform = transform;
        GridEventManager.GridObjectChanged(this, x, y);
        foreach(GridNode neighbor in neighbors)
        {
            Debug.Log("Notifying neighbors of tile " + x + "," + y);
            neighbor.NeighborUpdate(this);
        }
    }

    public void NeighborUpdate(GridNode sender)
    {
        if(neighbors.Contains(sender))
        {
            //test
            if(placedTransform != null)
            {
                if(placedTransform.GetComponent<PlacedObject>() != null)
                {
                    placedTransform.GetComponent<PlacedObject>().UpdatePositionalSprite(GetNeighborIndex());
                }
            }
        }
    }

    private int GetNeighborIndex()
    {
        if(neighbors.Count == 0)
            return 0;

        bool east = false, west = false, north = false, south = false;

        foreach(GridNode neighbor in neighbors)
        {
            if(neighbor.x == x - 1)
            {
                if(neighbor.y == y)
                {
                    if(neighbor.GetTransform() != null)
                        west = true;
                }
            }
            if(neighbor.x == x)
            {
                if(neighbor.y == y + 1)
                {
                    if(neighbor.GetTransform() != null)
                        north = true;
                }
                if(neighbor.y == y - 1)
                {
                    if(neighbor.GetTransform() != null)
                        south = true;
                }
            }
            if(neighbor.x == x + 1)
            {
                if(neighbor.y == y)
                {
                    if(neighbor.GetTransform() != null)
                        east = true;
                }
            }
        }
    
        if(east)
        {
            if(west) // ─ ─ ─
            {
                if(north) // ┴ ┴ ┴
                {
                    if(south) // all sides
                    {
                        return 16;
                    }
                    return 10;
                }
                if(south) // ┳ ┳ ┳
                {
                    return 22;
                }
                return 7;
            }

            if(north) // L L L
            {
                if(south) // ┝  ┝  ┝ 
                {
                    return 15;
                }
                return 6;
            }

            if(south)
            {
                return 18;
            }
            // east only
            return 1;
        }
        if(west)
        {
            if(north) // ┛ ┛ ┛
            {
                if(south) // ┫ ┫ ┫
                {
                    return 17;
                }
                return 8;
            }
            if(south) // ┓ ┓ ┓
            {
                return 20;
            }
            // west only
            return 2;
        }
        if(north)
        {
            if(south) // | | |
            {
                return 4;
            }
            // north only
            return 5;
        }
        if(south) // south only
        {
            return 3;
        }
        //no neighbors
        return 0;
    }
    #endregion
}
