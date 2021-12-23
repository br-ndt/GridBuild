using UnityEngine;

public class InputController : MonoBehaviour
{
    void HandleMovement()
	{
		float moveX = 0f;
        float moveY = 0f;

        if (Input.GetKey(KeyCode.W))
        {
            moveY = 1f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveX = -1f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveY = -1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveX = 1f;
        }

        Vector3 baseMoveDir = new Vector3(moveX, moveY, 0).normalized;
        InputEventManager.Move(this, baseMoveDir);
	}

    private void Update()
    {
        HandleMovement();
    }
}