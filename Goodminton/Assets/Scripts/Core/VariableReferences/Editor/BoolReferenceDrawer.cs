using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR

[CustomPropertyDrawer(typeof(BoolReference))]
public class BoolReferenceDrawer : ReferenceDrawer
{
}
#endif