using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItem
{
    void Drop(Vector3 worldPosition);
    void Pickup(PawnInventory pawn);
}
