using System;
using System.Collections.Generic;
using _DataTable.Script;
using Aloha.Coconut;
using Cysharp.Threading.Tasks;
using UnityEngine;

[Serializable]
public class CardTable
{
    [CSVColumn] public string Id;
    [CSVColumn] public CardType cardType;
    [CSVColumn] public int requiredAmount;
    [CSVColumn] public int unitAmount;
    [CSVColumn] public string iconkey;
    [CSVColumn] public string namekey;
    [CSVColumn] public string descriptionkey;
    [CSVColumn] public List<float> values;
}

public class CardTableList : ITableList
{
    private List<EtcTable> _etcTableList = new List<EtcTable>();
    private readonly Dictionary<string, EtcTable> _cachedTables = new Dictionary<string, EtcTable>();

    public async UniTask Init()
    {
        _etcTableList = await TableManager.GetAsync<EtcTable>("Etc");
    }

    public EtcTable GetCardTable(string id)
    {
        if (_cachedTables.TryGetValue(id, out var objectInfo))
        {
            return objectInfo;
        }

        var info = _etcTableList.Find(a => a.Id == id);
        if (info == null)
        {
            Debug.LogError($"EtcInfo not found. id: {id}");
            return null;
        }

        _cachedTables.Add(id, info);
        return info;
    }
}