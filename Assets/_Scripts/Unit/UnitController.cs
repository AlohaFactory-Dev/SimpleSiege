using System;
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

public class UnitController : MonoBehaviour, ITarget, ICaster
{
    [Inject] FactoryManager _factoryManager;
    [SerializeField] private Transform damageEffectPoint;
    [SerializeField] private Transform floatingEffectPoint;
    private UnitTable _unitTable;
    private UnitStatusSystem _statusSystem;
    private UnitTargetSystem _targetSystem;

    private float _attackTimer;
    private ITarget _target;
    private bool _isInitialized;
    private UnitStateMachine _stateMachine;

    private string _floatingTextId = "FloatingText";

    public ReactiveProperty<UnitState> state = new(UnitState.Idle);

    public UnitTable UnitTable => _unitTable;
    public int EffectValue => _effectValue;
    private int _effectValue;


    public void Spawn(Vector3 position, UnitTable unitTable)
    {
        Init();
        _unitTable = unitTable;
        if (_unitTable.teamType == TeamType.Enemy)
        {
            var attackPowerLevel = StageConainer.Get<StageManager>().CurrentStageTable.enemyAttackPowerLevel;
            _effectValue = Mathf.CeilToInt(_unitTable.effectValue * Mathf.Pow(1 + _unitTable.effectGrowth, Math.Max(attackPowerLevel - 1, 0)));
        }
        else
        {
            _effectValue = _unitTable.effectValue;
        }

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
            if (_target == null)
                state.Value = UnitState.Move;
            else
            {
                float distance = Vector3.Distance(transform.position, _target.Transform.position);
                if (distance > _unitTable.effectAbleRange)
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

    public Transform Transform => transform;

    public void TakeDamage(ICaster caster)
    {
        var floatingText = _factoryManager.FloatingTextFactory.GetText(_floatingTextId);
        floatingText.SetText(caster.EffectValue.ToString());
        floatingText.Play(floatingEffectPoint.position);

        var particle = _factoryManager.ParticleFactory.GetParticle(caster.UnitTable.effectVfxId);
        particle.Init(damageEffectPoint.position);
        particle.Play();
        if (_statusSystem.HpSystem.TakeDamage(caster.EffectValue))
        {
            state.Value = UnitState.Dead;
        }
    }
}