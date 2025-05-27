using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[ExecuteAlways]
[RequireComponent(typeof(LineRenderer))]
public abstract class Trajectory : MonoBehaviour
{
    LineRenderer lineRenderer;
    [Tooltip("How many seconds ahead to predict")]
    [SerializeField] float maxPredictionTime = 5f;

    [Tooltip("How thick the object is. It is better to overestimate here.")]
    [Min(0.01f)]
    [SerializeField] float objectRadius = 0.3f;

    [SerializeField] Transform endTarget;
    
    [SerializeField] LayerMask collisionMask; // = LayerMask.GetMask("Default", "Ground", "Clone");

    [SerializeField]  float gameTimeBetweenVertices = 0.2f;

    public Event UpdateEvent { get; } = new Event();
    public List<Vector3> linePoints { get; private set; }
    private Vector3 targetNormal = Vector3.up;

    protected virtual Vector3 InitialVelocity {
        get
        {
            return Vector3.forward;
        }
    }
    protected virtual float Drag => 0f;
    protected virtual Vector3 Gravity
    {
        get
        {
            return Physics.gravity;
        }
    }
    
    protected virtual void OnEnable()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void LateUpdate()
    {
        UpdateLine();
    }

    void UpdateLine()
    {
        linePoints = CalculateLinePoints();
        lineRenderer.positionCount = linePoints.Count;
        for(int i = 0; i < linePoints.Count; ++i)
        {
            lineRenderer.SetPosition(i, linePoints[i]);
        }

        if (endTarget)
        {
            endTarget.position = linePoints[linePoints.Count - 1];

            var forward = Vector3.ProjectOnPlane(InitialVelocity, Vector3.zero);
            var right = Vector3.Cross(targetNormal, forward);
            endTarget.rotation = Quaternion.LookRotation(Quaternion.AngleAxis(90f, right) * targetNormal, targetNormal);
        }


        UpdateEvent.Trigger();
    }
    public virtual Vector3 StartPosition()
    {
        return transform.position;
    }
    public virtual int PhysicsStepsForFirstVertex(int physicsStepsPerVertex)
    {
        return physicsStepsPerVertex;
    }

    List<Vector3> CalculateLinePoints()
    {
        var points = new List<Vector3>();
        float time = 0f;

        //var launchTransform = transform;

        Vector3 startPosition = StartPosition();
        Vector3 initialVelocity = InitialVelocity;

        Vector3 position = startPosition;
        Vector3 nextPosition;
        Vector3 acceleration = Gravity;

        points.Add(position);
        
        // the margin every physics frame between the physically correct future position and unity's discretely calculated position
        Vector3 errorOffsetPerPhysicsStep = Mathf.Pow(Time.fixedDeltaTime, 2f) * acceleration * 0.5f;

        int physicsStepsPerVertex = Mathf.Max(1, Mathf.FloorToInt(gameTimeBetweenVertices / Mathf.Max(0.0001f, Time.fixedDeltaTime)));

        // How many fixed updates were skipped between this vertex and the last
        int fixedUpdateSteps = PhysicsStepsForFirstVertex(physicsStepsPerVertex);
        int currentPhysicsStepIndex = 0;

        while(time < maxPredictionTime)
        {
            time = Mathf.Min(time + Time.fixedDeltaTime*fixedUpdateSteps, maxPredictionTime);

            //nextPosition += simulationVelocity * thisStep + 0.5f * acceleration * Mathf.Pow(thisStep, 2f);

            currentPhysicsStepIndex += fixedUpdateSteps;
            nextPosition = startPosition + initialVelocity * time + 0.5f * acceleration * Mathf.Pow(time, 2f); //mathematically accurate
            nextPosition += errorOffsetPerPhysicsStep * currentPhysicsStepIndex; // compensate for discrete unity physics steps
            
            fixedUpdateSteps = physicsStepsPerVertex;

            RaycastHit hit;
            var segment = nextPosition - position;
            var ray = new Ray(position, segment);

            if (Physics.SphereCast(ray, objectRadius, out hit, segment.magnitude, collisionMask, QueryTriggerInteraction.Ignore))
            {
                points.Add(hit.point);
                targetNormal = hit.normal;
                break;
            }
            else
            {
                points.Add(nextPosition);
                position = nextPosition;
            }
        }

        return points;
    }
}
