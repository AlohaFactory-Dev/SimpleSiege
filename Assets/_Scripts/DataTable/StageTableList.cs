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
public class StageTable
{
    [CSVColumn] public string stageId;
    [CSVColumn] public int stageNumber;
    [CSVColumn] public string mapId;
    [CSVColumn] public string stageIconId;
    [CSVColumn] public string stageNameKey;
    [CSVColumn] public string stageDescriptionKey;
    [CSVColumn] public int enemyAttackPowerLevel;
    [CSVColumn] public int enemyHpLevel;
}

public class StageTableList : ITableList
{
    private List<StageTable> _stageTableList = new();
    private readonly Dictionary<int, StageTable> _cachedInfo = new();

    public async UniTask Init()
    {
        _stageTableList = await TableManager.GetAsync<StageTable>("Stage");
    }

    public StageTable GetStageTable(int stageNumber)
    {
        if (_cachedInfo.TryGetValue(stageNumber, out var stageInfo))
        {
            return stageInfo;
        }

        var info = _stageTableList.Find(a => a.stageNumber == stageNumber);
        if (info == null)
        {
            Debug.LogError($"StageInfo not found. stageNumber: {stageNumber}");
            return null;
        }

        _cachedInfo.Add(stageNumber, info);
        return info;
    }
}