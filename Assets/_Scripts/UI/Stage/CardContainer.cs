using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class CardContainer : MonoBehaviour
{
    [Inject] private DeckSelectionManager _deckSelectionManager;
    private List<UnitCard> _unitCards = new();
    [SerializeField] private UnitCard unitCardPrefab;
    [SerializeField] private GameObject blackObject;
    [SerializeField] private Canvas canvas;
    [SerializeField] private CanvasGroup canvasGroup;

    public void Init()
    {
        blackObject.SetActive(false);
        var decks = _deckSelectionManager.SelectedCardDatas;

        foreach (var deck in decks)
        {
            var card = StageConainer.Container.InstantiatePrefab(unitCardPrefab, transform).GetComponent<UnitCard>();
            card.Init(deck);
            _unitCards.Add(card);
        }
    }

    public void OnFirst()
    {
        blackObject.SetActive(true);
        canvas.overrideSorting = true;
        canvas.sortingOrder = 9999;
        canvasGroup.alpha = 0.3f;
    }

    public void OnLast()
    {
        blackObject.SetActive(false);
        canvas.overrideSorting = false;
        canvasGroup.alpha = 1f;
    }
}