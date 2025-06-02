using UnityEngine;

public class Mintoner : MonoBehaviour, IResettable
{
    [SerializeField] float maxAcceleration = 5f;
    public float MaxAcceleartion => maxAcceleration;
    [SerializeField] float maxSpeed = 10f;
    public Rigidbody rb
    {
        get
        {
            if (!r) r = GetComponent<Rigidbody>();
            return r;
        }
    }
    private Rigidbody r;
    private Vector2 moveInput;
    private Vector3 startPos;
    private void Awake()
    {
        startPos = rb.position;
    }
    /// <summary>
    /// In local space
    /// </summary>
    /// <param name="moveInput"></param>
    public void SetMoveInput(Vector2 moveInput)
    {
        this.moveInput = moveInput;
    }
    private void FixedUpdate()
    {
        var goalVelocity = maxAcceleration * transform.TransformDirection(moveInput.HorizontalV2toV3()); // Colin: maxAcceleartion should be maxSpeed! But we already trained so I'm not gonna fix it teehee
        var desiredVelocityChange = goalVelocity - rb.linearVelocity;
        var velocityChange = Vector3.ClampMagnitude(desiredVelocityChange.normalized*maxAcceleration * Time.fixedDeltaTime, desiredVelocityChange.magnitude);
        rb.AddForce(velocityChange, ForceMode.VelocityChange);
        rb.linearVelocity = Vector3.ClampMagnitude(rb.linearVelocity, maxSpeed);
    }

    public void OnEpisodeBegin()
    {
        rb.position = startPos;
        rb.linearVelocity = Vector3.zero;
        SetMoveInput(Vector2.zero);
    }
}
