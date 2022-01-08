using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IJob
{
    void Execute();
    void SetState(string stateName);
}
