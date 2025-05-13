using UnityEngine;

public class Birdie : MonoBehaviour
{
    public Rigidbody rb { get; private set; }

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
}
