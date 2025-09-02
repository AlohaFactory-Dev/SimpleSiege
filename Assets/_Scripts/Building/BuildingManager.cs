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
        // EnemyBuilding이 파괴된 경우
        if (_enemyBuildings.Count > 0)
        {
            // 남아있는 EnemyBuilding 중 y가 가장 낮은 건물 찾기
            Building lowestYBuilding = _enemyBuildings[0];
            foreach (var b in _enemyBuildings)
            {
                if (b.transform.position.y < lowestYBuilding.transform.position.y)
                    lowestYBuilding = b;
            }

            // 파괴된 건물이 가장 낮은 y값을 가진 건물이었는지 확인
            if (destroyedBuilding.transform.position.y <= lowestYBuilding.transform.position.y)
            {
                OnNearestEnemyBuildingDestroyed.OnNext(destroyedBuilding.transform.position);
            }
        }
        // else
        // {
        //     // 마지막 EnemyBuilding이 파괴된 경우에도 실행
        //     OnNearestEnemyBuildingDestroyed.OnNext(destroyedBuilding.transform.position);
        // }
    }
}