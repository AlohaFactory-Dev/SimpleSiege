using _Scripts.Unit;
using UnityEngine;
using UniRx;
using Zenject;

public enum UnitState
{
    Idle,
    Move,
    Action
}

public class UnitController : MonoBehaviour
{
    private UnitTable _unitTable;
    private UnitStatusSystem _statusSystem;
    private UnitTargetSystem _targetSystem;

    private float _attackTimer;
    private Transform _target;
    public ReactiveProperty<UnitState> State = new(UnitState.Idle);

    public UnitTable UnitTable => _unitTable;

    public void Init(UnitTable unitTable)
    {
        _unitTable = unitTable;
        _statusSystem = GetComponentInChildren<UnitStatusSystem>();
        _targetSystem = GetComponentInChildren<UnitTargetSystem>();

        _statusSystem.Init(this);
        _targetSystem.Init(this);

        State.Value = UnitState.Move;
        State
            .DistinctUntilChanged()
            .Subscribe(OnStateChanged)
            .AddTo(this);
    }

    private void Update()
    {
        if (State.Value == UnitState.Move)
        {
            _target = _targetSystem.FindTarget();
            if (!_target)
                State.Value = UnitState.Move;
            else
            {
                float distance = Vector3.Distance(transform.position, _target.position);
                if (distance > _unitTable.attackAbleRange)
                    State.Value = UnitState.Move;
                else
                    State.Value = UnitState.Action;
            }
        }
    }

    public void OnActionEnd()
    {
        State.Value = UnitState.Move;
    }

    private void OnStateChanged(UnitState newState)
    {
        _statusSystem.ApplyState(_target, newState);
    }
}