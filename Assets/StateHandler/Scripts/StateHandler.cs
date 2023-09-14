using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateHandler : MonoBehaviour
{
    [Header("Config")]
    [SerializeReference, SubclassSelector] private List<IState> availableStates;
    [SerializeReference, SubclassSelector] private IState currentState;
    [SerializeReference, SubclassSelector] private IState defaultState;
    [SerializeField] private bool blockSwitchToSameState = true;

    private void Awake() 
    {
        if (currentState == null && defaultState != null)
        {
            ChangeState(defaultState);
        }
    }

    private void Update()
    {
        currentState.OnUpdateState();
    }

    public void ChangeState(String newState)
    {
        ChangeState(GetStateOfName(newState));
    }


    private IState GetStateOfName(String name)
    {
        foreach (var state in availableStates)
        {
            if (state.GetType().ToString() == name)
            {
                return state;
            }
        }
        Debug.LogError($"IState with name {name} not found");
        return defaultState;
    }

    private void ChangeState(IState newState)
    {
        if (blockSwitchToSameState && newState ==currentState)
        {
            return;
        }
        currentState.OnExitState();
        currentState = newState;
        currentState.OnEnterState();
    }

    public Type GetCurrentStateType()
    {
       return currentState.GetType();
    }
}


