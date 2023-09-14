using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class EnterShootState : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private StateHandler stateHandler;
    [SerializeField] private PlayerBallHandler playerBallHandler;
 
    [Header("Config")] 
    [SerializeField] private String shootStateName = nameof(PlayerThrowingState);
 
   

    public void OnEnterShootStateInput(InputAction.CallbackContext context)
    {
        if (context.performed && playerBallHandler.HasBall())
        {
            stateHandler.ChangeState(shootStateName);
        }
    }
}