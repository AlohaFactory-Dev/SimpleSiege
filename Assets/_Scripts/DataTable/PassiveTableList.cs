using System;
using System.Collections.Generic;
using _DataTable.Script;
using Aloha.Coconut;
using Cysharp.Threading.Tasks;
using UnityEngine;

[Serializable]
public class PassiveTable
{
    [CSVColumn] public string id;
    [CSVColumn] public string nameKey;
    [CSVColumn] public string descriptionKey;
    [CSVColumn] public string iconKey;
    [CSVColumn] public UpgradeValueType upgradeValueType;
    [CSVColumn] public float effectValue;
    [CSVColumn] public int probability;
    [CSVColumn] public UpgradeType passiveType;
    [CSVColumn] public List<string> targetIds;
    [CSVColumn] public List<float> values;
    [CSVColumn] public List<string> etcIds;
}

public class PassiveTableList : ITableList
{
    private List<PassiveTable> _etcTableList = new List<PassiveTable>();
    private readonly Dictionary<string, PassiveTable> _cachedTables = new Dictionary<string, PassiveTable>();

    public async UniTask Init()
    {
        _etcTableList = await TableManager.GetAsync<PassiveTable>("Passive");
    }

    public PassiveTable GetPassiveTable(string id)
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

    public List<PassiveTable> GetAllPassiveTables()
    {
        return _etcTableList;
    }
}