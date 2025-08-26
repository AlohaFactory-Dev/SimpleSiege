using Aloha.Coconut;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class DeckCard : MonoBehaviour
{
    [Inject] private DeckSelectionManager _deckSelectionManager;

    private CardTable _cardTable;
    private Animator _animator;
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI cardNameText;
    [SerializeField] private TextMeshProUGUI cardAmountText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Button addButton;

    public void Init(CardTable cardTable)
    {
        _cardTable = cardTable;
        // 여기서 카드 데이터를 UI에 반영 (예: 이미지, 텍스트 등)
        iconImage.sprite = ImageContainer.GetImage(cardTable.iconKey);
        cardNameText.text = TextTableV2.Get(cardTable.nameKey);
        descriptionText.text = TextTableV2.Get(cardTable.descriptionKey);
        cardAmountText.text = $"x{cardTable.cardAmount}";
        addButton.onClick.AddListener(OnAddButtonClicked);
        _animator = GetComponent<Animator>();
    }

    private void OnAddButtonClicked()
    {
        if (_deckSelectionManager.IsCardSelected(_cardTable))
        {
            _deckSelectionManager.UnselectCard(_cardTable);
            _animator.SetBool("IsSelected", false);
        }
        else
        {
            if (!_deckSelectionManager.SelectCard(_cardTable))
            {
                SystemUI.ShowToastMessage("DeckSelectionPopup/Max");
            }
            else
            {
                _animator.SetBool("IsSelected", true);
            }
        }
    }
}