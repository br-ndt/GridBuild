using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding
{
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;
    public static Pathfinding _Instance { get; private set; }
    private Grid<PathNode> grid;
    private List<PathNode> openList;
    private List<PathNode> closedList;
    public Pathfinding(int width, int height, float cellSize)
    {
        _Instance = this;
        grid = new Grid<PathNode>(width, height, cellSize, Vector3.zero, (Grid<PathNode> g, int x, int y) => new PathNode(g, x, y), false, false, Color.clear);
        for(int i = 0; i < width; ++i)
        {
            for(int j = 0; j < height; ++j)
            {
                GetNode(i, j).neighbors = GetNeighborList(GetNode(i, j));
            }
        }
    }

    public Grid<PathNode> GetGrid()
    {
        return grid;
    }

    public PathNode GetNode(int x, int y)
    {
        return grid.GetObject(x, y);
    }

    public List<Vector3> FindPath(Vector3 startPosition, Vector3 endPosition)
    {
        grid.GetXY(startPosition, out int startX, out int startY);
        grid.GetXY(endPosition, out int endX, out int endY);

        List<PathNode> path = FindPath(startX, startY, endX, endY);
        if(path == null)
            return null;
        else
        {
            List<Vector3> vectorPath = new List<Vector3>();
            foreach(PathNode pn in path)
            {
                vectorPath.Add(new Vector3(pn.x, pn.y) * grid.GetCellSize() + Vector3.one * grid.GetCellSize() * 0.5f);
            }
            return vectorPath;
        }
    }
    public List<PathNode> FindPath(int startX, int startY, int endX, int endY)
    {
        PathNode startNode = grid.GetObject(startX, startY);
        PathNode endNode = grid.GetObject(endX, endY);

        openList = new List<PathNode>{ startNode };
        closedList = new List<PathNode>();

        for(int x = 0; x < grid.GetWidth(); ++x)
        {
            for(int y = 0; y < grid.GetHeight(); ++y)
            {
                PathNode curNode = grid.GetObject(x, y);
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
            PathNode curNode = GetLowestFCostNode(openList);
            if(curNode == endNode)
            {
                return CalculatePath(endNode);
            }

            openList.Remove(curNode);
            closedList.Add(curNode);

            foreach(PathNode neighborNode in curNode.neighbors)
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

    private List<PathNode> GetNeighborList(PathNode curNode)
    {
        List<PathNode> neighborList = new List<PathNode>();
        if(curNode.x - 1 >= 0)
        {
            //LEFT
            neighborList.Add(GetNode(curNode.x - 1, curNode.y));
            //LEFTDOWN
            if(curNode.y - 1 >= 0) neighborList.Add(GetNode(curNode.x - 1, curNode.y - 1));
            //LEFTUP
            if(curNode.y + 1 < grid.GetHeight()) neighborList.Add(GetNode(curNode.x - 1, curNode.y + 1));
        }
        if(curNode.x + 1 < grid.GetWidth())
        {
            //RIGHT
            neighborList.Add(GetNode(curNode.x + 1, curNode.y));
            //RIGHTDOWN
            if(curNode.y - 1 >= 0) neighborList.Add(GetNode(curNode.x + 1, curNode.y - 1));
            //RIGHTUP
            if(curNode.y + 1 < grid.GetHeight()) neighborList.Add(GetNode(curNode.x + 1, curNode.y + 1));
        }
        //DOWN
        if(curNode.y - 1 >= 0) neighborList.Add(GetNode(curNode.x, curNode.y - 1));
        //UP
        if(curNode.y + 1 < grid.GetHeight()) neighborList.Add(GetNode(curNode.x, curNode.y + 1));

        return neighborList;
    }

    private List<PathNode> CalculatePath(PathNode endNode)
    {
        List<PathNode> path = new List<PathNode>();
        path.Add(endNode);
        PathNode curNode = endNode;
        while(curNode.prevNode != null)
        {
            path.Add(curNode.prevNode);
            curNode = curNode.prevNode;
        }

        path.Reverse();
        return path;
    }

    private int CalculateDistanceCost(PathNode a, PathNode b)
    {
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);
        int remaining = Mathf.Abs(xDistance - yDistance);
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    private PathNode GetLowestFCostNode(List<PathNode> pathNodeList)
    {
        PathNode lowestFCostNode = pathNodeList[0];
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
