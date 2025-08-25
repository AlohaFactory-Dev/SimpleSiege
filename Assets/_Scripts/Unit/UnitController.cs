using UnityEngine;
using UniRx;

public enum UnitState
{
    Idle,
    Move,
    Action,
    Dead
}

public class UnitController : MonoBehaviour
{
    private UnitTable _unitTable;
    private UnitStatusSystem _statusSystem;
    private UnitTargetSystem _targetSystem;

    private float _attackTimer;
    private Transform _target;
    private bool _isInitialized;
    private UnitStateMachine _stateMachine;

    public ReactiveProperty<UnitState> State = new(UnitState.Idle);

    public UnitTable UnitTable => _unitTable;
    [SerializeField] private Collider2D collider2d;


    public void Spawn(Vector3 position, UnitTable unitTable)
    {
        Init();
        _unitTable = unitTable;

        transform.position = position;
        _statusSystem.Init(this);
        _targetSystem.Init(this);
        _stateMachine = new UnitStateMachine(_statusSystem);
        State.Value = UnitState.Move;
    }

    private void Init()
    {
        if (_isInitialized) return;
        _isInitialized = true;
        _statusSystem = GetComponentInChildren<UnitStatusSystem>();
        _targetSystem = GetComponentInChildren<UnitTargetSystem>();

        State
            .DistinctUntilChanged()
            .Subscribe(newState => _stateMachine.OnStateChanged(_target, newState))
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

    public void TakeDamage(int damage)
    {
        if (_statusSystem.HpSystem.TakeDamage(damage))
        {
            Die();
        }
    }

    private void Die()
    {
        State.Value = UnitState.Dead;
    }
}