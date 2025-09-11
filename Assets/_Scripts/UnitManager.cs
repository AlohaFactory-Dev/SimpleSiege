using System.Collections.Generic;
using FactorySystem;
using UnityEngine;
using Zenject;

public class UnitManager
{
    [Inject] private readonly FactoryManager _factoryManager;
    public List<UnitController> PlayerUnits { get; private set; } = new();
    public List<UnitController> EnemyUnits { get; private set; } = new();
    private List<UnitController> _allUnit = new();


    public List<UnitController> SpawnUnit(Vector2 spawnPosition, string id, int amount, bool onAutoMove = true, bool isRandomOffset = true)
    {
        var table = TableListContainer.Get<UnitTableList>().GetUnitTable(id);
        var spawnedUnits = new List<UnitController>();
        for (int i = 0; i < amount; i++)
        {
            var offset = isRandomOffset ? Random.insideUnitCircle * 0.5f : Vector2.zero;
            var unit = _factoryManager.UnitFactroy.GetUnit(table.id);
            unit.Spawn(spawnPosition + offset, table, onAutoMove);
            // 유닛의 팀 설정
            if (table.teamType == TeamType.Player)
            {
                PlayerUnits.Add(unit);
            }
            else
            {
                EnemyUnits.Add(unit);
            }

            spawnedUnits.Add(unit);
        }

        _allUnit.AddRange(spawnedUnits);
        return spawnedUnits;
    }

    public void SetDeployedUnit(UnitController[] units)
    {
        foreach (var unit in units)
        {
            var table = TableListContainer.Get<UnitTableList>().GetUnitTable(unit.id);
            unit.Spawn(unit.transform.position, table, false, true);
            unit.Collider2D.enabled = false;
            if (unit.TeamType == TeamType.Player)
            {
                if (!PlayerUnits.Contains(unit))
                    PlayerUnits.Add(unit);
            }
            else
            {
                if (!EnemyUnits.Contains(unit))
                    EnemyUnits.Add(unit);
            }

            if (!_allUnit.Contains(unit))
                _allUnit.Add(unit);
        }
    }


    public void RemoveUnit(UnitController unit)
    {
        if (unit.TeamType == TeamType.Player)
        {
            PlayerUnits.Remove(unit);
        }
        else
        {
            EnemyUnits.Remove(unit);
        }

        _allUnit.Remove(unit);
    }
}