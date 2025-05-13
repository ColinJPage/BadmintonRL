using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class ConnectingLine_Collection : ConnectingLine
{
    [SerializeField] Transform[] transforms;

    public void SetTransforms(Transform[] newTransforms)
    {
        transforms = newTransforms;
    }

    protected override List<Vector3> GetPositions()
    {
        var positions = new List<Vector3>();
        for(int i = 0; i < transforms.Length; ++i)
        {
            positions.Add(transforms[i].position);
        }
        return positions;
    }
}
