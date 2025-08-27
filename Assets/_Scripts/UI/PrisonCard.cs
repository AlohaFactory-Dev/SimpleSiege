using System;
using Aloha.Coconut;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PrisonCard : MonoBehaviour
{
    [Inject] private PrisonUnitSelectionManager _prisonUnitSelectionManager;
    private CardData _cardData;
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI cardNameText;
    [SerializeField] private TextMeshProUGUI cardAmountText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Button addButton;
    private Action _offPopup;

    public void Init(CardData cardData, Action offPopup)
    {
        _offPopup = offPopup;
        _cardData = cardData;
        // 여기서 카드 데이터를 UI에 반영 (예: 이미지, 텍스트 등)
        iconImage.sprite = ImageContainer.GetImage(cardData.iconKey);
        cardNameText.text = TextTableV2.Get(cardData.nameKey);
        descriptionText.text = TextTableV2.Get(cardData.descriptionKey);
        cardAmountText.text = $"x{cardData.amount}";
        addButton.onClick.AddListener(OnPrisionSelectClicked);
    }

    private void OnPrisionSelectClicked()
    {
        _prisonUnitSelectionManager.AddToCardPool(_cardData);
        _offPopup?.Invoke();
    }
}