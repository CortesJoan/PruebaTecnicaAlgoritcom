using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerThrowingInputHandler
{
    PlayerActions playerActions;
    PlayerThrowingState throwingState;

    public PlayerThrowingInputHandler(PlayerThrowingState throwingState)
    {
        this.throwingState = throwingState;
    }

    public void SubscribeToInputEvents()
    {
        playerActions = new PlayerActions();
        playerActions.Enable();

        playerActions.ThrowingState.ThrowBall.performed += OnThrowInput;
        playerActions.ThrowingState.PowerChange.performed += OnPowerChangeInput;
        playerActions.ThrowingState.Cancel.performed += OnCancelInput;
        playerActions.ThrowingState.DebugThrow.performed += DebugSimulateTrajectory;
        playerActions.ThrowingState.DirectionChange.performed += OnDirectionChangeInput;
    }

    private void DebugSimulateTrajectory(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }
        throwingState.SimulateTrajectory();
    }

    public void UnsubscribeFromInputEvents()
    {
        playerActions.Disable();
        playerActions.ThrowingState.ThrowBall.performed -= OnThrowInput;
        playerActions.ThrowingState.DebugThrow.performed -= DebugSimulateTrajectory;
        playerActions.ThrowingState.PowerChange.performed -= OnPowerChangeInput;
        playerActions.ThrowingState.Cancel.performed -= OnCancelInput;
        playerActions.ThrowingState.DirectionChange.performed -= OnDirectionChangeInput;
    }

    void OnThrowInput(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }
        throwingState.OnThrowInput();
    }

    void OnPowerChangeInput(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }
        throwingState.OnPowerInputChanged(context.ReadValue<float>());
    }

    void OnCancelInput(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }
        throwingState.OnThrowingStateCancelInput();
    }

    void OnDirectionChangeInput(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }
        throwingState.OnDirectionChange(context.ReadValue<float>());
        
    }
}