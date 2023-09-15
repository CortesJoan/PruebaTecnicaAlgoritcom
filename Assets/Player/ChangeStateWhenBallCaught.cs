using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 

public class ChangeStateWhenBallCaught : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private StateHandler stateHandler;

    public void OnBallCaught()
    {     
        string currentName = stateHandler.GetCurrentStateType().Name;
        string newName = "";
        if (currentName.Contains("Catch"))
        {
            newName = nameof(PlayerIdleStateWithBall);
        }else{
          newName = currentName.Replace("WithoutBall", "WithBall");
        }
        stateHandler.ChangeState(newName);
    }

    public void OnBallLost()
    {
        string currentName = stateHandler.GetCurrentStateType().Name;
        string newName = currentName.Replace("WithBall", "WithoutBall");
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