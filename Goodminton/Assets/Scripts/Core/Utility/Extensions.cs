using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

public static class Extensions
{
    public static void DestroyChildren(this Transform transform)
    {
        for (int c = transform.childCount - 1; c >= 0; --c)
        {
            var go = transform.GetChild(c).gameObject;

#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                UnityEditor.EditorApplication.delayCall += () =>
                {
                    Object.DestroyImmediate(go);
                };
                continue;
            }
#endif
            Object.Destroy(go);

        }
    }

    public static GameObject InstantiatePrefab(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent)
    {
        GameObject go;
#if UNITY_EDITOR
        go = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
        go.transform.position = position;
        go.transform.rotation = rotation;
        go.transform.SetParent(parent, true);
#else
        go = Object.Instantiate(prefab, position, rotation, parent);
#endif
        return go;
    }
    public static void SetParentFlushly(this RectTransform child, RectTransform parent)
    {
        child.SetParent(parent, false); 
        child.anchorMin = Vector2.zero;
        child.anchorMax = Vector2.one;
        child.offsetMin = Vector2.zero;
        child.offsetMax = Vector2.zero;
    }
    public static Collider[] OverlapCapsule(this CapsuleCollider collider)
    {
        Vector3 center = collider.transform.position + collider.center;
        Vector3 axis = Vector3.zero;
        axis[collider.direction] = (collider.height - 2f * collider.radius) * 0.5f;
        return Physics.OverlapCapsule(center + axis, center - axis, collider.radius);
    }
    public static void Invoke(this MonoBehaviour mb, System.Action f, float delay)
    {
        mb.StartCoroutine(InvokeRoutine(f, delay));
    }

    private static IEnumerator InvokeRoutine(System.Action f, float delay)
    {
        yield return new WaitForSeconds(delay);
        f();
    }
}
