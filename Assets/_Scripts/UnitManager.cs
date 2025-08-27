using System.Collections.Generic;
using FactorySystem;
using UnityEngine;

public class UnitManager
{
    private readonly FactoryManager _factoryManager;
    public List<UnitController> PlayerUnits { get; private set; } = new();
    public List<UnitController> EnemyUnits { get; private set; } = new();


    public UnitManager(FactoryManager factoryManager)
    {
        _factoryManager = factoryManager;
    }

    public List<UnitController> SpawnUnit(Vector2 spawnPosition, string id, int amount, bool onAutoMove = true)
    {
        var table = TableListContainer.Get<UnitTableList>().GetUnitTable(id);
        var spawnedUnits = new List<UnitController>();
        for (int i = 0; i < amount; i++)
        {
            var offset = Random.insideUnitCircle * 0.5f; // 약간의 랜덤 오프셋 추가
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

        return spawnedUnits;
    }
}