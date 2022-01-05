using UnityEngine;

public class InputEventManager
{
    public delegate void MoveInputEventHandler(Vector3 moveDir);
    public delegate void MouseInputEventHandler(Vector3 position);
    public delegate void KeyboardInputEventHandler();

    public static event MoveInputEventHandler OnMove;
    public static event MouseInputEventHandler OnLeftClick;
    public static event MouseInputEventHandler OnRightClick;

    public static event KeyboardInputEventHandler OnRotateKey;

    public static void Move(object sender, Vector3 moveDir)
    {
        if(OnMove != null && !(moveDir.x == 0 && moveDir.y == 0) && sender.GetType() == typeof(InputController))
            OnMove(moveDir);
    }
    public static void LeftClick(object sender, Vector3 position)
    {
        Debug.Log(position);
        if(OnLeftClick != null && (position.x >= 0 && position.y >= 0) && sender.GetType() == typeof(InputController))
            OnLeftClick(position);
    }
    public static void RightClick(object sender, Vector3 position)
    {
        if(OnRightClick != null && (position.x >= 0 && position.y >= 0) && sender.GetType() == typeof(InputController))
            OnRightClick(position);
    }
    
    public static void Rotate(object sender)
    {
        if(OnRotateKey != null && sender.GetType() == typeof(InputController))
            OnRotateKey();
    }
}
