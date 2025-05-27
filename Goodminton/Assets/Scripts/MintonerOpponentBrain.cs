using UnityEngine;

public class MintonerOpponentBrain : MonoBehaviour
{
    private Mintoner mintoner;
    private Birdie birdie;
    [SerializeField] Transform seekT;

    private void Awake()
    {
        mintoner = GetComponent<Mintoner>();
        birdie = transform.parent.GetComponentInChildren<Birdie>();
    }
    private void FixedUpdate()
    {
        var intendedOffsetFromBirdie = transform.forward;
        var targetPos = seekT.position - intendedOffsetFromBirdie;
        var toTarget = targetPos - mintoner.transform.position;
        mintoner?.SetMoveInput(toTarget.Flatten().ToHorizontalV2());
    }
    //Vector3 PredictLandingPos(Rigidbody rb)
    //{
    //    Vector3 simPos = rb.position;
    //    Vector3 simVelocity = rb.linearVelocity;
    //    float deltaTime = Time.fixedDeltaTime;
    //    while (true)
    //    {
    //        //simVelocity
    //    }
    //    return simPos;
    //}
}
