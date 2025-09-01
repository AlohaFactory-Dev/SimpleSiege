using System;
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


    protected override void CustomInit()
    {
        _unitDetector = GetComponentInChildren<UnitDetector>();
        _unitDetector.Init(OnDetect);
    }

    private void OnDetect(UnitController unit)
    {
        if (unit.TeamType == TeamType.Player)
        {
            DestroyBuilding();
            _unitDetector.Off();
        }
    }

    protected override void Remove()
    {
        var units = Physics2D.OverlapCircleAll(transform.position, EffectRange, LayerMask.GetMask("Unit"));
        foreach (var unitCollider in units)
        {
            if (unitCollider.TryGetComponent<ITarget>(out var target) && !target.IsUntargetable && target.TeamType == TeamType.Player)
            {
                target.TakeDamage(this, EffectValue);
            }
        }

        base.Remove();
    }
}