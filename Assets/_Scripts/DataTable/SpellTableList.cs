using System.Collections.Generic;
using _DataTable.Script;
using Aloha.Coconut;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class SpellTable
{
    [CSVColumn] public string id;
    [CSVColumn] public float effectValue;
    [CSVColumn] public float effectRange;
    [CSVColumn] public AreaType areaType;
    [CSVColumn] public TargetType targetType;
    [CSVColumn] public string spellFxId;
    [CSVColumn] public List<float> values;
}


public class SpellTableList : ITableList
{
    private List<SpellTable> _spellTables = new();
    private readonly Dictionary<string, SpellTable> _cachedInfo = new();

    public async UniTask Init()
    {
        _spellTables = await TableManager.GetAsync<SpellTable>("Spell");
        _cachedInfo.Clear();
    }

    public SpellTable GetSpellTable(string id)
    {
        if (_cachedInfo.TryGetValue(id, out var objectInfo))
        {
            return objectInfo;
        }

        var info = _spellTables.Find(a => a.id == id);
        if (info == null)
        {
            Debug.LogError($"AttackObjectInfo not found. id: {id}");
            return null;
        }

        _cachedInfo.Add(id, info);
        return info;
    }
}