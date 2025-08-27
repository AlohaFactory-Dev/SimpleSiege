using System.Collections.Generic;
using Aloha.Coconut.UI;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PrisonSelectionPopup : UISlice
{
    [Inject] private PrisonUnitSelectionManager _prisonUnitSelectionManager;
    [SerializeField] private PrisonCard prisonCardPrefab;
    [SerializeField] private RectTransform content;
    private List<PrisonCard> _prisonCards = new();

    protected override void Open(UIOpenArgs openArgs)
    {
        GameManager.Pause();
        if (_prisonCards.Count > 0)
        {
            foreach (var deckCard in _prisonCards)
            {
                Destroy(deckCard.gameObject);
            }

            _prisonCards.Clear();
        }

        var selectedCard = _prisonUnitSelectionManager.GetPrisonUnits();
        foreach (var card in selectedCard)
        {
            CreateDeckCard(card);
        }

        base.Open(openArgs);
    }

    private void CreateDeckCard(CardData cardData)
    {
        var prisonCard = StageConainer.Container.InstantiatePrefab(prisonCardPrefab, content).GetComponent<PrisonCard>();
        prisonCard.Init(cardData, CloseView);
        _prisonCards.Add(prisonCard);
    }

    protected override void OnClose()
    {
        GameManager.Resume();
        base.OnClose();
    }
}