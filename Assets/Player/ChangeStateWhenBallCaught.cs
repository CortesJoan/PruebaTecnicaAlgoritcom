using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChangeStateWhenBallCaught : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private StateHandler stateHandler;

    public void OnBallCaught()
    {
        string currentName = stateHandler.GetCurrentStateType().Name;
        string newName = "";
        newName = currentName.Contains("Catch")
            ? nameof(PlayerIdleStateWithBall)
            : currentName.Replace("WithoutBall", "WithBall");
        stateHandler.ChangeState(newName);
    }

    public void OnBallLost()
    {
        string currentName = stateHandler.GetCurrentStateType().Name;
        string newName = "";
        newName = currentName.Contains("Throw")
            ? nameof(PlayerIdleStateWithoutBall)
            : currentName.Replace("WithBall", "WithoutBall");
        stateHandler.ChangeState(newName);
    }

    public void OnStartRunning()
    {
        string currentName = stateHandler.GetCurrentStateType().Name;
        string newName = currentName.Replace("Idle", "Running");
        stateHandler.ChangeState(newName);
    }

    public void OnStopRunning()
    {
        string currentName = stateHandler.GetCurrentStateType().Name;
        string newName = currentName.Replace("Running", "Idle");
        stateHandler.ChangeState(newName);
    }
}