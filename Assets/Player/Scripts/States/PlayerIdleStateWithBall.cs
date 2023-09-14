using System;
using UnityEngine;
using UnityEngine.Serialization;

[AddTypeMenu("PlayerStates/" + nameof(PlayerIdleStateWithBall)),Serializable]

public class PlayerIdleStateWithBall : IState
{
    [SerializeReference, SubclassSelector] private IAnimationHandler iAnimationHandler;
    [SerializeField] private string idleAnimationStateName = "PlayerIdleStateWithBall";
    public void OnEnterState()
    {
        iAnimationHandler.PlayAnimationState(idleAnimationStateName);    
     }

    public void OnUpdateState()
    {
        
     }

    public void OnExitState()
    {
     
    }
}