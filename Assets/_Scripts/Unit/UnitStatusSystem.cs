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
        ActionSystem.Init(unitController);
        AnimationSystem.Init(_recycleObject.Release);
        _collider2D.enabled = true;
    }

    private void GetComponents()
    {
        if (_isInitialized) return;
        _isInitialized = true;
        MoveSystem = GetComponentInChildren<UnitMoveSystem>();
        ActionSystem = GetComponentInChildren<UnitActionSystem>();
        AnimationSystem = GetComponentInChildren<UnitAnimationSystem>();
        HpSystem = GetComponentInChildren<UnitHpSystem>();
        _recycleObject = GetComponent<RecycleObject>();
        _collider2D = GetComponent<Collider2D>();
    }

    public void ApplyState(Transform target, UnitState state)
    {
        if (state == UnitState.Move)
        {
            MoveSystem.StartMove(target);
            ActionSystem.StopAction();
            AnimationSystem.PlayMove();
        }
        else if (state == UnitState.Action)
        {
            ActionSystem.StartAction(target);
            MoveSystem.StopMove();
            AnimationSystem.PlayAttack();
        }
        else if (state == UnitState.Dead)
        {
            _collider2D.enabled = false;
            ActionSystem.StopAction();
            MoveSystem.StopMove();
            AnimationSystem.PlayDead();
        }
        else
        {
            MoveSystem.StopMove();
            ActionSystem.StopAction();
            AnimationSystem.PlayIdle();
        }
    }
}