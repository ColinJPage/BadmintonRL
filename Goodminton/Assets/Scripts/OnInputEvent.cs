using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class OnInputEvent : MonoBehaviour
{
    [SerializeField] InputActionReference actionReference;
    [SerializeField] UnityEvent performEvent;

    private void OnEnable()
    {
        actionReference.action.Enable();
        actionReference.action.performed += OnAction;
    }
    private void OnDisable()
    {
        actionReference.action.performed -= OnAction;
    }
    void OnAction(InputAction.CallbackContext context)
    {
        performEvent?.Invoke();
    }
}
