using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public abstract class ConnectingLine : MonoBehaviour
{
    [SerializeField] Vector3 offset;
    [SerializeField] Space space = Space.World;

    [SerializeField] LineRenderer lineRenderer;

    private void OnValidate()
    {
        if(!lineRenderer) lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.useWorldSpace = space == Space.World;
        UpdateLine();
    }

    private void Start()
    {
        if (!lineRenderer) lineRenderer = GetComponent<LineRenderer>();
    }

    private void LateUpdate()
    {
        UpdateLine();
    }

    protected abstract List<Vector3> GetPositions();

    void UpdateLine()
    {
        var positions = GetPositions();
        lineRenderer.positionCount = positions.Count;
        Vector3 pos;
        for (int i = 0; i < positions.Count; ++i)
        {
            pos = (space == Space.World ? positions[i] : transform.InverseTransformPoint(positions[i])) + offset;
            lineRenderer.SetPosition(i, pos);
        }
    }
}
