using System.Collections.Generic;
using System.Linq;
using _DataTable.Script;
using Aloha.Coconut;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class UnitTable
{
    [CSVColumn] public string id;
    [CSVColumn] public float mass;
    [CSVColumn] public int effectValue;
    [CSVColumn] public float effectGrowth;
    [CSVColumn] public float actionInterval;
    [CSVColumn] public int maxHp;
    [CSVColumn] public float maxHpGrowth;
    [CSVColumn] public float moveSpeed;
    [CSVColumn] public float sightRange;
    [CSVColumn] public float effectAbleRange;
    [CSVColumn] public TargetType targetType;
    [CSVColumn] public AreaType areaType;
    [CSVColumn] public float effectRange;
    [CSVColumn] public EffectType effectType;
    [CSVColumn] public string projectTileId;
    [CSVColumn] public TargetSelectionType targetSelectionType;
    [CSVColumn] public TeamType teamType;
    [CSVColumn] public string effectVfxId;
    [CSVColumn] public TargetGroup targetGroup;
    [CSVColumn] public float idleTimeAfterSpawn;
    [CSVColumn] public List<float> values = new();
}


public class UnitTableList : ITableList
{
    private List<UnitTable> _attackObjectTables = new();
    private readonly Dictionary<string, UnitTable> _cachedInfo = new();

    public async UniTask Init()
    {
        _attackObjectTables = await TableManager.GetAsync<UnitTable>("Unit");
        _cachedInfo.Clear();
    }

    public UnitTable GetUnitTable(string id)
    {
        if (_cachedInfo.TryGetValue(id, out var objectInfo))
        {
            return objectInfo;
        }

        var info = _attackObjectTables.Find(a => a.id == id);
        if (info == null)
        {
            Debug.LogError($"UnitTable not found. id: {id}");
            return null;
        }

        _cachedInfo.Add(id, info);
        return info;
    }

    public List<UnitTable> GetTeamUnitTable(TeamType teamType)
    {
        return _attackObjectTables.FindAll(a => a.teamType == teamType);
    }
}