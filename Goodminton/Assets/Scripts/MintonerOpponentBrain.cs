using UnityEngine;

public class MintonerOpponentBrain : MonoBehaviour
{
    private Mintoner mintoner;
    private Birdie birdie;

    private void Awake()
    {
        mintoner = GetComponent<Mintoner>();
        birdie = transform.parent.GetComponentInChildren<Birdie>();
    }
    private void FixedUpdate()
    {
        var toBirdie = birdie.transform.position - mintoner.transform.position;
        mintoner?.SetMoveInput(toBirdie.Flatten().ToHorizontalV2());
    }
}
