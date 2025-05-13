using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorExtensions
{
    public static Vector2 V3toV2(this Vector3 v3)
    {
        return new Vector2(v3.x, v3.y);
    }
    public static Vector3 HorizontalV2toV3(this Vector2 v2)
    {
        return new Vector3(v2.x, 0f, v2.y);
    }
    public static Vector2 ToHorizontalV2(this Vector3 v3)
    {
        return new Vector2(v3.x, v3.z);
    }
    public static Vector3 Flatten(this Vector3 v3)
    {
        return new Vector3(v3.x, 0f, v3.z);
    }
    public static Vector3 FlattenZ(this Vector3 v3)
    {
        return new Vector3(v3.x, v3.y, 0f);
    }
}
