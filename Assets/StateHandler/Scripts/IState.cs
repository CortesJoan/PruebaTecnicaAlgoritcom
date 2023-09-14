using System;
 
public interface IState
{
    public abstract void OnEnterState();
    public abstract void OnUpdateState();
    public abstract void OnExitState();
}