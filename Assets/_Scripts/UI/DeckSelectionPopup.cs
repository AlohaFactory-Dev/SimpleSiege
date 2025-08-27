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
            var cardData = new CardData()
            {
                id = card.id,
                nameKey = card.nameKey,
                descriptionKey = card.descriptionKey,
                iconKey = card.iconKey,
                amount = card.cardAmount // 기본적으로 각 카드의 수량을 1로 설정
            };

            CreateDeckCard(cardData);
        }

        base.Open(openArgs);
        _deckSelectionManager.SelectedCardDatas.ObserveCountChanged().Subscribe(_ => UpdateDeckSizeText()).AddTo(this);
        UpdateDeckSizeText();
        startGameButton.onClick.AddListener(() =>
        {
            StageConainer.Get<StageManager>().StartStage();
            CloseView();
        });
    }

    private void UpdateDeckSizeText()
    {
        deckSizeText.text = $"{_deckSelectionManager.SelectedCardDatas.Count} / {_deckSelectionManager.MaxDeckSize}";
        startGameButton.interactable = _deckSelectionManager.SelectedCardDatas.Count == _deckSelectionManager.MaxDeckSize;
    }

    private void CreateDeckCard(CardData cardData)
    {
        var deckCard = StageConainer.Container.InstantiatePrefab(deckCardPrefab, content).GetComponent<DeckCard>();
        deckCard.Init(cardData);
    }
}