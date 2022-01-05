using UnityEngine;

public class InputController : MonoBehaviour
{
    void HandleMovement()
	{
		float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        if(moveX != 0 || moveY != 0)
        {            
            Vector3 baseMoveDir = new Vector3(moveX, moveY, 0).normalized;
            InputEventManager.Move(this, baseMoveDir);
        }
	}

    void HandleMouse()
    {
        if(Input.GetMouseButtonDown(0) && !Input.GetMouseButton(1))
        {
            InputEventManager.LeftClick(this, MouseUtils.GetMouseWorldPosition());
        }
        if(Input.GetMouseButtonDown(1) && !Input.GetMouseButton(0))
        {
            InputEventManager.RightClick(this, MouseUtils.GetMouseWorldPosition());
        }
    }

    private void Update()
    {
        HandleMovement();
        HandleMouse();
    }
}