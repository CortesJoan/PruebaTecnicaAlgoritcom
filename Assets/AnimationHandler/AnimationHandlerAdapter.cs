using System;
using UnityEngine;

[AddTypeMenu("IAnimationHandler/" + nameof(AnimationHandlerAdapter)), Serializable]
public class AnimationHandlerAdapter : IAnimationHandler
{
    [Header("References")]
    [SerializeReference] PlayerAnimationHandler playerAnimationHandler;


    public void PlayAnimationState(string animationStateName, int layer = 0)
    {
        playerAnimationHandler.PlayAnimationState(animationStateName, layer);
    }

    public void PauseCurrentAnimationState()
    {
        playerAnimationHandler.PauseCurrentAnimationState();
    }

    public void ResumeCurrentAnimationState()
    {
        playerAnimationHandler.ResumeCurrentAnimationState();
    }

    public float GetCurrentAnimationDuration()
    {
        return playerAnimationHandler.GetCurrentAnimationDuration();
     }
}