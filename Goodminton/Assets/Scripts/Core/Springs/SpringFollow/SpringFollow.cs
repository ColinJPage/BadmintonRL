using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Springs;

public abstract class SpringFollow : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Vector3Spring spring;

    public Vector3Spring Spring => spring;

    private void OnEnable()
    {
        InitializeSpring();
    }
    public void InitializeSpring()
    {
        if (!target) return;
        spring.SetState(target.position, Vector3.zero);
    }
    public void Follow(float delta)
    {
        if (!target) return;
        transform.position = spring.Update(delta, target.position);
    }
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;

    }
}
