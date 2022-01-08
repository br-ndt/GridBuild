
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnBrain : MonoBehaviour
{
    IJob currentJob;
    PawnState currentState = PawnState.Idle;

    private void Start()
    {
        InvokeRepeating("CheckState", 0f, 0.5f);
    }

    private void CheckState()
    {
        if(currentState == PawnState.Idle)
        {
            PawnEventManager.PawnIdle(this, this);
        }
    }

    public void SetJob(IJob jobToSet)
    {
        currentJob = jobToSet;
        currentJob.Execute();
    }

}

public enum PawnState
{
    Idle,
    Moving,
    Working,
    Sleeping,
    Wounded,
    Dead
}
