using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class PlacedObjectScriptableObject : ScriptableObject
{
    #region Fields/Properties
    public enum Dir
    {
        North,
        South,
        East,
        West
    }

    public string nameString;
    public Transform prefab;
    public Transform visual;

    public Sprite[] sprites;
    public Sprite constructionSprite;
    public int width;
    public int height;
    
    public bool lockRotate;
    public bool hasBuildCost;
    public int minimumSkill;
    public int laborCost;

    public List<ResourceCost> ResourceCosts = new List<ResourceCost>();
    #endregion

    #region Methods    
    public static Dir GetNextDir(Dir dir)
    {
        switch(dir)
        {
            default:
            case Dir.South: return Dir.West;
            case Dir.West: return Dir.North;
            case Dir.North: return Dir.East;
            case Dir.East: return Dir.South;
        }
    }
    public int GetRotationAngle(Dir dir)
    {
        if(!lockRotate)
        {
            switch(dir)
            {
                default:
                case Dir.South:
                case Dir.North: return 0;
                case Dir.West:
                case Dir.East: return 90;
            }
        }
        else
            return 0;
    }

    public Vector2Int GetRotationOffset(Dir dir)
    {
        if(!lockRotate)
        {
            switch(dir)
            {
                default:
                case Dir.South: return new Vector2Int(0, 0);
                case Dir.West: return new Vector2Int(0, width);
                case Dir.North: return new Vector2Int(width, height);
                case Dir.East: return new Vector2Int(height, 0);
            }
        }
        else
            return new Vector2Int(0, 0);
    }

    public List<Vector2Int> GetGridPositionList(Vector2Int offset, Dir dir)
    {
        List<Vector2Int> gridPositionList = new List<Vector2Int>();
        if(!lockRotate)
        {
            switch(dir)
            {
                default:
                case Dir.South:
                case Dir.North:
                    for(int x = 0; x < width; ++x)
                    {
                        for(int y = 0; y < height; ++y)
                        {
                            gridPositionList.Add(offset + new Vector2Int(x, y));
                        }
                    }
                    break;
                case Dir.West:
                case Dir.East:
                    for(int x = 0; x < height; ++x)
                    {
                        for(int y = 0; y < width; ++y)
                        {
                            gridPositionList.Add(offset + new Vector2Int(x, y));
                        }
                    }
                    break;
            }
        }
        else
        {
            for(int x = 0; x < width; ++x)
            {
                for(int y = 0; y < height; ++y)
                {
                    gridPositionList.Add(offset + new Vector2Int(x, y));
                }
            }
        }
        return gridPositionList;
    }
    #endregion
}