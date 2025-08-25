using System.Collections.Generic;
using Aloha.Coconut.UI;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class DeckSelectionPopup : UISlice
{
    [Inject] private DeckSelectionManager _deckSelectionManager;
    [SerializeField] private DeckCard deckCardPrefab;
    [SerializeField] private RectTransform content;
    [SerializeField] private TextMeshProUGUI deckSizeText;
    [SerializeField] private Button startGameButton;

    protected override void Open(UIOpenArgs openArgs)
    {
        var allCards = StageConainer.Get<DeckSelectionManager>().AllCards;
        foreach (var card in allCards)
        {
            CreateDeckCard(card);
        }

        base.Open(openArgs);
        _deckSelectionManager.SelectedCards.ObserveCountChanged().Subscribe(_ => UpdateDeckSizeText()).AddTo(this);
        UpdateDeckSizeText();
        startGameButton.onClick.AddListener(() =>
        {
            StageConainer.Get<StageManager>().StartStage();
            CloseView();
        });
    }

    private void UpdateDeckSizeText()
    {
        deckSizeText.text = $"{_deckSelectionManager.SelectedCards.Count} / {_deckSelectionManager.MaxDeckSize}";
        startGameButton.interactable = _deckSelectionManager.SelectedCards.Count == _deckSelectionManager.MaxDeckSize;
    }

    private void CreateDeckCard(CardTable cardTable)
    {
        var deckCard = Instantiate(deckCardPrefab, content);
        deckCard.Init(cardTable);
    }
}