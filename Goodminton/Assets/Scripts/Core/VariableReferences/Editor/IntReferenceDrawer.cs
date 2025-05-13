using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR

[CustomPropertyDrawer(typeof(IntReference))]
public class IntReferenceDrawer : ReferenceDrawer
{
}
#endif