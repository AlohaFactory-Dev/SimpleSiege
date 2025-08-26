using System;
using UnityEngine;

[RequireComponent(typeof(RecycleObject))]
public class UnitStatusSystem : MonoBehaviour
{
    private RecycleObject _recycleObject;
    public UnitMoveSystem MoveSystem { get; private set; }
    public UnitActionSystem ActionSystem { get; private set; }
    public UnitAnimationSystem AnimationSystem { get; private set; }
    public UnitHpSystem HpSystem { get; private set; }
    private Collider2D _collider2D;
    private bool _isInitialized;

    public void Init(UnitController unitController)
    {
        GetComponents();
        HpSystem.Init(unitController.UnitTable);
        MoveSystem.Init(unitController);
        AnimationSystem.Init(_recycleObject.Release);
        ActionSystem.Init(unitController, AnimationSystem);
        _collider2D.enabled = true;
    }

    private void GetComponents()
    {
        if (_isInitialized) return;
        _isInitialized = true;
        MoveSystem = GetComponent<UnitMoveSystem>();
        AnimationSystem = GetComponentInChildren<UnitAnimationSystem>();
        HpSystem = GetComponentInChildren<UnitHpSystem>();
        ActionSystem = GetComponent<UnitActionSystem>();
        _recycleObject = GetComponent<RecycleObject>();
        _collider2D = GetComponent<Collider2D>();
    }

    public void ApplyState(ITarget target, UnitState state)
    {
        if (state == UnitState.Move)
        {
            AnimationSystem.PlayMove();
            MoveSystem.StartMove(target);
            ActionSystem.StopAction();
        }
        else if (state == UnitState.Action)
        {
            AnimationSystem.PlayAction();
            MoveSystem.StopMove();
            ActionSystem.StartAction(target);
        }
        else if (state == UnitState.Dead)
        {
            AnimationSystem.PlayDead();
            _collider2D.enabled = false;
            ActionSystem.StopAction();
            MoveSystem.StopMove();
        }
        else
        {
            MoveSystem.StopMove();
            ActionSystem.StopAction();
            AnimationSystem.PlayIdle();
        }
    }
}