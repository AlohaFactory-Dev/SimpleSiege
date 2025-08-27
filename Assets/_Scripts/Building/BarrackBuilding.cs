using UnityEngine;
using Zenject;

public class WallBuilding : Building
{
    [Inject] private UnitManager _unitManager;
    [SerializeField] private Transform spawnPoint;
    private string SpawnUnitId => BuildingTable.stringValues[0];
    private int SpawnAmount => (int)BuildingTable.values[0];

    protected override void CustomInit()
    {
        Vector2 spawnPosition = (Vector2)spawnPoint.position + Random.insideUnitCircle;
        _unitManager.SpawnUnit(spawnPosition, SpawnUnitId, SpawnAmount);
    }
}