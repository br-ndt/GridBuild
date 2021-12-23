using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject
{
    private Grid<GridObject> grid;
    private int x;
    private int y;
    private Transform transformHere;
    private PathNode node;

    public GridObject(Grid<GridObject> grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
    }

    public Transform GetTransform()
    {
        return transformHere;
    }

    public void SetTransform(Transform transform)
    {
        this.transformHere = transform;
        GridEventManager.GridObjectChanged(this, x, y);
    }

    public void ClearTransform()
    {
        transformHere = null;
    }

    public bool CanBuild()
    {
        return transformHere == null;
    }

    public int GetNeighborIndex()
    {
        bool east = false, west = false, north = false, south = false;
        
        GridObject testObj;
        testObj = grid.GetObject(x + 1, y);
        if(testObj != null)
        {
            if(testObj.GetTransform() != null)
            // check for matching "tile" type
                east = true;
        }
        testObj = grid.GetObject(x - 1, y);
        if(testObj != null)
        {
            if(testObj.GetTransform() != null)
            // check for matching "tile" type
                west = true;
        }
        testObj = grid.GetObject(x, y + 1);
        if(testObj != null)
        {
            if(testObj.GetTransform() != null)
            // check for matching "tile" type
                north = true;
        }
        testObj = grid.GetObject(x, y - 1);
        if(testObj != null)
        {
            if(testObj.GetTransform() != null)
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

    public override string ToString()
    {
        return x + ", " + y + "\n";
    }
}
