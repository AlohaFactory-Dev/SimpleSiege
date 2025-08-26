using System;
using UnityEngine;

public class UnitAnimationEventHandler : MonoBehaviour
{
    private Action _onDieAction;
    private Action _onAction;

    public void Init(Action onDieAction)
    {
        _onDieAction = onDieAction;
    }

    public void SetOnAction(Action onAction)
    {
        _onAction = onAction;
    }

    // Animation Event
    public void OnDie()
    {
        _onDieAction?.Invoke();
    }

    public void OnAction()
    {
        _onAction?.Invoke();
    }
}