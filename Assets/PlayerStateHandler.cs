using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateHandler : MonoBehaviour
{
    [SerializeReference, SubclassSelector] public List<State> availableStates;
    [SerializeReference, SubclassSelector] public State currentState;
    [SerializeReference, SubclassSelector] public State defaultState;
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


    private State GetStateOfName(String name)
    {
        foreach (var state in availableStates)
        {
            if (state.GetType().ToString() == name)
            {
                return state;
            }
        }
        Debug.LogError($"State with name {name} not found");
        return defaultState;
    }

    private void ChangeState(State newState)
    {
        if (blockSwitchToSameState && newState ==currentState)
        {
            return;
        }
        currentState.OnExitState();
        currentState = newState;
        currentState.OnEnterState();
    }
}


