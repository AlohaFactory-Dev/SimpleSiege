using System.Collections.Generic;
using System.Linq;
using UniRx;

public class DeckSelectionManager
{
    private List<CardTable> _allCards;
    public List<CardTable> AllCards => _allCards;

    public ReactiveCollection<CardTable> SelectedCards { get; private set; } = new();
    private int maxDeckSize;
    public int MaxDeckSize => maxDeckSize;

    public DeckSelectionManager()
    {
        _allCards = TableListContainer.Get<CardTableList>().GetAllCardTable().ToList();
        maxDeckSize = (int)TableListContainer.Get<EtcTableList>().GetEtcTable("initial").values[0];
    }

    public bool SelectCard(CardTable cardTable)
    {
        // 카드 선택 로직 구현
        if (SelectedCards.Count < maxDeckSize && !SelectedCards.Contains(cardTable))
        {
            SelectedCards.Add(cardTable);
            return true;
        }

        return false;
    }

    public void UnselectCard(CardTable cardTable)
    {
        // 카드 선택 해제 로직 구현
        if (SelectedCards.Contains(cardTable))
        {
            SelectedCards.Remove(cardTable);
        }
    }

    public bool IsCardSelected(CardTable cardTable)
    {
        return SelectedCards.Contains(cardTable);
    }
}