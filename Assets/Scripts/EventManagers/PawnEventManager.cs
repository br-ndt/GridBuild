using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnEventManager
{
    public delegate void IdlePawnEventHandler(PawnBrain thisPawn);
    public static event IdlePawnEventHandler OnPawnIdle;

    public static void PawnIdle(object sender, PawnBrain thisPawn)
    {
        if(OnPawnIdle != null) OnPawnIdle(thisPawn);
    }
}
