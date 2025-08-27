using System;
using UnityEngine;

public class BuildingAnimationEventHandler : MonoBehaviour
{
    private Action _onDestroyAction;

    public void Init(Action onDestroyAction)
    {
        _onDestroyAction = onDestroyAction;
    }

    // Animation Event
    public void OnDestroyBuilding()
    {
        _onDestroyAction?.Invoke();
    }
}