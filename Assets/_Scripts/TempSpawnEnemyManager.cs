using System;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class TempSpawnEnemyManager : MonoBehaviour
{
    private UnitTable[] _enemyTables;
    [SerializeField] private Transform spawnPoint;
    [Inject] private UnitManager _unitManager;

    private void Start()
    {
        _enemyTables = TableListContainer.Get<UnitTableList>().GetTeamUnitTable(TeamType.Enemy).ToArray();
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
        if (_enemyTables.Length == 0) return;
        int randomIndex = UnityEngine.Random.Range(0, _enemyTables.Length);
        _unitManager.SpawnUnit((Vector2)spawnPoint.position + new Vector2(Random.Range(0, 1), Random.Range(0, 1)), _enemyTables[randomIndex].id, 1);
    }
}