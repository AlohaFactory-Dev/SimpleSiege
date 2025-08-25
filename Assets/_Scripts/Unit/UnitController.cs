using FactorySystem;
using UnityEngine;
using UniRx;
using Zenject;

public enum UnitState
{
    Idle,
    Move,
    Action,
    Dead
}

public class UnitController : MonoBehaviour
{
    [Inject] FactoryManager _factoryManager;
    [SerializeField] private Transform damageEffectPoint;
    [SerializeField] private Transform floatingEffectPoint;
    private UnitTable _unitTable;
    private UnitStatusSystem _statusSystem;
    private UnitTargetSystem _targetSystem;

    private float _attackTimer;
    private Transform _target;
    private bool _isInitialized;
    private UnitStateMachine _stateMachine;

    private string _floatingTextId = "FloatingText";

    public ReactiveProperty<UnitState> state = new(UnitState.Idle);

    public UnitTable UnitTable => _unitTable;


    public void Spawn(Vector3 position, UnitTable unitTable)
    {
        Init();
        _unitTable = unitTable;

        transform.position = position;
        _statusSystem.Init(this);
        _targetSystem.Init(this);
        _stateMachine = new UnitStateMachine(_statusSystem);
        state.Value = UnitState.Move;
    }

    private void Init()
    {
        if (_isInitialized) return;
        _isInitialized = true;
        _statusSystem = GetComponentInChildren<UnitStatusSystem>();
        _targetSystem = GetComponentInChildren<UnitTargetSystem>();

        state
            .DistinctUntilChanged()
            .Subscribe(newState => _stateMachine.OnStateChanged(_target, newState))
            .AddTo(this);
    }

    private void Update()
    {
        if (state.Value == UnitState.Move)
        {
            _target = _targetSystem.FindTarget();
            if (!_target)
                state.Value = UnitState.Move;
            else
            {
                float distance = Vector3.Distance(transform.position, _target.position);
                if (distance > _unitTable.attackAbleRange)
                    state.Value = UnitState.Move;
                else
                    state.Value = UnitState.Action;
            }
        }
    }

    public void OnActionEnd()
    {
        state.Value = UnitState.Move;
    }

    public void TakeDamage(int damage, string fxId)
    {
        var floatingText = _factoryManager.FloatingTextFactory.GetText(_floatingTextId);
        floatingText.SetText(damage.ToString());
        floatingText.Play(floatingEffectPoint.position);

        var particle = _factoryManager.ParticleFactory.GetParticle(fxId);
        particle.Init(damageEffectPoint.position);
        particle.Play();
        if (_statusSystem.HpSystem.TakeDamage(damage))
        {
            state.Value = UnitState.Dead;
        }
    }
}