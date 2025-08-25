using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class UnitCard : MonoBehaviour
{
    public CardTable cardTable;
    public Image iconImage;
    private bool _isSelected;
    private Button _button;

    [Inject] private CardSelectionManager _cardSelectionManager;
    [Inject] private CardPoolManager _cardPoolManager;

    public void Init()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        _cardSelectionManager.SelectCard(this);
    }

    public void SetCardData(CardTable table)
    {
        cardTable = table;
        iconImage.sprite = ImageContainer.GetImage(table.iconKey);
    }


    public void SetSelected(bool selected)
    {
        _isSelected = selected;
        // 선택 상태에 따라 UI 변경 (예: 테두리 색상)
    }
}