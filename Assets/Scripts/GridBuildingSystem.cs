using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBuildingSystem : MonoBehaviour
{
    [SerializeField] private List<PlacedObjectScriptableObject> placedObjectList;
    private PlacedObjectScriptableObject placedObject;
    [SerializeField] private int gridWidth = 10;
    [SerializeField] private int gridHeight = 10;
    [SerializeField] float cellSize = 10f;
    [SerializeField] bool gridDebug;
    [SerializeField] bool textDebug;
    [SerializeField] Color debugColor;
    private Grid<GridObject> grid;
    Pathfinding pathfinding;
    private PlacedObjectScriptableObject.Dir dir = PlacedObjectScriptableObject.Dir.South;

    private void Awake()
    {
        grid = new Grid<GridObject>(gridWidth, gridHeight, cellSize, Vector3.zero, (Grid<GridObject> g, int x, int y) => new GridObject(g, x, y), gridDebug, textDebug, debugColor);
        pathfinding = new Pathfinding(gridWidth, gridHeight, cellSize);
        placedObject = placedObjectList[0];
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            grid.GetXY(MouseUtils.GetMouseWorldPosition(), out int x, out int y);

            List<Vector2Int> gridPositionList = placedObject.GetGridPositionList(new Vector2Int(x, y), dir);

            bool canBuild = true;
            foreach(Vector2Int gridPosition in gridPositionList)
            {
                if(!grid.GetObject(gridPosition.x, gridPosition.y).CanBuild())
                {
                    canBuild = false;
                    break;
                }
            }

            if(canBuild)
            {
                Vector2Int rotationOffset = placedObject.GetRotationOffset(dir);
                Vector3 placedObjectWorldPosition = grid.GetWorldPosition(x, y) + 
                    new Vector3(rotationOffset.x, rotationOffset.y) * grid.GetCellSize();

                Transform builtTransform =
                    Instantiate(
                        placedObject.prefab,
                        grid.GetWorldPosition(x, y),
                        Quaternion.Euler(0, 0, placedObject.GetRotationAngle(dir))
                    ).transform;
                builtTransform.GetComponent<PlacedObject>().Initialize(grid, placedObject, x, y);
                foreach(Vector2Int gridPosition in gridPositionList)
                {
                    grid.GetObject(gridPosition.x, gridPosition.y).SetTransform(builtTransform);
                    pathfinding.GetGrid().GetObject(gridPosition.x, gridPosition.y).SetIsWalkable(false);
                }
            }
            else
            {
                // "CANNOT BUILD
            }
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            dir = PlacedObjectScriptableObject.GetNextDir(dir);
        }

        if(Input.GetKeyDown(KeyCode.Alpha1)){ placedObject = placedObjectList[0]; }
        if(Input.GetKeyDown(KeyCode.Alpha2)){ placedObject = placedObjectList[1]; }
        if(Input.GetKeyDown(KeyCode.Alpha3)){ placedObject = placedObjectList[2]; }
    }
}
