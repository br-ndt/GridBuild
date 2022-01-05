using UnityEngine;

public class GridEventManager
{
    public delegate void GridEventHandler(int x, int y);

    public static event GridEventHandler OnGridObjectChanged;

    public static void GridObjectChanged(object sender, int x, int y)
    {
        if(OnGridObjectChanged != null) OnGridObjectChanged(x, y);
    }
}