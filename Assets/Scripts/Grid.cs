using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Grid<TGridObject>
{

    Color debugColor;
    private int width;
    private int height;
    private float cellSize;
    private Vector3 originPosition;
    private TGridObject[,] gridArray;
    private TextMesh[,] debugTextArray;
    private Transform debug;
    bool gridDebug;
    bool textDebug;

    public Grid(int width, int height, float cellSize, Vector3 originPosition, Func<Grid<TGridObject>, int, int, TGridObject> createGridObjectFunc, Func<TGridObject, bool> initializeGridObjectFunc, Transform debug, bool gridDebug, bool textDebug, Color debugColor)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;
        this.debug = debug;
        this.debugColor = debugColor;
        GridEventManager.OnGridObjectChanged += Grid_GridObjectChanged;

        gridArray = new TGridObject[width, height];

        for(int x = 0; x < width; ++x)
        {
            for(int y = 0; y < height; ++y)
            {
                gridArray[x, y] = createGridObjectFunc(this, x, y);
            }
        }

        for(int x = 0; x < width; ++x)
        {
            for(int y = 0; y < height; ++y)
            {
                initializeGridObjectFunc(gridArray[x,y]);
            }
        }

        GridEventManager.GridComplete(this);

        if(gridDebug || textDebug)
        {
            DrawDebug(gridDebug, textDebug, width, height);
        }

    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize + originPosition;
    }

    public void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
    }

    public Vector3 GetXY(Vector3 worldPosition)
    {
        int x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        int y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
        return new Vector3(x, y, 0);
    }

    public int GetWidth()
    {
        return this.width;
    }

    public int GetHeight()
    {
        return this.height;
    }

    public float GetCellSize()
    {
        return this.cellSize;
    }

    /*public void SetObject(int x, int y, TGridObject obj)
    {
        if(x >= 0 && y >= 0 && x < width && y < height)
        {
            gridArray[x, y] = obj;
            if(textDebug)
                debugTextArray[x, y].text = gridArray[x, y].ToString();
        }
            
    }*/

    public void Grid_GridObjectChanged(object sender, int x, int y)
    {
        if(textDebug)
        {
            debugTextArray[x, y].text = gridArray[x, y].ToString();
            if(GetGridObject(x, y) != null) debugTextArray[x, y].color = Color.blue;
            else debugTextArray[x, y].color = debugColor;
        }        
    }

    /*public void SetObject(Vector3 worldPosition, TGridObject obj)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        SetObject(x, y, obj);
    }*/

    public TGridObject GetGridObject(int x, int y)
    {
        if(x >= 0 && y >= 0 && x < width && y < height)
        {
            return gridArray[x, y];
        }
        else
        {
            return default(TGridObject);
        }
    }

    public TGridObject GetGridObject(Vector3 worldPosition)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        return GetGridObject(x, y);
    }

    private void DrawDebug(bool grid, bool text, int width, int height)
    {
        for(int x = 0; x < width; ++x)
        {
            for(int y = 0; y < height; ++y)
            {
                if(text)
                {                        
                    debugTextArray = new TextMesh[width, height];
                    debugTextArray[x, y] = TextUtils.CreateWorldText(gridArray[x, y]?.ToString(), debug, GetWorldPosition(x, y) + new Vector3(cellSize/2, cellSize/2, -1), 8, debugColor, TextAnchor.MiddleCenter);
                    debugTextArray[x, y].characterSize = 0.5f;
                }
                if(grid)
                {
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), debugColor, 100f);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), debugColor, 100f);

                }
            }
        }
        if(grid)
        {
            Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), debugColor, 100f);
            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), debugColor, 100f);
        }
    }
}
