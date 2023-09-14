using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
public class PlayerAnimationHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Animator animator;

    public void PlayAnimationState(string animationStateName,int layer=0)
    {
        Debug.Log("GoingToState" + animationStateName + " layer" + layer);
        animator.Play(Animator.StringToHash(animationStateName),layer);
    }

    public void PauseCurrentAnimationState()
    {
        animator.speed = 0;
    }

    public void ResumeCurrentAnimationState()
    {
        animator.speed = 1;
    }

    public float GetCurrentAnimationDuration()
    {
        return animator.GetCurrentAnimatorStateInfo(0).length;
    }
} 