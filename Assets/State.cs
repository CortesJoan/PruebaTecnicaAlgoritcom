using System;

[Serializable]
public abstract class State
{
    public abstract void OnEnterState();
    public abstract void OnUpdateState();
    public abstract void OnExitState();
}