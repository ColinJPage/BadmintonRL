using UnityEngine;
using UnityEngine.Events;

public class TriggerEnterEvent : MonoBehaviour
{
    [SerializeField] UnityEvent triggerEvent;

    private void OnTriggerEnter(Collider other)
    {
        triggerEvent?.Invoke();
    }
}
