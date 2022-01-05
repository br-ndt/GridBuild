using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacedObject : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    PlacedObjectScriptableObject objectData;

    Grid<GridNode> grid;

    int x;
    int y;

    [SerializeField]
    bool isBuilt = false;
    bool hasResources = false;
    List<ResourceCost> resourceCosts;

    private bool AssessPreBuiltStatus()
    {        
        if(!objectData.hasBuildCost)
        {
            hasResources = true;
            resourceCosts = null;
            return true;
        }
        else
        {
            resourceCosts = objectData.ResourceCosts;
            return false;
        }
    }
    private void AssessResourceStatus()
    {
        foreach(ResourceCost r in resourceCosts)
        {
            if(r.investedCount < r.neededCount && r.resource.resourceName != "Seconds")
            {
                hasResources = false;
                return;
            }
        }
        hasResources = true;
        UpdatePositionalSprite();
        UpdateSpriteOpacity();
    }
    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();  
    }    
    private void Grid_OnGridValueChanged(int x, int y)
    {
        if(this.x >= x - 1 && this.y >= y - 1 && this.x <= x + 1 && this.y <= y + 1)
        {
            UpdatePositionalSprite();
        }
    }

    private void UpdatePositionalSprite()
    {
        if(hasResources && !isBuilt)
        {
            spriteRenderer.sprite = objectData.constructionSprite;
            return;
        }
        spriteRenderer.sprite = objectData.sprites[grid.GetGridObject(x, y).GetNeighborIndex()];
    }

    private void UpdateSpriteOpacity()
    {
        if(isBuilt || hasResources)
            spriteRenderer.color = Color.white;
        else
            spriteRenderer.color = new Color(0.2f, 0.7f, 1f, 0.7f);
    }

    public void Initialize(Grid<GridNode> grid, PlacedObjectScriptableObject SO, int x, int y, out bool isBuilt)
    {
        this.x = x;
        this.y = y;
        this.grid = grid;
        GridEventManager.OnGridObjectChanged += Grid_OnGridValueChanged;
        objectData = SO;
        spriteRenderer.sortingOrder = -y;
        isBuilt = AssessPreBuiltStatus();
        UpdatePositionalSprite(); 
        UpdateSpriteOpacity();
    }

    public void UpdateResource(ResourceScriptableObject resource, int count)
    {
        ResourceCost curResource = resourceCosts.Find(r => r.resource == resource);
        curResource.investedCount += count;
        if(curResource.investedCount >= curResource.neededCount)
        {
            AssessResourceStatus();
        }        
    }
}
