using UnityEngine;

public class InputEventManager
{
    #region Movement Keys
    public delegate void MoveInputEventHandler(Vector3 moveDir);
    public static event MoveInputEventHandler OnMove;
    public static void Move(object sender, Vector3 moveDir)
    {
        if(OnMove != null && !(moveDir.x == 0 && moveDir.y == 0) && sender.GetType() == typeof(InputController))
            OnMove(moveDir);
    }
    #endregion

    #region Mouse
    public delegate void MouseInputEventHandler(Vector3 position);

    public static event MouseInputEventHandler OnLeftClick;
    public static void LeftClick(object sender, Vector3 position)
    {
        if(OnLeftClick != null && (position.x >= 0 && position.y >= 0) && sender.GetType() == typeof(InputController))
            OnLeftClick(position);
    }

    public static event MouseInputEventHandler OnRightClick;    
    public static void RightClick(object sender, Vector3 position)
    {
        if(OnRightClick != null && (position.x >= 0 && position.y >= 0) && sender.GetType() == typeof(InputController))
            OnRightClick(position);
    } 
    #endregion

    #region Other Keyboard
    public delegate void KeyboardInputEventHandler();
    public static event KeyboardInputEventHandler OnRotateKey;
    public static void Rotate(object sender)
    {
        if(OnRotateKey != null && sender.GetType() == typeof(InputController))
            OnRotateKey();
    }

    public delegate void NumKeyInputEventHandler(int num);
    public static event NumKeyInputEventHandler OnNumKey;
    public static void NumKey(object sender, int numkey) // eventually might want per-number basis
    {
        if(OnNumKey != null && sender.GetType() == typeof(InputController))
            OnNumKey(numkey);
    }
    #endregion    
}