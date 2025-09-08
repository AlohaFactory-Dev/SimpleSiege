using Aloha.Coconut;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class DeckCard : MonoBehaviour
{
    [Inject] private DeckSelectionManager _deckSelectionManager;

    private CardData _cardData;
    private Animator _animator;
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI cardNameText;
    [SerializeField] private TextMeshProUGUI cardAmountText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Button addButton;

    public void Init(CardData cardData)
    {
        _cardData = cardData;
        // 여기서 카드 데이터를 UI에 반영 (예: 이미지, 텍스트 등)
        iconImage.sprite = ImageContainer.GetImage(cardData.iconKey);
        cardNameText.text = TextTableV2.Get(cardData.nameKey);
        descriptionText.text = TextTableV2.Get(cardData.descriptionKey);
        cardAmountText.text = $"x{cardData.amount}";
        addButton.onClick.AddListener(OnDeckSelectClicked);
        _animator = GetComponent<Animator>();
    }

    private void OnDeckSelectClicked()
    {
        if (_deckSelectionManager.IsCardSelected(_cardData))
        {
            _deckSelectionManager.UnselectCard(_cardData);
            _animator.SetBool("IsSelected", false);
        }
        else
        {
            if (!_deckSelectionManager.SelectCard(_cardData))
            {
                SystemUI.ShowToastMessage(TextTableV2.Get("DeckSelectionPopup/Max"));
            }
            else
            {
                _animator.SetBool("IsSelected", true);
            }
        }
    }
}