using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class BadmintonAgent : Agent
{
    [SerializeField] Mintoner mintoner;
    [SerializeField] Birdie birdie;
    [SerializeField] float birdieRandomRadius = 10f;
    [SerializeField] float birdieServeSpeed = 5f;
    [SerializeField] float birdieServeAngle = 50f;

    private Vector3 agentStartPos;

    Transform courtT => transform.parent;
    protected override void Awake()
    {
        base.Awake();
        //Randomize decision offset
        var requester = GetComponent<DecisionRequester>();
        if (requester)
        {
            requester.DecisionStep = Random.Range(0, requester.DecisionPeriod);
        }
    }
    private void Start()
    {
        agentStartPos = mintoner.rb.position;
        
    }
    public override void OnEpisodeBegin()
    {
        base.OnEpisodeBegin();
        //mintoner.rb.MovePosition(agentStartPos);
        mintoner.rb.linearVelocity = Vector3.zero;
        mintoner.SetMoveInput(Vector2.zero);
        birdie.rb.AddForce(Vector3.RotateTowards(Vector3.back * birdieServeSpeed, Vector3.up, birdieServeAngle*Mathf.Deg2Rad, 0f), ForceMode.VelocityChange);
        //birdie.rb.MovePosition(courtT.position + Random.insideUnitCircle.HorizontalV2toV3().normalized * birdieRandomRadius);
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        base.CollectObservations(sensor);
        var toBirdie = birdie.rb.position - mintoner.rb.position;
        sensor.AddObservation(toBirdie);
        var relativeVelocity = (mintoner.rb.linearVelocity - birdie.rb.linearVelocity).ToHorizontalV2();
        sensor.AddObservation(relativeVelocity);
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        base.OnActionReceived(actions);

        var moveInput = new Vector2(actions.ContinuousActions[0], actions.ContinuousActions[1]);
        moveInput = Vector2.ClampMagnitude(moveInput, 1f);
        //mintoner.SetMoveInput(moveInput);
        mintoner.rb.AddForce(moveInput.HorizontalV2toV3() * mintoner.MaxAcceleartion, ForceMode.Acceleration);

        // Near the birdie
        //if(Vector3.Distance(mintoner.rb.position, birdie.rb.position) < 1.3f)
        //{
        //    AddReward(1f);
        //    EndEpisode();
        //    return;
        //}
        var toBirdie = birdie.rb.position - mintoner.rb.position;
        AddReward(birdieRandomRadius - toBirdie.magnitude);

        // Fell off platform lol
        if(mintoner.rb.position.y < courtT.position.y-1f)
        {
            EndEpisode();
        }
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        var moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        continuousActionsOut[0] = moveInput.x;
        continuousActionsOut[1] = moveInput.y;
    }
}
