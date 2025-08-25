using System;
using UnityEngine;

public class UnitDieEventHandler : MonoBehaviour
{
    private Action _onDieAction;

    public void Init(Action onDieAction)
    {
        _onDieAction = onDieAction;
    }

    public void OnDie()
    {
        _onDieAction?.Invoke();
    }
}