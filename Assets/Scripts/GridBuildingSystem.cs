using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBuildingSystem : MonoBehaviour
{
    [SerializeField] private List<PlacedObjectScriptableObject> placedObjectList;
    private PlacedObjectScriptableObject placedObject;
    private Grid<GridNode> grid;
    private PlacedObjectScriptableObject.Dir dir = PlacedObjectScriptableObject.Dir.South;
    [SerializeField] ResourceScriptableObject testResource;

    private void Awake()
    {
        placedObject = placedObjectList[0];
        InputEventManager.OnLeftClick += Input_TryPlace;
        InputEventManager.OnRightClick += InputTest_ResourceUpdate;
        InputEventManager.OnRotateKey += Input_RotateObject;
    }

    public void Initialize(Grid<GridNode> grid)
    {
        this.grid = grid;
    }

    private void Input_RotateObject()
    {
        dir = PlacedObjectScriptableObject.GetNextDir(dir);
    }

    private void Input_TryPlace(Vector3 position)
    {
        if(grid != null)
        {
            grid.GetXY(position, out int x, out int y);
            if(x >= 0 && y >= 0)
            {
                
                List<Vector2Int> gridPositionList = placedObject.GetGridPositionList(new Vector2Int(x, y), dir);

                bool canPlace = true;
                foreach(Vector2Int gridPosition in gridPositionList)
                {
                    if(!grid.GetGridObject(gridPosition.x, gridPosition.y).PlacementAvailable())
                    {
                        canPlace = false;
                        break;
                    }
                }

                if(canPlace)
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
                    builtTransform.GetComponent<PlacedObject>().Initialize(grid, placedObject, x, y, out bool isBuilt);
                    foreach(Vector2Int gridPosition in gridPositionList)
                    {
                        grid.GetGridObject(gridPosition.x, gridPosition.y).SetTransform(builtTransform);
                        if(isBuilt)
                            grid.GetGridObject(gridPosition.x, gridPosition.y).SetIsWalkable(false);
                    }
                }
                else
                {
                    // "CANNOT BUILD
                }
            }
        }
    }

    private void InputTest_ResourceUpdate(Vector3 position)
    {
        Transform thing = grid.GetGridObject(position).GetTransform();
        if(thing != null)
        {
            PlacedObject placo = thing.GetComponent<PlacedObject>();
            if(placo != null)
            {
                placo.UpdateResource(testResource, 100);
            }
        }        
    }
    private void Update()
    {
        //TESTING
        if(Input.GetKeyDown(KeyCode.Alpha1)){ placedObject = placedObjectList[0]; }
        if(Input.GetKeyDown(KeyCode.Alpha2)){ placedObject = placedObjectList[1]; }
        if(Input.GetKeyDown(KeyCode.Alpha3)){ placedObject = placedObjectList[2]; }
    }
}
