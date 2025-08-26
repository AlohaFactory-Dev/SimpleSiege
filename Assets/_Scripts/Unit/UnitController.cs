using System;
using System.Collections;
using System.Collections.Generic;
using FactorySystem;
using UnityEngine;
using UniRx;
using Zenject;

public enum UnitState
{
    Spawn,
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

    private float _attackTimer;
    private ITarget _target;
    private bool _isInitialized;

    private string _floatingTextId = "FloatingText";

    [SerializeField] private UnitState state;

    public UnitTable UnitTable => _unitTable;
    public int EffectValue => _effectValue;
    public int MaxHp => _maxHp;
    public bool IsDead => _statusSystem.HpSystem.IsDead;
    public bool IsUntargetable => state == UnitState.Dead || state == UnitState.Spawn;
    private int _effectValue;
    private int _maxHp;
    public Rigidbody2D Rigidbody2D { get; private set; }
    public CircleCollider2D CircleCollider2D { get; private set; }
    public Transform Transform => transform;

    public void Spawn(Vector3 position, UnitTable unitTable)
    {
        Init();
        _unitTable = unitTable;
        if (_unitTable.teamType == TeamType.Enemy)
        {
            var stageTable = StageConainer.Get<StageManager>().CurrentStageTable;
            _effectValue = Mathf.CeilToInt(_unitTable.effectValue * Mathf.Pow(1 + _unitTable.effectGrowth, Math.Max(stageTable.enemyAttackPowerLevel - 1, 0)));
            _maxHp = Mathf.CeilToInt(_unitTable.maxHp * Mathf.Pow(1 + _unitTable.maxHpGrowth, Math.Max(stageTable.enemyHpLevel - 1, 0)));
        }
        else
        {
            _effectValue = _unitTable.effectValue;
            _maxHp = _unitTable.maxHp;
        }

        transform.position = position;
        _statusSystem.Init(this);
        state = UnitState.Spawn;
        ChangeState(UnitState.Spawn);


        StartCoroutine(WaitAndMove(_unitTable.idleTimeAfterSpawn));
    }

    private IEnumerator WaitAndMove(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        ChangeState(UnitState.Move);
    }

    private void Init()
    {
        if (_isInitialized) return;
        _isInitialized = true;
        _statusSystem = GetComponentInChildren<UnitStatusSystem>();
        Rigidbody2D = GetComponent<Rigidbody2D>();
        CircleCollider2D = GetComponent<CircleCollider2D>();
    }

    public void ChangeState(UnitState newState, ITarget target = null)
    {
        if (state == UnitState.Dead) return;
        state = newState;
        _statusSystem.ApplyState(target, newState);
    }


    public void TakeDamage(ICaster caster)
    {
        if (IsDead) return;
        if (state == UnitState.Spawn)
            return;

        var floatingText = _factoryManager.FloatingTextFactory.GetText(_floatingTextId);
        floatingText.SetText(caster.EffectValue.ToString());
        floatingText.Play(floatingEffectPoint.position);

        var particle = _factoryManager.ParticleFactory.GetParticle(caster.UnitTable.effectVfxId);
        particle.Init(damageEffectPoint.position);
        particle.Play();
        if (_statusSystem.HpSystem.TakeDamage(caster.EffectValue))
        {
            ChangeState(UnitState.Dead);
        }
    }
}