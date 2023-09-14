using System;
using UnityEngine;
using UnityEngine.Serialization;

[AddTypeMenu("PlayerStates/" + nameof(PlayerRunningStateWithoutBall)), Serializable]
public class PlayerRunningStateWithoutBall : IState
{ 
    [Header("References")]
    [SerializeReference, SubclassSelector] private IAnimationHandler iAnimationHandler;
    [SerializeField] private PlayerMovement playerMovement;

    [Header("Config")]
    [SerializeField] private string runningAnimationStateName = "PlayerRunningStateWithoutBall";
    [SerializeField] private float movementSpeed = 100;
 
    

    public void OnEnterState()
    {
        iAnimationHandler.PlayAnimationState(runningAnimationStateName);
        playerMovement.ChangeMoveSpeed(movementSpeed);
    }

    public void OnUpdateState()
    {
    }

    public void OnExitState()
    {
    }
}