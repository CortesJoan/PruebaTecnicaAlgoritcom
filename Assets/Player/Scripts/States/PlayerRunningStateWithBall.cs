using System;
using UnityEngine;
using UnityEngine.Serialization;

[AddTypeMenu("PlayerStates/" + nameof(PlayerRunningStateWithBall)), Serializable]
public class PlayerRunningStateWithBall : IState
{ 
    [Header("References")]
    [SerializeReference] private IAnimationHandler iAnimationHandler;
    [SerializeField] private PlayerMovement playerMovement;

    [Header("Config")] 
    [SerializeField] private string walkingRunningStateName = "PlayerRunningStateWithBall";
    [SerializeField] private float movementSpeed = 90;
    
   

    public void OnEnterState()
    {
        iAnimationHandler.PlayAnimationState(walkingRunningStateName);
        playerMovement.ChangeMoveSpeed(movementSpeed);
    }

    public void OnUpdateState()
    {
    }

    public void OnExitState()
    {
    }
}