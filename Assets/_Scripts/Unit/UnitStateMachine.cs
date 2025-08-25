using UnityEngine;

public class UnitStateMachine
{
    private readonly UnitStatusSystem _statusSystem;

    public UnitStateMachine(UnitStatusSystem statusSystem)
    {
        _statusSystem = statusSystem;
    }

    public void OnStateChanged(Transform target, UnitState newState)
    {
        _statusSystem.ApplyState(target, newState);
    }
}