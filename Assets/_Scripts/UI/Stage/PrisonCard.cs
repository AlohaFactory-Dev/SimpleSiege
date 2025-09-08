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
    private Action _onBlockObject;
    private Animator _animator;

    public void Init(Action offPopup, Action onBlockObject)
    {
        _offPopup = offPopup;
        _onBlockObject = onBlockObject;
        addButton.onClick.AddListener(OnPrisionSelectClicked);
        _animator = GetComponent<Animator>();
    }

    public void Refresh(CardData cardData)
    {
        _animator.SetTrigger("Refresh");
        gameObject.SetActive(true);
        _cardData = cardData;
        // 여기서 카드 데이터를 UI에 반영 (예: 이미지, 텍스트 등)
        iconImage.sprite = ImageContainer.GetImage(cardData.iconKey);
        cardNameText.text = TextTableV2.Get(cardData.nameKey);
        descriptionText.text = TextTableV2.Get(cardData.descriptionKey);
        cardAmountText.text = $"x{cardData.amount}";
    }


    private void OnPrisionSelectClicked()
    {
        _animator.SetTrigger("Select");
        _onBlockObject?.Invoke();
        _prisonUnitSelectionManager.AddToCardPool(_cardData);
    }

    public void OnClose()
    {
        _offPopup?.Invoke();
    }
}