using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBuildingSystem : MonoBehaviour
{
    [SerializeField] private List<PlacedObjectScriptableObject> objectToPlaceList;
    [SerializeField] private Transform placementGhost;
    private SpriteRenderer ghostSpriteRenderer;
    [SerializeField] private PlacedObjectScriptableObject objectToPlace;
    private Grid<GridNode> grid;
    private PlacedObjectScriptableObject.Dir dir = PlacedObjectScriptableObject.Dir.South;
    [SerializeField] ResourceScriptableObject testResource;
    Vector3 mousePos; //testing?
    List<ResourceCost> resourceCosts;

    public void Initialize(Grid<GridNode> grid)
    {
        this.grid = grid;
    }

    private void Awake()
    {
        objectToPlace = objectToPlaceList[0];
        ghostSpriteRenderer = placementGhost.GetComponent<SpriteRenderer>();
        ghostSpriteRenderer.sprite = objectToPlace.sprites[0];
        
        InputEventManager.OnLeftClick += Input_TryPlace;
        //InputEventManager.OnRightClick += InputTest_ResourceUpdate;
        InputEventManager.OnRotateKey += Input_RotateObjectToPlace;
        InputEventManager.OnNumKey += Input_ChangeObjectToPlace;
    }

    private void Update()
    {
        mousePos = MouseUtils.GetMouseWorldPosition();
        placementGhost.position = grid.GetXY(mousePos);
    }

    private void Input_ChangeObjectToPlace(int numkey)
    {
        if(numkey < objectToPlaceList.Count)
        {
            objectToPlace = objectToPlaceList[numkey];            
            ghostSpriteRenderer.sprite = objectToPlace.sprites[0];
            resourceCosts = objectToPlace.ResourceCosts;
        }
    }

    private void Input_RotateObjectToPlace()
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
                
                List<Vector2Int> gridPositionList = objectToPlace.GetGridPositionList(new Vector2Int(x, y), dir);

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
                    Vector2Int rotationOffset = objectToPlace.GetRotationOffset(dir);
                    Vector3 objectToPlaceWorldPosition = grid.GetWorldPosition(x, y) + 
                        new Vector3(rotationOffset.x, rotationOffset.y) * grid.GetCellSize();
                    
                    List<GridNode> gridNodes = new List<GridNode>();
                    foreach(Vector2Int gridPosition in gridPositionList)
                    {
                        gridNodes.Add(grid.GetGridObject(gridPosition.x, gridPosition.y));
                    }

                    Transform builtTransform =
                        Instantiate(
                            objectToPlace.prefab,
                            objectToPlaceWorldPosition,
                            Quaternion.Euler(0, 0, objectToPlace.GetRotationAngle(dir))
                        ).transform;
                    PlacedObject placedObject = builtTransform.GetComponent<PlacedObject>();
                    placedObject.Initialize(gridNodes, objectToPlace, x, y);
                    
                    IJob thisJob = new Job_Build(placedObject, resourceCosts);
                    JobEventManager.AddJob(this, thisJob);

                    foreach(GridNode gridNode in gridNodes)
                    {
                        gridNode.SetTransform(builtTransform);
                        gridNode.SetIsWalkable(objectToPlace.hasBuildCost && objectToPlace.ResourceCosts.Count > 0 ? true : false);
                    }
                }
                else
                {
                    // "CANNOT BUILD
                }
            }
        }
    }
}
