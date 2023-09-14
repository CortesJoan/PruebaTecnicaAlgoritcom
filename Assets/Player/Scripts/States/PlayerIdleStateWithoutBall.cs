using System;
using UnityEngine;
using UnityEngine.Serialization;

[AddTypeMenu("PlayerStates/" + nameof(PlayerIdleStateWithoutBall)), Serializable]
public class PlayerIdleStateWithoutBall : IState
{
    [FormerlySerializedAs("playerAnimationHandler")]   [SerializeReference, SubclassSelector] private IAnimationHandler animationHandler;
    [SerializeField] private string idleAnimationStateName = "PlayerIdleStateWithoutBall";
     
    public void OnEnterState() 
    {
        animationHandler.PlayAnimationState(idleAnimationStateName);    
    }

    public void OnUpdateState()
    {
     }

    public void OnExitState()
    {
     }
}