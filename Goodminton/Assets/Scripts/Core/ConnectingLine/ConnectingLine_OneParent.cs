using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class ConnectingLine_OneParent : ConnectingLine
{
    [SerializeField] Transform transformParent;

    protected override List<Vector3> GetPositions()
    {
        var positions = new List<Vector3>();
        for(int i = 0; i < positions.Count; ++i)
        {
            positions.Add(transformParent.GetChild(i).position);
        }
        return positions;
    }
}

