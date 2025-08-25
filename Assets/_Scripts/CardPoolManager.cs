using UniRx;
using UnityEngine;

public class CardPoolManager
{
    public IReadOnlyReactiveDictionary<string, int> CardDict => _cardDict;
    private readonly ReactiveDictionary<string, int> _cardDict = new();

    public void SetCardPool(CardTable table)
    {
        if (_cardDict.ContainsKey(table.id))
        {
            Debug.LogError($"Card pool with id '{table.id}' already exists!");
        }
        else
        {
            _cardDict.Add(table.id, table.cardAmount);
        }
    }

    public bool ConsumeCard(string cardId)
    {
        if (_cardDict.TryGetValue(cardId, out var currentAmount) && currentAmount >= 0)
        {
            _cardDict[cardId]--;
            return true;
        }

        Debug.LogWarning($"Not enough cards of id '{cardId}' to consume.");
        return false;
    }
}