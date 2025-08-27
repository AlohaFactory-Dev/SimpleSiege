using System;
using System.Collections.Generic;
using _DataTable.Script;
using Aloha.Coconut;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class BuildingTable
{
    [CSVColumn] public string id;
    [CSVColumn] public int maxHp;
    [CSVColumn] public float maxHpGrowth;
    [CSVColumn] public int effectValue;
    [CSVColumn] public float effectGrowth;
    [CSVColumn] public List<float> values;
    [CSVColumn] public List<string> stringValues;
}

public class BuildingTableLis : ITableList
{
    private List<BuildingTable> _buildingTables = new();
    private readonly Dictionary<string, BuildingTable> _cachedInfo = new();

    public async UniTask Init()
    {
        _buildingTables = await TableManager.GetAsync<BuildingTable>("Building");
    }

    public BuildingTable GetBuildingTable(string id)
    {
        if (_cachedInfo.TryGetValue(id, out var stageInfo))
        {
            return stageInfo;
        }

        var info = _buildingTables.Find(a => a.id == id);
        if (info == null)
        {
            Debug.LogError($"BuildingTable not found. id: {id}");
            return null;
        }

        _cachedInfo.Add(id, info);
        return info;
    }
}