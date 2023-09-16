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
    [SerializeField] private StateHandler stateHandler;
    [SerializeField] private PlayerBallHandler playerBallHandler;
    [SerializeField] private PowerBarController powerBarController;
    [SerializeField] private PlayerTargetBasket playerTargetBasket;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeReference, SubclassSelector] private IAnimationHandler animationHandler;
    [SerializeField] private GameObject powerBarCanvas;


    [Header("Config")] 
    [SerializeField] private AssetReference fakeBallReference;
    [SerializeField] private LayerMask layerMask;
    [SerializeField, Range(0, 1)] private float throwingForceClampedAtEnterTheState = 0.5f;
    [SerializeField] private float maxThrowingForce = 20f;
    [SerializeField] private ForceMode throwingForceMode;
    [SerializeField] private GameObject throwingCamera;
    private const float valueToPowerUpEachTime = 0.01f;
    private const float valueToChangeDirectionEachTime = 0.01f;
    private string stateToReturnOnCancel = nameof(PlayerIdleStateWithBall);
    private string stateToReturnAfterThrowing = nameof(PlayerIdleStateWithoutBall);

    [Header("Debug")] private TrajectorySimulator trajectorySimulator;
    private Rigidbody throwingBallRigidbody;
    private Transform previousCamera;
    private PlayerThrowingInputHandler inputHandler;

    public void OnEnterState()
    {
        powerBarController.SetCurrentPower(throwingForceClampedAtEnterTheState);
        AddressableAssetLoader addressableAssetLoader = new AddressableAssetLoader(fakeBallReference);
        addressableAssetLoader.LoadAsync(HandleLoadedFakeBall);
        throwingBallRigidbody = playerBallHandler.GetBall().GetComponent<Rigidbody>();
        trajectorySimulator = new TrajectorySimulator(fakeBallReference, playerBallHandler.transform,throwingBallRigidbody);
        HandleThrowingCamera();
        animationHandler.PlayAnimationState(GetType().Name);
        powerBarCanvas.SetActive(true);
        inputHandler = new PlayerThrowingInputHandler(this);
        inputHandler.SubscribeToInputEvents();
        playerMovement.BlockMovement();
    }

    private void HandleThrowingCamera()
    {
        if (!throwingCamera) return;
        var targetGroup = throwingCamera.GetComponent<CinemachineVirtualCamera>().Follow
            .GetComponent<CinemachineTargetGroup>();
        if (targetGroup)
        {
            targetGroup.m_Targets[1].target = playerTargetBasket.GetTargetBasket()
                .GetComponentInParent<BasketballHoopController>().GetTargetPoint().transform;
        }
        throwingCamera.SetActive(true);
        previousCamera = playerMovement.GetMoveRelativeTo();
        playerMovement.SetMoveRelativeTo(throwingCamera.transform);
    }

    private void DebugSimulateTrajectory()
    {
        SimulateTrajectory();
    }

    private void HandleLoadedFakeBall(GameObject newFakeBall)
    {
        SimulateTrajectory();
    }

    public void OnUpdateState()
    {
    }

    public void OnExitState()
    {
        playerMovement.UnblockMovement();
        powerBarCanvas.SetActive(false);
        if (throwingCamera)
        {
            playerMovement.SetMoveRelativeTo(previousCamera.transform);
            previousCamera.gameObject.SetActive(true);
            throwingCamera.SetActive(false);
        }
        inputHandler.UnsubscribeFromInputEvents();
        trajectorySimulator.Cleanup();
    }

    public void OnThrowingStateCancelInput()
    {
        trajectorySimulator.Cleanup();
        stateHandler.ChangeState(stateToReturnOnCancel);
    }

    public void OnThrowInput()
    {
        ReleaseBall();
        playerBallHandler.SetLastThrowPosition(throwingBallRigidbody.position);
        trajectorySimulator.ThrowRigidbodyInTrajectory(throwingBallRigidbody, powerBarController.GetCurrentPower(),
            maxThrowingForce, throwingForceMode);
        playerMovement.UnblockMovement();
        stateHandler.ChangeState(stateToReturnAfterThrowing);
    }

    public void ReleaseBall()
    {
        playerBallHandler.LoseBall(false);
    }

    public void OnPowerInputChanged(float newDirection)
    {
        var direction = Mathf.Clamp(newDirection, -1, 1);
        powerBarController.SetCurrentPower(Mathf.Clamp01(powerBarController.GetCurrentPower() +
                                                         direction * valueToPowerUpEachTime));
        SimulateTrajectory();
    }


    public void OnDirectionChange(float readValue)
    {
        trajectorySimulator.ChangeXTrajectory(readValue*valueToChangeDirectionEachTime);
        SimulateTrajectory();
    }

    public void SimulateTrajectory()
    {
        trajectorySimulator.DrawTrajectory(playerBallHandler.GetBallVisuals().transform.position,
            powerBarController.GetCurrentPower(),
            maxThrowingForce, layerMask);
    }
}