using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class BarrackBuilding : Building
{
    [Inject] private UnitManager _unitManager;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject boundary;
    private UnitDetector _unitDetector;
    private string SpawnUnitId => BuildingTable.stringValues[0];
    private int SpawnAmount => (int)BuildingTable.values[0];
    private float DetectorRadius => BuildingTable.values[1];
    private bool _hasUnits;
    private List<UnitController> _units = new();

    protected override void CustomInit()
    {
        _unitDetector = GetComponentInChildren<UnitDetector>();
        _hasUnits = true;
        Vector2 spawnPosition = (Vector2)spawnPoint.position + Random.insideUnitCircle;
        _units = _unitManager.SpawnUnit(spawnPosition, SpawnUnitId, SpawnAmount);
        _unitDetector.Init(OnDetect);
        _unitDetector.SetRadius(DetectorRadius);
    }


    private void OnDetect(UnitController unit)
    {
        if (unit.TeamType == TeamType.Player && _hasUnits)
        {
            EmissionUnits();
            _unitDetector.Off();
        }
    }

    public override void TakeDamage(ICaster caster)
    {
        if (_isDestroyed) return;
        EmissionUnits();
        base.TakeDamage(caster);
    }

    private void EmissionUnits()
    {
        if (_hasUnits)
        {
            _hasUnits = false;
            boundary.SetActive(false);
            foreach (var unit in _units)
            {
                unit.ChangeState(UnitState.Move);
            }

            _units.Clear();
        }
    }
}