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
    public List<GridNode> neighbors;
    public GridNode prevNode;
    #endregion

    #region Constructor
    public GridNode(Grid<GridNode> grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
        isWalkable = true;
    }
    #endregion

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
    public bool CanBuild()
    {
        return placedTransform == null;
    }

    public void SetTransform(Transform transform)
    {
        placedTransform = transform;
        GridEventManager.GridObjectChanged(this, x, y);
    }

    public int GetNeighborIndex()
    {
        bool east = false, west = false, north = false, south = false;
        
        GridNode checkNode;
        checkNode = grid.GetGridObject(x + 1, y);
        if(checkNode != null)
        {
            if(checkNode.GetTransform() != null)
            // check for matching "tile" type
                east = true;
        }
        checkNode = grid.GetGridObject(x - 1, y);
        if(checkNode != null)
        {
            if(checkNode.GetTransform() != null)
            // check for matching "tile" type
                west = true;
        }
        checkNode = grid.GetGridObject(x, y + 1);
        if(checkNode != null)
        {
            if(checkNode.GetTransform() != null)
            // check for matching "tile" type
                north = true;
        }
        checkNode = grid.GetGridObject(x, y - 1);
        if(checkNode != null)
        {
            if(checkNode.GetTransform() != null)
            // check for matching "tile" type
                south = true;
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
