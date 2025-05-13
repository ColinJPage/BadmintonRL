using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR

[CustomPropertyDrawer(typeof(GameObjectReference))]
public class GameObjectReferenceDrawer : ReferenceDrawer
{
}
#endif