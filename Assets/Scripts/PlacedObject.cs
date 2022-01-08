using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacedObject : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    PlacedObjectScriptableObject objectData;

    [SerializeField]
    bool isBuilt = false;
    bool hasResources = false;
    Job_Build thisJob;
    List<GridNode> nodes;
    
    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();  
    }    

    public void UpdatePositionalSprite(int index)
    {
        if(!hasResources && !isBuilt)
        {
            spriteRenderer.sprite = objectData.sprites[index];
            UpdateSpriteOpacity();
            return;
        }

        if(hasResources && !isBuilt)
        {
            spriteRenderer.sprite = objectData.constructionSprite;
            return;
        }

        spriteRenderer.sprite = objectData.sprites[index];
    }

    private void UpdateSpriteOpacity()
    {
        if(isBuilt || hasResources)
            spriteRenderer.color = Color.white;
        else
            spriteRenderer.color = new Color(0.2f, 0.7f, 1f, 0.7f);
    }

    public Vector3 GetPosition()
    {
        return new Vector3(nodes[0].x, nodes[0].y, 0);
    }
    
    public void Initialize(List<GridNode> nodes, PlacedObjectScriptableObject SO, int x, int y)
    {
        this.nodes = nodes;
        objectData = SO;
        spriteRenderer.sortingOrder = -y;
        spriteRenderer.sprite = objectData.sprites[0];
        UpdateSpriteOpacity();
    }
}
