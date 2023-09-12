using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Serialization;

[AddTypeMenu("PlayerStates/" + nameof(PlayerThrowingState)), Serializable]
public class PlayerThrowingState : State
{
    [SerializeField] private PlayerBallHandler playerBallHandler;
    [FormerlySerializedAs("fakeBallAssetReference")]
    [SerializeField]
    private AssetReference fakeBallReference;
    [SerializeField] private Vector3 throwDirection;
    [SerializeField] private float maxThrowingForce = 20f;
    public ForceMode throwingForceMode;
    [SerializeField] private GameObject throwingCamera;
    [SerializeField] private PlayerTargetGoal playerTargetGoal;

    [Header("Debug")] private Rigidbody throwingBallRigidbody;
    [SerializeField] private float throwingForcePercentage = 0;
    private GameObject fakeBallInstance;

    private PlayerActions playerActions;
    private float valueToPowerUpEachTime = 0.1f;
    [SerializeField] PlayerStateHandler playerStateHandler;
    private string stateToReturnOnCancel;

    public override void OnEnterState()
    {
        playerActions = new PlayerActions();
        playerActions.Enable();
        var asyncOperationHandler = fakeBallReference.InstantiateAsync();
        asyncOperationHandler.Completed += HandleLoadedFakeBall;
        throwingBallRigidbody = playerBallHandler.GetBall().GetComponent<Rigidbody>();
        throwingCamera.SetActive(true);
        playerActions.ThrowingState.ThrowBall.performed += OnThrowInput;
        playerActions.ThrowingState.DebugThrow.performed += DebugSimulateTrajectory;
        playerActions.ThrowingState.PowerChange.performed += OnPowerInputChanged;
        playerActions.ThrowingState.Cancel.performed += OnThrowingStateCancelInput;
    }

    private void DebugSimulateTrajectory(InputAction.CallbackContext obj)
    {
        SimulateTrajectory();
    }


    private void HandleLoadedFakeBall(AsyncOperationHandle<GameObject> obj)
    {
        fakeBallInstance = obj.Result;
        SimulateTrajectory();
    }

    public void SimulateTrajectory()
    {
        Rigidbody fakeBallRigidbody = fakeBallInstance.GetComponent<Rigidbody>();
        fakeBallRigidbody.GetComponent<TrailRenderer>().Clear();
        fakeBallRigidbody.MovePosition(throwingBallRigidbody.position);
        fakeBallRigidbody.MoveRotation(throwingBallRigidbody.transform.rotation);

        fakeBallRigidbody.velocity = Vector3.zero;
        fakeBallRigidbody.angularVelocity = Vector3.zero;
        //   fakeBallRigidbody.transform.position = throwingBallRigidbody.transform.position;
        //     fakeBallRigidbody.transform.rotation = throwingBallRigidbody.transform.rotation;
        //   throwDirection = (playerTargetGoal.GetTargetGoal().transform.position - throwingBallRigidbody.transform.position).normalized;

        throwDirection =
            (playerTargetGoal.GetTargetGoal().transform.position - throwingBallRigidbody.transform.position)
            .normalized; // Calcula la dirección de lanzamiento
        throwDirection.y = 1; // Añade una componente vertical positiva
        throwDirection = throwDirection.normalized; // Normaliza la dirección de nuevo
        Throw(fakeBallRigidbody);
    }

    public void AdjustThrowingForce()
    {
        SimulateTrajectory();
    }


    public override void OnUpdateState()
    {
    }

    public override void OnExitState()
    {
        Addressables.ReleaseInstance(fakeBallInstance);
        fakeBallReference.ReleaseAsset();
        throwingCamera.SetActive(false);
        playerActions.ThrowingState.ThrowBall.performed -= OnThrowInput;
        playerActions.ThrowingState.PowerChange.performed -= OnPowerInputChanged;
        playerActions.ThrowingState.Cancel.performed -= OnThrowingStateCancelInput;
    }

    public void OnThrowingStateCancelInput(InputAction.CallbackContext context)
    {
        playerStateHandler.ChangeState("PlayerIdleState");
    }

    public void OnThrowInput(InputAction.CallbackContext context)
    {
        Throw(throwingBallRigidbody);
    }

    public void Throw(Rigidbody bodyToThrow)
    {
        bodyToThrow.AddForce(
            throwDirection * throwingForcePercentage * maxThrowingForce,
            throwingForceMode);
    }

    public void OnPowerInputChanged(InputAction.CallbackContext context)
    {
        float direction = Mathf.Clamp(context.ReadValue<float>(), -1, 1);
        throwingForcePercentage = Mathf.Clamp(throwingForcePercentage + direction * valueToPowerUpEachTime, 0, 1);
    }

    public void OnChangeDirectionInput(InputAction.CallbackContext context)
    {
        float direction = context.ReadValue<float>();
    }
}