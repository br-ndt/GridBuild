using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Job_Haul : IJob
{
    Vector3 resourceLocation;
    Vector3 destination;
    JobState jobState;
    bool isAssigned;
    public Job_Haul(Vector3 destination)
    {
        this.destination = destination;
        jobState = JobState.SeekingStockpile;
    }

    public void Execute()
    {
        switch(jobState)
        {
            case JobState.SeekingStockpile:
                // find stockpile
                return;
            case JobState.FetchingItem:
                // send pawn to resource location, put item in pawn inventory
                return;
            case JobState.BringingItem:
                // send pawn to stockpile location, remove item from pawn inventory, place in location
                return;
            case JobState.Complete:
                // done, don't need to do anything
                return;
        }
    }

    public void SetState(string stateName)
    {
        switch(stateName)
        {
            case "SeekingStockpile":
                jobState = JobState.SeekingStockpile;
                return;
            case "FetchingItem":
                jobState = JobState.FetchingItem;
                return;
            case "BringingItem":
                jobState = JobState.BringingItem;
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
        SeekingStockpile,
        FetchingItem,
        BringingItem,
        Complete
    }
}
