using System.Collections.Generic;
using _DataTable.Script;
using Aloha.Coconut;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class UnitTable
{
    public string id;
    public int attackPower;
    public float attackInterval;
    public int hp;
    public float moveSpeed;
    public float sightRange;
    public float attackAbleRange;
    public TargetType targetType;
    public AreaType areaType;
    public float attackRange;
    public AttackType attackType;
    public string attackObjectId;
    public TargetSelectionType targetSelectionType;
    public TeamType teamType;
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