using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingOld : MonoBehaviour
{
    private Grid grid;
    private void Start() {
        grid = new Grid(8, 4, 10, Vector3.zero);
    }

    private void Update()
    {
        if(Input.GetMouseButton(0))
        {
            grid.SetValue(MouseUtils.GetMouseWorldPosition(), grid.GetValue(MouseUtils.GetMouseWorldPosition())+8);
        }
        if(Input.GetMouseButton(1))
        {
            Debug.Log(grid.GetValue(MouseUtils.GetMouseWorldPosition()));
        }
    }
}
