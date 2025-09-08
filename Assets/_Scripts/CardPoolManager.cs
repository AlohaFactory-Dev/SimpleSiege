using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class CardPoolManager
{
    public IReadOnlyReactiveDictionary<string, int> CardDict => _cardDict;
    private readonly ReactiveDictionary<string, int> _cardDict = new();
    private DeckSelectionManager _selectionManager;

    public CardPoolManager(DeckSelectionManager deckSelectionManager)
    {
        _selectionManager = deckSelectionManager;
    }

    public void SetCardPool()
    {
        foreach (var cardData in _selectionManager.SelectedCardDatas)
        {
            if (_cardDict.ContainsKey(cardData.id))
            {
                Debug.LogError($"Card pool with id '{cardData.id}' already exists!");
            }
            else
            {
                _cardDict.Add(cardData.id, cardData.amount);
            }
        }
    }

    public bool ConsumeCard(string cardId)
    {
        if (_cardDict.TryGetValue(cardId, out var currentAmount) && currentAmount > 0)
        {
            _cardDict[cardId]--;
            return true;
        }

        return false;
    }

    public void AddCard(string cardId, int amount)
    {
        if (!_cardDict.TryAdd(cardId, amount))
        {
            _cardDict[cardId] += amount;
        }
    }

    public int GetCardAmount(string cardId)
    {
        return _cardDict.TryGetValue(cardId, out var amount) ? amount : 0;
    }
}