using UnityEngine;

public class Birdie : MonoBehaviour, IResettable
{
    [SerializeField] float maxSpeed = 15f;
    public Rigidbody rb { get; private set; }
    private Vector3 startPos;

    public void OnEpisodeBegin()
    {
        rb.position = startPos;
        rb.linearVelocity = Vector3.zero;
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        startPos = rb.position;
        rb.maxLinearVelocity = maxSpeed;
    }
    /*void OnCollisionEnter(Collision collision)
    {
        // apply some random extra impulse when bouncing
        var impulse = collision.contacts[0].impulse;
        var randomDir = Random.onUnitSphere*90f;
        randomDir.y = Mathf.Abs(randomDir.y);
        rb.AddForce(Quaternion.Euler(randomDir) * impulse * 0.1f, ForceMode.Impulse);
        //rb.AddForce()
    }*/
}
