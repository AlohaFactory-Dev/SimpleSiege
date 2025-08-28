using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class WallBuilding : Building
{
    [SerializeField] private Transform spawnPoint;
    [Inject] private UnitManager _unitManager;
    private string SpawnUnitId => BuildingTable.stringValues[0];
    private int SpawnAmount => (int)BuildingTable.values[0];
    private float EffectAbleRange => BuildingTable.values[1];
    private List<UnitController> _units = new();

    protected override void CustomInit()
    {
        if (string.IsNullOrEmpty(SpawnUnitId)) return;
        Vector2 spawnPosition = (Vector2)spawnPoint.position + Random.insideUnitCircle;
        var units = _unitManager.SpawnUnit(spawnPosition, SpawnUnitId, SpawnAmount, false);
        _units.AddRange(units);
        foreach (var unit in _units)
        {
            unit.SetWallUnit("Wall", EffectAbleRange);
        }
    }

    protected override void DestroyBuilding()
    {
        foreach (var unit in _units)
        {
            unit.ForceRelease();
        }

        _units.Clear();
        base.DestroyBuilding();
    }
}