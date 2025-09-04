using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class WallBuilding : Building
{
    [SerializeField] private Transform[] spawnPoints;
    [Inject] private UnitManager _unitManager;
    private string SpawnUnitId => BuildingTable.stringValues[0];
    private float EffectAbleRange => BuildingTable.values[0];
    private List<UnitController> _units = new();

    protected override void CustomInit()
    {
        if (string.IsNullOrEmpty(SpawnUnitId)) return;

        var units = _unitManager.SpawnUnit(transform.position, SpawnUnitId, spawnPoints.Length, false);
        _units.AddRange(units);

        for (int i = 0; i < _units.Count; i++)
        {
            var spawnPoint = spawnPoints[i % spawnPoints.Length];
            units[i].SetWallUnit("Wall", EffectAbleRange, spawnPoint.position);
        }
    }

    protected override void DestroyBuilding()
    {
        foreach (var unit in _units)
        {
            unit.ChangeState(UnitState.Dead);
        }

        base.DestroyBuilding();
    }

    protected override void Remove()
    {
        foreach (var unit in _units)
        {
            unit.ForceRelease();
        }

        _units.Clear();
        base.Remove();
    }
}