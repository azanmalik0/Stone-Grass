using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyStack : Stacker
{
    void Start()
    {
        CalculateCellPositions();
        SetGridYOffset(gridOffset.y);

    }

}
