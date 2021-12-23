using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMovement : MonoBehaviour
{
    private const float SPEED = 4f;
    private int currentPathIndex;
    List<Vector3> pathVectorList;

    private void Update()
    {
        HandleMovement();

        if (Input.GetMouseButtonDown(1))
        {
            SetTargetPosition(MathUtils.RoundDownVector3(MouseUtils.GetMouseWorldPosition()));
        }
    }
    
    private void HandleMovement()
    {
        if (pathVectorList != null)
        {
            Vector3 targetPosition = pathVectorList[currentPathIndex];
            if (Vector3.Distance(transform.position, targetPosition) > 0.1f)
            {
                Vector3 moveDir = (targetPosition - transform.position).normalized;

                float distanceBefore = Vector3.Distance(transform.position, targetPosition);
                transform.position = transform.position + moveDir * SPEED * Time.deltaTime;
            }
            else
            {
                currentPathIndex++;
                if (currentPathIndex >= pathVectorList.Count)
                {
                    StopMoving();
                }
            }
        }
    }
    private void StopMoving()
    {
        pathVectorList = null;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void SetTargetPosition(Vector3 targetPosition)
    {
        Debug.Log("moving to " + targetPosition);
        currentPathIndex = 0;
        pathVectorList = Pathfinding._Instance.FindPath(GetPosition(), targetPosition);

        if(pathVectorList != null && pathVectorList.Count > 1)
        {
            pathVectorList.RemoveAt(0);
        }
    }
}
