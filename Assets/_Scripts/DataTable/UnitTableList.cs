using System.Collections.Generic;
using _DataTable.Script;
using Aloha.Coconut;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class UnitTable
{
    public string id;
    public int effectValue;
    public float effectGrowth;
    public float actionInterval;
    public int maxHp;
    public float maxHpGrowth;
    public float moveSpeed;
    public float sightRange;
    public float effectAbleRange;
    public TargetType targetType;
    public AreaType areaType;
    public float effectRange;
    public EffectType effectType;
    public string projectTileId;
    public TargetSelectionType targetSelectionType;
    public TeamType teamType;
    public string effectVfxId;
    public TargetGroup targetGroup;
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
            Debug.LogError($"AttackObjectInfo not found. id: {id}");
            return null;
        }

        _cachedInfo.Add(id, info);
        return info;
    }
}