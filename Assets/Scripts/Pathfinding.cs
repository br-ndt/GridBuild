using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding
{
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;
    public static Pathfinding _Instance { get; private set; }
    private Grid<GridNode> grid;
    private List<GridNode> openList;
    private List<GridNode> closedList;
    public Pathfinding(int width, int height, Grid<GridNode> grid)
    {
        _Instance = this;
        this.grid = grid;
        for(int i = 0; i < width; ++i)
        {
            for(int j = 0; j < height; ++j)
            {
                grid.GetGridObject(i, j).neighbors = GetNeighborList(grid.GetGridObject(i, j));
            }
        }
    }

    public List<Vector3> FindPath(Vector3 startPosition, Vector3 endPosition)
    {
        grid.GetXY(startPosition, out int startX, out int startY);
        grid.GetXY(endPosition, out int endX, out int endY);

        List<GridNode> path = FindPath(startX, startY, endX, endY);
        if(path == null)
            return null;
        else
        {
            List<Vector3> vectorPath = new List<Vector3>();
            foreach(GridNode pn in path)
            {
                vectorPath.Add(new Vector3(pn.x, pn.y) * grid.GetCellSize() + Vector3.one * grid.GetCellSize() * 0.5f);
            }
            return vectorPath;
        }
    }
    public List<GridNode> FindPath(int startX, int startY, int endX, int endY)
    {
        GridNode startNode = grid.GetGridObject(startX, startY);
        GridNode endNode = grid.GetGridObject(endX, endY);

        openList = new List<GridNode>{ startNode };
        closedList = new List<GridNode>();

        for(int x = 0; x < grid.GetWidth(); ++x)
        {
            for(int y = 0; y < grid.GetHeight(); ++y)
            {
                GridNode curNode = grid.GetGridObject(x, y);
                curNode.gCost = int.MaxValue;
                curNode.CalculateFCost();
                curNode.prevNode = null;
            }
        }

        startNode.gCost = 0;
        startNode.hCost = CalculateDistanceCost(startNode, endNode);
        startNode.CalculateFCost();

        while(openList.Count > 0)
        {
            GridNode curNode = GetLowestFCostNode(openList);
            if(curNode == endNode)
            {
                return CalculatePath(endNode);
            }

            openList.Remove(curNode);
            closedList.Add(curNode);

            foreach(GridNode neighborNode in curNode.neighbors)
            {
                if(closedList.Contains(neighborNode)) continue;
                if(!neighborNode.isWalkable)
                {
                    closedList.Add(neighborNode);
                    continue;
                }

                int tentativeGCost = curNode.gCost + CalculateDistanceCost(curNode, neighborNode);
                if(tentativeGCost < neighborNode.gCost)
                {
                    neighborNode.prevNode = curNode;
                    neighborNode.gCost = tentativeGCost;
                    neighborNode.hCost = CalculateDistanceCost(neighborNode, endNode);
                    neighborNode.CalculateFCost();

                    if(!openList.Contains(neighborNode))
                    {
                        openList.Add(neighborNode);
                    }
                }
            }
        }

        // Out of nodes in openList
        return null;
    }

    private List<GridNode> GetNeighborList(GridNode curNode)
    {
        List<GridNode> neighborList = new List<GridNode>();
        if(curNode.x - 1 >= 0)
        {
            //LEFT
            neighborList.Add(grid.GetGridObject(curNode.x - 1, curNode.y));
            //LEFTDOWN
            if(curNode.y - 1 >= 0) neighborList.Add(grid.GetGridObject(curNode.x - 1, curNode.y - 1));
            //LEFTUP
            if(curNode.y + 1 < grid.GetHeight()) neighborList.Add(grid.GetGridObject(curNode.x - 1, curNode.y + 1));
        }
        if(curNode.x + 1 < grid.GetWidth())
        {
            //RIGHT
            neighborList.Add(grid.GetGridObject(curNode.x + 1, curNode.y));
            //RIGHTDOWN
            if(curNode.y - 1 >= 0) neighborList.Add(grid.GetGridObject(curNode.x + 1, curNode.y - 1));
            //RIGHTUP
            if(curNode.y + 1 < grid.GetHeight()) neighborList.Add(grid.GetGridObject(curNode.x + 1, curNode.y + 1));
        }
        //DOWN
        if(curNode.y - 1 >= 0) neighborList.Add(grid.GetGridObject(curNode.x, curNode.y - 1));
        //UP
        if(curNode.y + 1 < grid.GetHeight()) neighborList.Add(grid.GetGridObject(curNode.x, curNode.y + 1));

        return neighborList;
    }

    private List<GridNode> CalculatePath(GridNode endNode)
    {
        List<GridNode> path = new List<GridNode>();
        path.Add(endNode);
        GridNode curNode = endNode;
        while(curNode.prevNode != null)
        {
            path.Add(curNode.prevNode);
            curNode = curNode.prevNode;
        }

        path.Reverse();
        return path;
    }

    private int CalculateDistanceCost(GridNode a, GridNode b)
    {
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);
        int remaining = Mathf.Abs(xDistance - yDistance);
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    private GridNode GetLowestFCostNode(List<GridNode> pathNodeList)
    {
        GridNode lowestFCostNode = pathNodeList[0];
        for(int i = 1; i < pathNodeList.Count; ++i)
        {
            if(pathNodeList[i].fCost < lowestFCostNode.fCost)
            {
                lowestFCostNode = pathNodeList[i];
            }
        }
        return lowestFCostNode;
    }
}
