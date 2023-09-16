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
    [FormerlySerializedAs("catchBall")]
    [Header("References")]
    [SerializeField] private CatchBallInput catchBallInput;
    [SerializeField] private ChangeStateWhenBallCaught changeStateWhenBallCaught;
    [SerializeField] private PlayerBallHandler playerBallHandler;
    [SerializeField] private PlayerPossessionCounter playerPossessionCounter;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeReference, SubclassSelector] private IAnimationHandler animationHandler;

    [Header("Config")] 
    [SerializeField] private GameObject catchingCamera;

    [Header("Debug")] 
    private PlayerActions playerActions;
    private Transform previousCamera;
    private GameObject catchedBall;
    private float timeSinceEnteredState = 0;

    public void OnEnterState()
    {
        timeSinceEnteredState = 0;
        catchedBall = catchBallInput.GetCaughtBall();
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
        playerPossessionCounter.OnPossessionStarted();
    }


    public void OnUpdateState()
    {
        float currentDuration = animationHandler.GetCurrentAnimationDuration();

        timeSinceEnteredState = Mathf.Min(timeSinceEnteredState + Time.deltaTime, currentDuration);
        if (timeSinceEnteredState.Equals(currentDuration))
        {
            CatchTheBall();
        }
    }

    public void OnExitState()
    {
        playerMovement.SetMoveRelativeTo(previousCamera.transform);
        if (catchingCamera)
        {
            catchingCamera.SetActive(false);
        }
    }
}