using System;
using System.Collections.Generic;
using _DataTable.Script;
using Aloha.Coconut;
using Cysharp.Threading.Tasks;
using UnityEngine;

[Serializable]
public class PrisonUnitPoolTable
{
    [CSVColumn] public string id;
    [CSVColumn] public int amount;
    [CSVColumn] public int probability;
}

public class PrisonUnitPoolTableList : ITableList
{
    private List<PrisonUnitPoolTable> _etcTableList = new List<PrisonUnitPoolTable>();
    private readonly Dictionary<string, PrisonUnitPoolTable> _cachedTables = new Dictionary<string, PrisonUnitPoolTable>();

    public async UniTask Init()
    {
        _etcTableList = await TableManager.GetAsync<PrisonUnitPoolTable>("PrisonUnitPool");
    }

    public PrisonUnitPoolTable GetTable(string id)
    {
        if (_cachedTables.TryGetValue(id, out var objectInfo))
        {
            return objectInfo;
        }

        var info = _etcTableList.Find(a => a.id == id);
        if (info == null)
        {
            Debug.LogError($"EtcInfo not found. id: {id}");
            return null;
        }

        _cachedTables.Add(id, info);
        return info;
    }

    public List<PrisonUnitPoolTable> GetAllTables()
    {
        return _etcTableList;
    }
}