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

        _allUnit.AddRange(spawnedUnits);
        return spawnedUnits;
    }


    private float _sortingOrderUpdateInterval = 0.1f;
    private float _sortingOrderUpdateTimer = 0f;


    private void SetAllUnitSortingOrder()
    {
        // y값 기준 오름차순 정렬 (y가 낮은게 먼저)
        _allUnit.Sort((a, b) => a.transform.position.y.CompareTo(b.transform.position.y));
        // y가 낮은 유닛부터 sortingOrder를 높게 설정
        for (int i = 0; i < _allUnit.Count; i++)
        {
            // 예시: sortingOrder를 1000에서 시작해서 y가 낮을수록 높게
            _allUnit[i].SetSortingOrder(-i);
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