using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PrisonUnitSelectionManager
{
    private readonly PrisonUnitPoolTable[] _poolTables;
    private CardData[] _cardTables;
    private PrisonUnitPoolTable[] _filteredPools;
    private readonly CardPoolManager _cardPoolManager;
    private readonly int _maxSelectCount = 3;
    private DeckSelectionManager _deckSelectionManager;

    public PrisonUnitSelectionManager(DeckSelectionManager deckSelectionManager, CardPoolManager cardPoolManager)
    {
        _poolTables = TableListContainer.Get<PrisonUnitPoolTableList>().GetAllTables().ToArray();
        _cardPoolManager = cardPoolManager;
        _deckSelectionManager = deckSelectionManager;
    }

    public List<CardData> GetPrisonUnits()
    {
        _cardTables = _deckSelectionManager.SelectedCardDatas.ToArray();
        _filteredPools = _poolTables.Where(pool => _cardTables.Any(card => card.id == pool.id)).ToArray();

        var selectedUnits = new List<CardData>();
        var candidatePools = new List<PrisonUnitPoolTable>(_filteredPools);
        while (selectedUnits.Count < _maxSelectCount && candidatePools.Count > 0)
        {
            int totalProbability = candidatePools.Sum(pool => pool.probability);
            int randomValue = Random.Range(0, totalProbability + 1);
            int cumulativeProbability = 0;

            for (int i = 0; i < candidatePools.Count; i++)
            {
                var pool = candidatePools[i];
                cumulativeProbability += pool.probability;
                if (randomValue < cumulativeProbability)
                {
                    var cardTable = _cardTables.FirstOrDefault(c => c.id == pool.id);
                    if (cardTable != null)
                    {
                        selectedUnits.Add(new CardData
                        {
                            id = cardTable.id,
                            nameKey = cardTable.nameKey,
                            descriptionKey = cardTable.descriptionKey,
                            iconKey = cardTable.iconKey,
                            amount = pool.amount
                        });
                    }

                    candidatePools.RemoveAt(i);
                    break;
                }
            }
        }

        return selectedUnits;
    }

    public void AddToCardPool(CardData cardData)
    {
        _cardPoolManager.AddCard(cardData.id, cardData.amount);
    }
}