using UnityEngine;

public class InputEventManager
{
    public delegate void MoveInputEventHandler(object sender, Vector3 moveDir);

    public static event MoveInputEventHandler OnMove;

    public static void Move(object sender, Vector3 moveDir)
    {
        if(OnMove != null && !(moveDir.x == 0 && moveDir.y == 0)) OnMove(sender, moveDir);
    }
}
