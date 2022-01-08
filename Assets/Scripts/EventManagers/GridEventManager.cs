using UnityEngine;

public class GridEventManager
{
    public delegate void GridEventHandler();
    public delegate void GridNodeEventHandler(object sender, int x, int y);

    public static event GridEventHandler OnGridComplete;
    public static event GridNodeEventHandler OnGridObjectChanged;

    public static void GridComplete(object sender)
    {
        Debug.Log("GridComplete() Event");
        if(OnGridComplete != null && sender is Grid) OnGridComplete();
    }
    public static void GridObjectChanged(object sender, int x, int y)
    {
        if(OnGridObjectChanged != null && sender is GridNode) OnGridObjectChanged(sender, x, y);
    }
}