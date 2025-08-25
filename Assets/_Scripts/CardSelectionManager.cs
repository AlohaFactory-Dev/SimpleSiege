using Aloha.Coconut;
using UnityEngine;
using Zenject;

public class CardSelectionManager
{
    public UnitCard SelectedCard { get; private set; }
    public bool IsCardSelected => SelectedCard != null;
    [Inject] private UnitManager _unitManager;
    [Inject] private SpellController _spellController;
    [Inject] private CardPoolManager _cardPoolManager;

    public void SelectCard(UnitCard card)
    {
        if (SelectedCard == card)
        {
            // 이미 선택된 카드를 다시 선택하면 해제
            card.SetSelected(false);
            SelectedCard = null;
        }
        else
        {
            if (SelectedCard != null)
                SelectedCard.SetSelected(false);
            SelectedCard = card;
            card.SetSelected(true);
        }
    }

    public void UseSelectedCard(Vector2 position)
    {
        // 카드 사용 로직 구현
        if (!IsCardSelected) return;
        // 예: 유닛 생성, 마나 소모 등
        if (!_cardPoolManager.ConsumeCard(SelectedCard.cardTable.id))
        {
            SystemUI.ShowNotEnoughToastMessage("MP");
            return;
        }

        if (SelectedCard.cardTable.cardType == CardType.Unit)
        {
            _unitManager.SpawnUnit(position, SelectedCard.cardTable.id, SelectedCard.cardTable.unitAmount, TeamType.Player);
        }
        else if (SelectedCard.cardTable.cardType == CardType.Spell)
        {
            _spellController.CastSpell(position, SelectedCard.cardTable.id);
        }
    }

    public void OffSelectedCard()
    {
        // 카드 사용 로직 구현
        if (!IsCardSelected) return;
        SelectedCard.SetSelected(false);
        SelectedCard = null;
    }
}