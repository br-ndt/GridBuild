using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JobEventManager
{
    public delegate void AddJobEventHandler(IJob jobToAdd);

    public static event AddJobEventHandler OnAddJob;

    public static void AddJob(object sender, IJob jobToAdd)
    {
        if(OnAddJob != null && jobToAdd != null)
        {
            OnAddJob(jobToAdd);
        }
    }
}
