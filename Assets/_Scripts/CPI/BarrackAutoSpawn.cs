using System;
using UnityEngine;

public class BarrackAutoSpawn : MonoBehaviour
{
    [SerializeField] private float autoSpawnInterval;
    [SerializeField] private string unitId;
    [SerializeField] private float spawnCount;
    private float _timer;

    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= autoSpawnInterval)
        {
            _timer = 0f;
            var units = StageConainer.Get<UnitManager>().SpawnUnit(transform.position, unitId, (int)spawnCount, false);
            foreach (var unit in units)
            {
                unit.ChangeState(UnitState.Move, null, true);
            }
        }
    }
}