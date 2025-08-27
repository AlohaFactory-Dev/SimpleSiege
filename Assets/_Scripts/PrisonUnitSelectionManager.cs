using System.Collections.Generic;
using System.Linq;

public class PrisonUnitSelectionManager
{
    private PrisonUnitPoolTable[] _poolTables;
    private CardData[] _cardTables;
    private int _maxSelectCount = 3;
    private CardPoolManager _cardPoolManager;

    public PrisonUnitSelectionManager(DeckSelectionManager deckSelectionManager, CardPoolManager cardPoolManager)
    {
        _poolTables = TableListContainer.Get<PrisonUnitPoolTableList>().GetAllTables().ToArray();
        _cardTables = deckSelectionManager.SelectedCardDatas.ToArray();
        _cardPoolManager = cardPoolManager;
    }

    public List<CardData> GetPrisonUnits()
    {
        List<CardData> selectedUnits = new List<CardData>();
        int totalProbability = 0;
        foreach (var pool in _poolTables)
        {
            totalProbability += pool.probability;
        }

        HashSet<string> selectedIds = new HashSet<string>();
        System.Random random = new System.Random();

        while (selectedUnits.Count < _maxSelectCount)
        {
            int randomValue = random.Next(0, totalProbability);
            int cumulativeProbability = 0;

            foreach (var pool in _poolTables)
            {
                cumulativeProbability += pool.probability;
                if (randomValue < cumulativeProbability)
                {
                    if (!selectedIds.Contains(pool.id))
                    {
                        var cardTable = System.Array.Find(_cardTables, c => c.id == pool.id);
                        if (cardTable != null)
                        {
                            selectedUnits.Add(new CardData
                            {
                                nameKey = cardTable.nameKey,
                                descriptionKey = cardTable.descriptionKey,
                                iconKey = cardTable.iconKey,
                                amount = pool.amount
                            });
                            selectedIds.Add(pool.id);
                        }
                    }

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