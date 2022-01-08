using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Job_Build : IJob
{
    PlacedObject buildTarget;
    Vector3 targetLocation;
    JobState jobState;
    List<ResourceCost> resourceCosts;
    bool isAssigned;
    public Job_Build(PlacedObject buildTarget, List<ResourceCost> resourceCosts)
    {
        this.buildTarget = buildTarget;
        this.targetLocation = buildTarget.GetPosition();
        this.resourceCosts = resourceCosts;
        jobState = JobState.AwaitingResource;
    }

    public void Execute()
    {
        switch(jobState)
        {
            case JobState.AwaitingResource:
                // check if resources available, if not, wait to execute
                return;
            case JobState.FetchingResource:
                // fetch resource, bring it to jobsite
                return;
            case JobState.LaborPending:
                // go to jobsite
                return;
            case JobState.LaborOccurring:
                // stand by jobsite, do build checks
                return;
            case JobState.Complete:
                // done, don't need to do anything
                return;
        }
        // locate buildable object
        // move AI to buildable object
        // every X seconds, do a skill tick
        // if critical success, Y labor cost to buildable
        // if success, 1 labor cost added to buildable
        // if failure, 0 labor cost to buildable
        // if critical failure, labor cost reset to 0, chance to reduce resources
    }

    public void SetState(string stateName)
    {
        switch(stateName)
        {
            case "AwaitingResource":
                jobState = JobState.AwaitingResource;
                return;
            case "FetchingResource":
                jobState = JobState.FetchingResource;
                return;
            case "LaborPending":
                jobState = JobState.LaborPending;
                return;
            case "LaborOccurring":
                jobState = JobState.LaborOccurring;
                return;
            case "Complete":
                jobState = JobState.Complete;
                return;
            default:
                return;
        }
    }

    enum JobState
    {
        AwaitingResource,
        FetchingResource,
        LaborPending,
        LaborOccurring,
        Complete
    }
}
