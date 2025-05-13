using UnityEngine;
using UnityEngine.Events;

public class OnEnableEvent : MonoBehaviour
{
    [SerializeField] UnityEvent enableEvent;
    [SerializeField] UnityEvent disableEvent;
    private void OnEnable()
    {
        enableEvent?.Invoke();
    }
    private void OnDisable()
    {
        disableEvent?.Invoke();
    }
}
