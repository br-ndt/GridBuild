using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBuildingSystem : MonoBehaviour
{
    [SerializeField] private List<PlacedObjectScriptableObject> placedObjectList;
    private PlacedObjectScriptableObject placedObject;
    private Grid<GridNode> grid;
    private PlacedObjectScriptableObject.Dir dir = PlacedObjectScriptableObject.Dir.South;

    private void Awake()
    {
        placedObject = placedObjectList[0];
    }

    public void Initialize(Grid<GridNode> grid)
    {
        this.grid = grid;
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0) && grid != null)
        {
            grid.GetXY(MouseUtils.GetMouseWorldPosition(), out int x, out int y);
            if(x >= 0 && y >= 0)
            {
                
                List<Vector2Int> gridPositionList = placedObject.GetGridPositionList(new Vector2Int(x, y), dir);

                bool canBuild = true;
                foreach(Vector2Int gridPosition in gridPositionList)
                {
                    if(!grid.GetGridObject(gridPosition.x, gridPosition.y).CanBuild())
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
                        grid.GetGridObject(gridPosition.x, gridPosition.y).SetTransform(builtTransform);
                        grid.GetGridObject(gridPosition.x, gridPosition.y).SetIsWalkable(false);
                    }
                }
                else
                {
                    // "CANNOT BUILD
                }
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
