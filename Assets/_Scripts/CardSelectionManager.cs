using UnityEngine;

public class CardSelectionManager
{
    public UnitCard SelectedCard { get; private set; }
    public bool IsCardSelected => SelectedCard != null;

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
}