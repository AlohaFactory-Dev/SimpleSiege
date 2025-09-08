using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class TempSpawnEnemyManager : MonoBehaviour
{
    private List<UnitTable> _enemyTables;
    [SerializeField] private Transform spawnPoint;
    [Inject] private UnitManager _unitManager;

    private void Start()
    {
        _enemyTables = TableListContainer.Get<UnitTableList>().GetTeamUnitTable(TeamType.Enemy);
        foreach (var VARIABLE in _enemyTables)
        {
            if (VARIABLE.id == "E_King")
            {
                _enemyTables.Remove(VARIABLE);
                break;
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        if (_enemyTables.Count == 0) return;
        int randomIndex = UnityEngine.Random.Range(0, _enemyTables.Count);
        _unitManager.SpawnUnit((Vector2)spawnPoint.position + new Vector2(Random.Range(0, 1), Random.Range(0, 1)), _enemyTables[randomIndex].id, 1);
    }
}