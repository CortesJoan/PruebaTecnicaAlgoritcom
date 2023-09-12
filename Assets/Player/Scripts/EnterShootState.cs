using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnterShootState : MonoBehaviour
{
    [SerializeField] private String shootStateName = nameof(PlayerThrowingState);
    [SerializeField] private PlayerStateHandler playerStateHandler;
    [SerializeField] private PlayerBallHandler playerBallHandler;

    public void OnEnterShootStateInput(InputAction.CallbackContext context)
    {
        if (context.performed && playerBallHandler.HasBall())
        {
            playerStateHandler.ChangeState(shootStateName);
        }
    }
}