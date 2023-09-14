using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Serialization;

[AddTypeMenu("PlayerStates/" + nameof(PlayerCatchBallState)), Serializable]
public class PlayerCatchBallState : IState
{
    [Header("References")]
     [SerializeField] private CatchBall catchBall;
    [SerializeField] private ChangeStateWhenBallCaught changeStateWhenBallCaught;
    [SerializeField] private PlayerBallHandler playerBallHandler;
    [SerializeField] private PlayerTargetGoal playerTargetGoal;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeReference, SubclassSelector] private IAnimationHandler animationHandler;

    [Header("Config")] 
    [SerializeField] private AssetReference fakeBallReference;
    [SerializeField] private Vector3 throwDirection;
    [SerializeField] private float throwingForcePercentage = 0.5f;
    [SerializeField] private float maxThrowingForce = 20f;
    [SerializeField] private ForceMode throwingForceMode; 
    [SerializeField] private GameObject catchingCamera;

    [Header("Debug")] 
    private GameObject projectileSimulationInstance;
    private Rigidbody throwingBallRigidbody;
    private PlayerActions playerActions;
    private float valueToPowerUpEachTime = 0.1f;
      private Transform previousCamera;
    private float minimumYDirection = 1;
    private GameObject catchedBall;
    public void OnEnterState()
    {
        timeSinceEnteredState = 0;
        catchedBall = catchBall.gameObject;
        playerActions = new PlayerActions();
        playerActions.Enable();
        playerMovement.BlockMovement();
        if (catchingCamera != null)
        {
            catchingCamera.SetActive(true);
            playerMovement.SetMoveRelativeTo(catchingCamera.transform);
        }
        previousCamera = playerMovement.GetMoveRelativeTo();
        animationHandler.PlayAnimationState(GetType().Name);
     }

   

    public void CatchTheBall()
    {
        playerBallHandler.GiveBall(catchedBall);
        changeStateWhenBallCaught.OnBallCaught();
      
        playerMovement.UnblockMovement();
    }

    private float timeSinceEnteredState = 0;
    public void OnUpdateState()
    {
        float currentDuration = animationHandler.GetCurrentAnimationDuration();

        timeSinceEnteredState = Mathf.Min(timeSinceEnteredState + Time.deltaTime, currentDuration);
        if (timeSinceEnteredState==currentDuration)
        {
            CatchTheBall();
        }
        
    }


    public void OnExitState()
    {
        playerMovement.SetMoveRelativeTo(previousCamera.transform);

        Addressables.ReleaseInstance(projectileSimulationInstance);
        fakeBallReference.ReleaseAsset();
        catchingCamera.SetActive(false);
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
         float direction = Mathf.Clamp(context.ReadValue<float>(), -1, 1);
        throwingForcePercentage = Mathf.Clamp(throwingForcePercentage + direction * valueToPowerUpEachTime, 0, 1);
    }

    public void OnChangeDirectionInput(InputAction.CallbackContext context)
    {
         Vector2 direction = context.ReadValue<Vector2>();
    }
}