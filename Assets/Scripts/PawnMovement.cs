using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnMovement : MonoBehaviour
{
    private int currentPathIndex;
    private float moveSpeed;
    List<Vector3> pathVectorList;

    private void Awake()
    {
        //   test
        InputEventManager.OnRightClick += SetTargetPosition;
        moveSpeed = GetComponent<PawnStats>().moveSpeed;
    }
    private void Update()
    {
        HandleMovement();
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
                transform.position = transform.position + moveDir * moveSpeed * Time.deltaTime;
            }
            else
            {
                currentPathIndex++;
                if (currentPathIndex >= pathVectorList.Count)
                {
                    StopMoving();
                    // PawnEventManager.PawnArrived(this);
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
        targetPosition = MathUtils.RoundDownVector3(targetPosition);
        currentPathIndex = 0;
        pathVectorList = Pathfinding._Instance.FindPath(GetPosition(), targetPosition);

        if(pathVectorList != null && pathVectorList.Count > 1)
        {
            pathVectorList.RemoveAt(0);
        }
    }
}
