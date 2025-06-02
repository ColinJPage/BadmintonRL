using UnityEngine;
using UnityEngine.InputSystem;
using Unity.MLAgents.Actuators;

/// <summary>
/// This class reads player input and can spit out the action values for the agent
/// </summary>
public class MintonerHeuristic : MonoBehaviour
{
    [SerializeField] InputActionAsset inputActions;
    [SerializeField] Mintoner mintoner;

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
        moveInput = (Quaternion.Euler(0f, Vector3.SignedAngle(transform.forward, Camera.main.transform.forward, Vector3.up), 0f) * moveInput.HorizontalV2toV3()).ToHorizontalV2();
        mintoner?.SetMoveInput(moveInput);
    }
    public void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = moveInput.x;
        continuousActionsOut[1] = moveInput.y;
    }
}
