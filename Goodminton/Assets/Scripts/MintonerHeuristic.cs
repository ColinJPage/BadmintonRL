using UnityEngine;
using UnityEngine.InputSystem;
using Unity.MLAgents.Actuators;

/// <summary>
/// This class reads player input and can spit out the action values for the agent
/// </summary>
public class MintonerHeuristic : MonoBehaviour
{
    [SerializeField] InputActionAsset inputActions;

    private Vector2 moveInput;
    private void OnEnable()
    {
        inputActions.Enable();
        inputActions.actionMaps[0].FindAction("Move").performed += OnMove;
        inputActions.actionMaps[0].FindAction("Move").canceled += OnMove;
    }
    private void OnDisable()
    {
        inputActions.actionMaps[0].FindAction("Move").performed -= OnMove;
        inputActions.actionMaps[0].FindAction("Move").canceled -= OnMove;
    }
    void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
    public void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = moveInput.x;
        continuousActionsOut[1] = moveInput.y;
    }
}
