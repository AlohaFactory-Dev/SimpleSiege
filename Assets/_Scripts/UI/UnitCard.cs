using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class UnitCard : MonoBehaviour
{
    public CardData cardData;
    public Image iconImage;
    private bool _isSelected;
    private Button _button;

    [Inject] private CardSelectionManager _cardSelectionManager;

    public void Init()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnClick);
    }

    public void SetCardData(CardData data)
    {
        cardData = data;
        iconImage.sprite = cardData.icon;
    }

    public void OnClick()
    {
        _cardSelectionManager.SelectCard(this);
    }

    public void SetSelected(bool selected)
    {
        _isSelected = selected;
        // 선택 상태에 따라 UI 변경 (예: 테두리 색상)
    }
}