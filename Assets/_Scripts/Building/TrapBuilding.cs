using System;
using FactorySystem;
using UniRx;
using UnityEngine;

public class TrapBuilding : Building, ICaster
{
    public int EffectValue => EffectCalculator.CalculateEffectValue(BuildingTable.effectValue, BuildingTable.effectGrowth, StageConainer.Get<StageManager>().CurrentStageTable.enemyAttackPowerLevel);
    public string ProjectTileId => BuildingTable.stringValues[1];
    public string EffectVfxId => BuildingTable.stringValues[0];
    public TargetType TargetType => TargetType.Enemy;
    public AreaType AreaType => AreaType.Circle;
    public float EffectRange => BuildingTable.values[1];
    public IReadOnlyReactiveProperty<float> EffectActionSpeed => new ReactiveProperty<float>(1f);
    private UnitDetector _unitDetector;
    private TrapAttackEventHandler _attackEventHandler;

    protected override void CustomInit()
    {
        Collider2D.isTrigger = true;
        _unitDetector = GetComponentInChildren<UnitDetector>();
        _attackEventHandler = GetComponentInChildren<TrapAttackEventHandler>();
        _unitDetector.Init(OnDetect);
        _attackEventHandler.Init(Attack);
    }

    private void OnDetect(UnitController unit)
    {
        if (unit.TeamType == TeamType.Player)
        {
            DestroyBuilding();
            _unitDetector.Off();
        }
    }

    private void Attack()
    {
        var units = Physics2D.OverlapCircleAll(transform.position, EffectRange, LayerMask.GetMask("Unit"));
        foreach (var unitCollider in units)
        {
            if (unitCollider.TryGetComponent<ITarget>(out var target) && !target.IsUntargetable && target.TeamType == TeamType.Player)
            {
                target.TakeDamage(this, EffectValue);
            }
        }

        var particle = StageConainer.Get<FactoryManager>().ParticleFactory.GetParticle("EVFX_Trap");
        particle.Init(transform.position);
        particle.Play();
    }
}