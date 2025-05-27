using UnityEngine;

public class Mintoner : MonoBehaviour, IResettable
{
    [SerializeField] float maxAcceleration = 5f;
    public float MaxAcceleartion => maxAcceleration;
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
    public void SetMoveInput(Vector2 moveInput)
    {
        this.moveInput = moveInput;
    }
    private void FixedUpdate()
    {
        rb.AddForce(moveInput.HorizontalV2toV3() * maxAcceleration, ForceMode.Acceleration);
    }

    public void OnEpisodeBegin()
    {
        rb.position = startPos;
        rb.linearVelocity = Vector3.zero;
        SetMoveInput(Vector2.zero);
    }
}
