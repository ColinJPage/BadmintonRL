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
    [SerializeField] float livingRewardPerSec = 1f;

    private MintonerHeuristic heuristic;

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
        heuristic = GetComponent<MintonerHeuristic>();
    }
    private void Start()
    {
        agentStartPos = mintoner.rb ? mintoner.rb.position : mintoner.transform.position;
        
    }
    public override void OnEpisodeBegin()
    {
        base.OnEpisodeBegin();
        //mintoner.rb.MovePosition(agentStartPos);
        foreach(var r in transform.parent.GetComponentsInChildren<IResettable>())
        {
            r.OnEpisodeBegin();
        }
        var serveDir = Vector3.RotateTowards(Vector3.back * birdieServeSpeed, Vector3.up, birdieServeAngle * Mathf.Deg2Rad, 0f);
        serveDir = Quaternion.Euler(0f, Random.Range(-1f, 1f) * 45f, 0f) * serveDir;
        birdie.rb.AddForce(serveDir, ForceMode.VelocityChange);
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
        mintoner.SetMoveInput(moveInput);
        //mintoner.rb.AddForce(moveInput.HorizontalV2toV3() * mintoner.MaxAcceleartion, ForceMode.Acceleration);

        // Near the birdie
        //if(Vector3.Distance(mintoner.rb.position, birdie.rb.position) < 1.3f)
        //{
        //    AddReward(1f);
        //    EndEpisode();
        //    return;
        //}
        

       
    }
    private void FixedUpdate()
    {
        // Fell off platform lol
        if (mintoner.rb.position.y < courtT.position.y - 1f)
        {
            EndEpisode();
            return;
        }

        var birdPos = birdie.rb.position;
        // fault
        if(birdPos.y < transform.position.y + 0.25f)
        {
            // True if birdie landed on opponent's side, false otherwise
            bool agentsPoint = birdie.transform.parent.InverseTransformPoint(birdie.rb.position).z > 0f;
            if (agentsPoint)
            {
                AddReward(100f); // we won!
            }
            else
            {
                var toBirdie = birdie.rb.position - mintoner.rb.position;
                AddReward(birdieRandomRadius - toBirdie.magnitude);
            }
            EndEpisode();
        }
        else
        {
            AddReward(livingRewardPerSec * Time.fixedDeltaTime);
        }
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        heuristic.Heuristic(actionsOut);
    }
}
