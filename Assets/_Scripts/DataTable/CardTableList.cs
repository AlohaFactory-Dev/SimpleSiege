using System;
using System.Collections.Generic;
using _DataTable.Script;
using Aloha.Coconut;
using Cysharp.Threading.Tasks;
using UnityEngine;

[Serializable]
public class CardTable
{
    [CSVColumn] public string id;
    [CSVColumn] public CardType cardType;
    [CSVColumn] public float holdTime;
    [CSVColumn] public float spawnInterval;
    [CSVColumn] public int cardAmount;
    [CSVColumn] public int unitAmount;
    [CSVColumn] public string iconKey;
    [CSVColumn] public string nameKey;
    [CSVColumn] public string descriptionKey;
    [CSVColumn] public List<float> values;
}

public class CardTableList : ITableList
{
    private List<CardTable> _etcTableList = new List<CardTable>();
    private readonly Dictionary<string, CardTable> _cachedTables = new Dictionary<string, CardTable>();

    public async UniTask Init()
    {
        _etcTableList = await TableManager.GetAsync<CardTable>("Card");
    }

    public CardTable GetCardTable(string id)
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

    public List<CardTable> GetAllCardTable()
    {
        return _etcTableList;
    }
}