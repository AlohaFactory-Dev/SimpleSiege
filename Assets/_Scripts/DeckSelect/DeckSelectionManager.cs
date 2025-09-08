using System.Collections.Generic;
using System.Linq;
using UniRx;

public class DeckSelectionManager
{
    private List<CardTable> _allCards;
    public List<CardTable> AllCards => _allCards;

    public ReactiveCollection<CardData> SelectedCardDatas { get; private set; } = new();
    private readonly int _maxDeckSize;
    public int MaxDeckSize => _maxDeckSize;

    public DeckSelectionManager()
    {
        _allCards = TableListContainer.Get<CardTableList>().GetAllCardTable().ToList();
        _maxDeckSize = (int)TableListContainer.Get<EtcTableList>().GetEtcTable("initial").values[0];
    }

    public bool SelectCard(CardData cardData)
    {
        // 카드 선택 로직 구현
        if (SelectedCardDatas.Count < _maxDeckSize && !SelectedCardDatas.Contains(cardData))
        {
            SelectedCardDatas.Add(cardData);
            return true;
        }

        return false;
    }

    public void UnselectCard(CardData cardData)
    {
        // 카드 선택 해제 로직 구현
        if (SelectedCardDatas.Contains(cardData))
        {
            SelectedCardDatas.Remove(cardData);
        }
    }

    public bool IsCardSelected(CardData cardData)
    {
        return SelectedCardDatas.Contains(cardData);
    }
}