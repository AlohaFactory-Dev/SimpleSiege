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
        HpSystem.Init(unitController.MaxHp);
        MoveSystem.Init(unitController);
        AnimationSystem.Init(_recycleObject.Release);
        ActionSystem.Init(unitController, AnimationSystem);
        _collider2D = GetComponent<Collider2D>();
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
    }


    public void ApplyState(ITarget target, UnitState state)
    {
        switch (state)
        {
            case UnitState.Idle:
                AnimationSystem.PlayIdle();
                MoveSystem.StopMove();
                break;
            case UnitState.Spawn:
                AnimationSystem.PlaySpawn();
                ActionSystem.StopAction();
                MoveSystem.StopMove();
                _collider2D.enabled = true;
                break;
            case UnitState.Move:
                AnimationSystem.PlayMove();
                ActionSystem.StopAction();
                MoveSystem.StartMove();
                break;
            case UnitState.Action:
                AnimationSystem.PlayAction();
                ActionSystem.StartAction(target);
                MoveSystem.StopMove();
                break;
            case UnitState.Dead:
                AnimationSystem.PlayDead();
                ActionSystem.StopAction();
                MoveSystem.StopMove();
                _collider2D.enabled = false;
                break;
            default:
                Debug.LogError("Unknown state: " + state);
                break;
        }
    }
}