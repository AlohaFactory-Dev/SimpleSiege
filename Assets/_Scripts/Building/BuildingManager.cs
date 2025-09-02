using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    private List<Building> _allBuildings = new List<Building>();
    private List<Building> _playerBuildings = new List<Building>();
    private List<Building> _enemyBuildings = new List<Building>();
    public ISubject<Vector2> OnNearestEnemyBuildingDestroyed = new Subject<Vector2>();
    public ISubject<TeamType> OnStageResult = new Subject<TeamType>();

    public void Init()
    {
        var buildings = GetComponentsInChildren<Building>();
        foreach (var building in buildings)
        {
            building.Init();
        }
    }

    public void AddBuilding(Building building)
    {
        if (!_allBuildings.Contains(building))
            _allBuildings.Add(building);
        if (building.TeamType == TeamType.Player)
        {
            if (!_playerBuildings.Contains(building))
                _playerBuildings.Add(building);
        }
        else
        {
            if (!_enemyBuildings.Contains(building))
                _enemyBuildings.Add(building);
        }
    }

    public void RemoveBuilding(Building building)
    {
        if (_allBuildings.Contains(building))
            _allBuildings.Remove(building);

        if (building.TeamType == TeamType.Player)
        {
            if (_playerBuildings.Contains(building))
                _playerBuildings.Remove(building);
        }
        else
        {
            if (_enemyBuildings.Contains(building))
            {
                _enemyBuildings.Remove(building);
                GetNearestEnemyBuilding(building);
            }
        }

        CheckAllBuildingsDestroyed(building.TeamType);
    }

    private void CheckAllBuildingsDestroyed(TeamType teamType)
    {
        if (teamType == TeamType.Player && _playerBuildings.Count == 0)
        {
            OnStageResult.OnNext(TeamType.Enemy);
        }
        else if (teamType == TeamType.Enemy && _enemyBuildings.Count == 0)
        {
            OnStageResult.OnNext(TeamType.Player);
        }
    }

    private void GetNearestEnemyBuilding(Building destroyedBuilding)
    {
        // 파괴되지 않은 빌딩만 필터링
        var validEnemyBuildings = _enemyBuildings.FindAll(b => b);

        if (validEnemyBuildings.Count > 0)
        {
            Building lowestYBuilding = validEnemyBuildings[0];
            foreach (var b in validEnemyBuildings)
            {
                if (b.transform.position.y < lowestYBuilding.transform.position.y)
                    lowestYBuilding = b;
            }

            if (destroyedBuilding.transform.position.y <= lowestYBuilding.transform.position.y)
            {
                OnNearestEnemyBuildingDestroyed.OnNext(destroyedBuilding.transform.position);
            }
        }
    }
}