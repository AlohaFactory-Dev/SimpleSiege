using System;
using UnityEngine;

public class TrapBuilding : Building, ICaster
{
    public int EffectValue => EffectCalculator.CalculateEffectValue(BuildingTable.effectValue, BuildingTable.effectGrowth, StageConainer.Get<StageManager>().CurrentStageTable.enemyAttackPowerLevel);
    public string ProjectTileId => BuildingTable.stringValues[1];
    public string EffectVfxId => BuildingTable.stringValues[0];
    private float ExplosionRadius => BuildingTable.values[1];
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
        var units = Physics2D.OverlapCircleAll(transform.position, ExplosionRadius, LayerMask.GetMask("Unit"));
        foreach (var unitCollider in units)
        {
            if (unitCollider.TryGetComponent<ITarget>(out var target) && !target.IsUntargetable && target.TeamType == TeamType.Player)
            {
                target.TakeDamage(this);
            }
        }

        base.Remove();
    }
}