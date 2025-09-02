using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class BarrackBuilding : Building
{
    [Inject] private UnitManager _unitManager;
    [SerializeField] private Transform[] spawnPoint;
    private UnitDetector _unitDetector;
    private string SpawnUnitId => BuildingTable.stringValues[0];
    private float DetectorRadius => BuildingTable.values[1];
    private bool _hasUnits;
    private List<UnitController> _units = new();

    protected override void CustomInit()
    {
        _unitDetector = GetComponentInChildren<UnitDetector>();
        _hasUnits = true;
        _units = _unitManager.SpawnUnit(transform.position, SpawnUnitId, spawnPoint.Length);
        _unitDetector.Init(OnDetect);
        _unitDetector.SetRadius(DetectorRadius);
        for (int i = 0; i < _units.Count; i++)
        {
            _units[i].SetBarrackUnit(spawnPoint[i].position);
        }
    }


    private void OnDetect(UnitController unit)
    {
        if (unit.TeamType == TeamType.Player && _hasUnits)
        {
            EmissionUnits();
            _unitDetector.Off();
        }
    }

    public override void TakeDamage(ICaster caster, int damage)
    {
        if (IsDestroyed) return;
        EmissionUnits();
        base.TakeDamage(caster, damage);
    }

    private void EmissionUnits()
    {
        if (_hasUnits)
        {
            for (int i = 0; i < _units.Count; i++)
            {
                _units[i].ColliderActive(true);
            }

            _hasUnits = false;
            foreach (var unit in _units)
            {
                unit.ChangeState(UnitState.Move);
            }

            _units.Clear();
        }
    }
}