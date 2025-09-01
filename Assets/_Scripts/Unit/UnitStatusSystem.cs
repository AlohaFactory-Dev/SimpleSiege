using System;
using UnityEngine;

[RequireComponent(typeof(RecycleObject))]
public class UnitStatusSystem : MonoBehaviour
{
    private RecycleObject _recycleObject;
    public UnitMoveSystem MoveSystem { get; private set; }
    public UnitActionSystem ActionSystem { get; private set; }
    public UnitAnimationSystem AnimationSystem { get; private set; }
    public UnitRotationSystem RotationSystem { get; private set; }
    public HpSystem HpSystem { get; private set; }
    private UnitTargetSystem _targetSystem;
    private Collider2D _collider2D;
    private bool _isInitialized;

    public void Init(UnitController unitController)
    {
        GetComponents();
        _targetSystem.Init(unitController);
        HpSystem.Init(unitController.MaxHp, unitController.TeamType, unitController.UnitTable.maxHp);
        MoveSystem.Init(unitController, _targetSystem, RotationSystem);
        AnimationSystem.Init(_recycleObject.Release, unitController);
        ActionSystem.Init(unitController, AnimationSystem, _targetSystem, RotationSystem);
        _collider2D = GetComponent<Collider2D>();
    }

    private void GetComponents()
    {
        if (_isInitialized) return;
        _isInitialized = true;
        MoveSystem = GetComponent<UnitMoveSystem>();
        AnimationSystem = GetComponentInChildren<UnitAnimationSystem>();
        HpSystem = GetComponentInChildren<HpSystem>();
        ActionSystem = GetComponent<UnitActionSystem>();
        _recycleObject = GetComponent<RecycleObject>();
        _targetSystem = GetComponentInChildren<UnitTargetSystem>();
        RotationSystem = GetComponent<UnitRotationSystem>();
    }

    public void ForceRelease()
    {
        _collider2D.enabled = false;
        ActionSystem.StopAction();
        MoveSystem.StopMove();
        _recycleObject.Release();
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
            case UnitState.Siege:
                AnimationSystem.PlayIdle();
                ActionSystem.StartSiegeAction();
                MoveSystem.StopMove();
                break;

            default:
                Debug.LogError("Unknown state: " + state);
                break;
        }
    }

    public void ApplyHitAnimation()
    {
        AnimationSystem.PlayHit();
    }
}