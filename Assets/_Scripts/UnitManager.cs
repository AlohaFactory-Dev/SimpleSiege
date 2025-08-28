using System.Collections.Generic;
using FactorySystem;
using UnityEngine;
using Zenject;

public class UnitManager : MonoBehaviour
{
    [Inject] private readonly FactoryManager _factoryManager;
    public List<UnitController> PlayerUnits { get; private set; } = new();
    public List<UnitController> EnemyUnits { get; private set; } = new();
    [SerializeField] private KingUnit playerKing;
    [SerializeField] private KingUnit enemyKing;

    public void Init()
    {
        playerKing.SetKingUnit();
        enemyKing.SetKingUnit();
        PlayerUnits.Add(playerKing);
        EnemyUnits.Add(enemyKing);
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

    public void RemoveUnit(UnitController unit)
    {
        if (unit.TeamType == TeamType.Player)
        {
            PlayerUnits.Remove(unit);
            if (unit == playerKing)
            {
                Debug.Log("Player King Defeated! Game Over.");
                // 게임 오버 처리 로직 추가
                StageConainer.Get<StageManager>().EndStage(false);
            }
        }
        else
        {
            EnemyUnits.Remove(unit);
            if (unit == enemyKing)
            {
                Debug.Log("Enemy King Defeated! You Win!");
                // 승리 처리 로직 추가
                StageConainer.Get<StageManager>().EndStage(true);
            }
        }
    }
}