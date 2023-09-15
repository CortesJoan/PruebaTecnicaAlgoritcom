using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Serialization;

[AddTypeMenu("PlayerStates/" + nameof(PlayerThrowingState)), Serializable]
public class PlayerThrowingState : IState
{
    [Header("References")]
    [SerializeField]
    private StateHandler stateHandler;
    [SerializeField] private PlayerBallHandler playerBallHandler;
    [FormerlySerializedAs("playerTargetGoal")]
    [SerializeField]
    private PlayerTargetBasket playerTargetBasket;
    [SerializeField] private PlayerMovement playerMovement;
    [FormerlySerializedAs("canvasGameObject")]
    [SerializeField]
    private GameObject powerBarCanvas;
    [SerializeReference, SubclassSelector] private IAnimationHandler animationHandler;

    [Header("Config")] [SerializeField] private AssetReference fakeBallReference;
    [SerializeField] private Vector3 throwDirection;
    [FormerlySerializedAs("throwingForcePercentage")]
    [SerializeField, Range(0, 1)]
    private float throwingForceClamped = 0.5f;
    [SerializeField] private float maxThrowingForce = 20f;
    [SerializeField] private ForceMode throwingForceMode;
    [SerializeField] private GameObject throwingCamera;
    private const float valueToPowerUpEachTime = 0.1f;
    private const float requiredYDirection = 1;
    [Header("Debug")] private TrajectorySimulator trajectorySimulator;
    private GameObject projectileSimulationInstance;
    private Rigidbody throwingBallRigidbody;
    private PlayerActions playerActions;
    private string stateToReturnOnCancel = nameof(PlayerIdleStateWithBall);
    private string stateToReturnAfterThrowing = nameof(PlayerIdleStateWithoutBall);
    private Transform previousCamera;

    public void OnEnterState()
    {
        playerActions = new PlayerActions();
        playerActions.Enable();
        playerActions.ThrowingState.ThrowBall.performed += OnThrowInput;
        playerActions.ThrowingState.DebugThrow.performed += DebugSimulateTrajectory;
        playerActions.ThrowingState.PowerChange.performed += OnPowerInputChanged;
        playerActions.ThrowingState.Cancel.performed += OnThrowingStateCancelInput;

        AddressableAssetLoader addressableAssetLoader = new AddressableAssetLoader(fakeBallReference);
        addressableAssetLoader.LoadAsync(HandleLoadedFakeBall);
        trajectorySimulator = new TrajectorySimulator(fakeBallReference);


        throwingBallRigidbody = playerBallHandler.GetBall().GetComponent<Rigidbody>();
        if (throwingCamera)
        {
            var targetGroup = throwingCamera.GetComponent<CinemachineVirtualCamera>().Follow
                .GetComponent<CinemachineTargetGroup>();
            if (targetGroup)
            {
                targetGroup.m_Targets[0].target = playerTargetBasket.GetTargetBasket()
                    .GetComponentInParent<BasketballHoopController>().GetTargetPoint().transform;
            }
            throwingCamera.SetActive(true);
            previousCamera = playerMovement.GetMoveRelativeTo();
            playerMovement.SetMoveRelativeTo(throwingCamera.transform);
        }

        animationHandler.PlayAnimationState(GetType().Name);
        powerBarCanvas.SetActive(true);
    }

    private void DebugSimulateTrajectory(InputAction.CallbackContext obj)
    {
        SimulateTrajectory();
    }


    private void HandleLoadedFakeBall(GameObject newFakeBall)
    {
        projectileSimulationInstance = newFakeBall;
        SimulateTrajectory();
    }


    public void AdjustThrowingForce()
    {
        SimulateTrajectory();
    }

    public void SimulateTrajectory()
    {
        trajectorySimulator.SimulateTrajectory(playerBallHandler.GetBallVisuals().transform.position,
            playerTargetBasket.GetTargetBasket().transform.position, throwingForceClamped, requiredYDirection);
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
        powerBarCanvas.SetActive(false);
        if (throwingCamera)
        {
            playerMovement.SetMoveRelativeTo(previousCamera.transform);
            throwingCamera.SetActive(false);
        }
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
        playerBallHandler.SetLastThrowPosition(throwingBallRigidbody.position);
        trajectorySimulator.ThrowRigidbodyInTrajectory(throwingBallRigidbody, throwDirection, throwingForceClamped,
            maxThrowingForce);
        stateHandler.ChangeState(stateToReturnAfterThrowing);
    }


    public void OnPowerInputChanged(InputAction.CallbackContext context)
    {
        SimulateTrajectory();
        float direction = Mathf.Clamp(context.ReadValue<float>(), -1, 1);
        throwingForceClamped = Mathf.Clamp(throwingForceClamped + direction * valueToPowerUpEachTime, 0, 1);
    }

    public void OnChangeDirectionInput(InputAction.CallbackContext context)
    {
        SimulateTrajectory();
        Vector2 direction = context.ReadValue<Vector2>();
    }
}