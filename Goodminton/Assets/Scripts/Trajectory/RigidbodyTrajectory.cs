using UnityEngine;

public class RigidbodyTrajectory : Trajectory
{
    private Rigidbody rb;
    protected override Vector3 InitialVelocity => rb ? rb.linearVelocity : Vector3.zero;

    private void Awake()
    {
        rb = GetComponentInParent<Rigidbody>();
    }
}
