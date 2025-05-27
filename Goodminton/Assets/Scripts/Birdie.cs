using UnityEngine;

public class Birdie : MonoBehaviour, IResettable
{
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
    }
}
