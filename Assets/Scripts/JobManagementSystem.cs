using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

public class JobManagementSystem : MonoBehaviour
{
    public ConcurrentQueue<IJob> jobQueue;

    private void Awake()
    {
        jobQueue = new ConcurrentQueue<IJob>();
        JobEventManager.OnAddJob += AddJob;
        PawnEventManager.OnPawnIdle += AssignJob;
    }

    private void AddJob(IJob jobToAdd)
    {
        jobQueue.Enqueue(jobToAdd);
    }

    private void AssignJob(PawnBrain curPawn)
    {
        if(jobQueue.TryDequeue(out IJob job))
        {
            curPawn.SetJob(job);
        }
    }
}
