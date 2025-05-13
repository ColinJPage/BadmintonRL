using UnityEngine;

public class Mintoner : MonoBehaviour
{
    [SerializeField] float maxAcceleration = 5f;
    public float MaxAcceleartion => maxAcceleration;
    public Rigidbody rb { get; private set; }
    private Vector2 moveInput;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    public void SetMoveInput(Vector2 moveInput)
    {
        this.moveInput = moveInput;
    }
    private void FixedUpdate()
    {
        rb.AddForce(moveInput.HorizontalV2toV3() * maxAcceleration, ForceMode.Acceleration);
    }
}
