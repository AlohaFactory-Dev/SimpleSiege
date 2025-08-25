using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class CardContainer : MonoBehaviour
{
    [Inject] private DeckSelectionManager _deckSelectionManager;
    private List<UnitCard> _unitCards = new();
    [SerializeField] UnitCard unitCardPrefab;

    public void Init()
    {
        var decks = _deckSelectionManager.SelectedCards;
        foreach (var deck in decks)
        {
            var card = Instantiate(unitCardPrefab, transform);
            card.Init(deck);
            _unitCards.Add(card);
        }
    }
}