using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Serialization;

[AddTypeMenu("PlayerStates/" + nameof(PlayerThrowingState)), Serializable]
public class PlayerThrowingState : IState
{
    [Header("References")]
    [SerializeField] private StateHandler stateHandler;
    [SerializeField] private PlayerBallHandler playerBallHandler;
    [SerializeField] private PlayerTargetGoal playerTargetGoal;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private GameObject canvasGameObject;
    [SerializeReference,SubclassSelector] private IAnimationHandler animationHandler;

    [Header("Config")]
    [SerializeField] private AssetReference fakeBallReference;
    [SerializeField] private Vector3 throwDirection;
    [SerializeField] private float throwingForcePercentage = 0.5f;
    [SerializeField] private float maxThrowingForce = 20f;
    [SerializeField] private ForceMode throwingForceMode;
    [SerializeField] private GameObject throwingCamera;

    [Header("Debug")]
    private GameObject projectileSimulationInstance;
    private Rigidbody throwingBallRigidbody;
    private PlayerActions playerActions;
    private float valueToPowerUpEachTime = 0.1f;
    private string stateToReturnOnCancel = nameof(PlayerIdleStateWithBall);
    private string stateToReturnAfterThrowing = nameof(PlayerIdleStateWithoutBall);
    private Transform previousCamera;
    private float minimumYDirection = 1;
    public void OnEnterState()
    {
        playerActions = new PlayerActions();
        playerActions.Enable();
        playerActions.ThrowingState.ThrowBall.performed += OnThrowInput;
        playerActions.ThrowingState.DebugThrow.performed += DebugSimulateTrajectory;
        playerActions.ThrowingState.PowerChange.performed += OnPowerInputChanged;
        playerActions.ThrowingState.Cancel.performed += OnThrowingStateCancelInput;
        
        var asyncOperationHandler = fakeBallReference.InstantiateAsync();
        asyncOperationHandler.Completed += HandleLoadedFakeBall;
        
        throwingBallRigidbody = playerBallHandler.GetBall().GetComponent<Rigidbody>();
        throwingCamera.SetActive(true);
       
        previousCamera = playerMovement.GetMoveRelativeTo();
        playerMovement.SetMoveRelativeTo(throwingCamera.transform);
        animationHandler.PlayAnimationState(GetType().Name);
        canvasGameObject.SetActive(true);
    }

    private void DebugSimulateTrajectory(InputAction.CallbackContext obj)
    {
        SimulateTrajectory();
    }


    private void HandleLoadedFakeBall(AsyncOperationHandle<GameObject> obj)
    {
        projectileSimulationInstance = obj.Result;
        SimulateTrajectory();
    }

   

    public void AdjustThrowingForce()
    {
        SimulateTrajectory();
    }

    public void SimulateTrajectory()
    {
        var ballVisual=        playerBallHandler.GetBallVisuals();
        Rigidbody projectileSimulation = projectileSimulationInstance.GetComponent<Rigidbody>();
        projectileSimulation.GetComponent<TrailRenderer>().Clear();
        projectileSimulation.MovePosition(ballVisual.transform.position);
        projectileSimulation.MoveRotation(ballVisual.transform.rotation);

        projectileSimulation.velocity = Vector3.zero;
        projectileSimulation.angularVelocity = Vector3.zero;
        
        throwDirection =
            (playerTargetGoal.GetTargetGoal().transform.position - ballVisual.transform.position);
            
        throwDirection = throwDirection.normalized;
        throwDirection.y = minimumYDirection;

        Throw(projectileSimulation);
    }
    public void OnUpdateState()
    {
    }

    public void ReleaseBall()
    {
        playerBallHandler.GetComponent<CatchBall>().LoseBall();
    }
    public void OnExitState()
    {
        canvasGameObject.SetActive(false);
        playerMovement.SetMoveRelativeTo(previousCamera.transform);

        Addressables.ReleaseInstance(projectileSimulationInstance);
        fakeBallReference.ReleaseAsset();
        throwingCamera.SetActive(false);
        playerActions.ThrowingState.ThrowBall.performed -= OnThrowInput;
        playerActions.ThrowingState.DebugThrow.performed -= DebugSimulateTrajectory;
        playerActions.ThrowingState.PowerChange.performed -= OnPowerInputChanged;
        playerActions.ThrowingState.Cancel.performed -= OnThrowingStateCancelInput;
     }

    public void OnThrowingStateCancelInput(InputAction.CallbackContext context)
    {
        stateHandler.ChangeState(stateToReturnOnCancel);
    }

    public void OnThrowInput(InputAction.CallbackContext context)
    {
        ReleaseBall();
        Throw(throwingBallRigidbody);
        stateHandler.ChangeState(stateToReturnAfterThrowing);

    }

    public void Throw(Rigidbody bodyToThrow)
    { 
        bodyToThrow.velocity = Vector3.zero;
        bodyToThrow.angularVelocity = Vector3.zero;
        bodyToThrow.AddForce(
            throwDirection * throwingForcePercentage * maxThrowingForce,
            throwingForceMode);
    }

    public void OnPowerInputChanged(InputAction.CallbackContext context)
    {
        SimulateTrajectory();
        float direction = Mathf.Clamp(context.ReadValue<float>(), -1, 1);
        throwingForcePercentage = Mathf.Clamp(throwingForcePercentage + direction * valueToPowerUpEachTime, 0, 1);
    }

    public void OnChangeDirectionInput(InputAction.CallbackContext context)
    {
        SimulateTrajectory(); 
        Vector2 direction = context.ReadValue<Vector2>();
    }

   
}