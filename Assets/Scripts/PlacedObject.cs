using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacedObject : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    PlacedObjectScriptableObject objectData;

    Grid<GridObject> grid;

    int x;
    int y;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();  
    }    
    private void Grid_OnGridValueChanged(object sender, int x, int y)
    {
        if(this.x >= x - 1 && this.y >= y - 1 && this.x <= x + 1 && this.y <= y + 1)
        {
            UpdateSprite();
        }
    }

    public void UpdateSprite()
    {
        spriteRenderer.sprite = objectData.sprites[grid.GetObject(x, y).GetNeighborIndex()];
    }

    public void Initialize(Grid<GridObject> grid, PlacedObjectScriptableObject SO, int x, int y)
    {
        this.x = x;
        this.y = y;
        this.grid = grid;
        GridEventManager.OnGridObjectChanged += Grid_OnGridValueChanged;
        objectData = SO;
        spriteRenderer.sortingOrder = -y;
        UpdateSprite(); 
    }
}
