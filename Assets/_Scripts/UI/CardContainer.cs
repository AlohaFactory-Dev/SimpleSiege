using UnityEngine;

public class CardContainer : MonoBehaviour
{
    private UnitCard[] _unitCards;

    public void Init()
    {
        _unitCards = GetComponentsInChildren<UnitCard>();
        foreach (var card in _unitCards)
        {
            card.Init();
        }
    }
}