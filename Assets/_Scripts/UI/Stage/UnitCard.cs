using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class UnitCard : MonoBehaviour
{
    public CardTable CardTable { get; private set; }
    [Inject] private CardSelectionManager _cardSelectionManager;
    [Inject] private CardPoolManager _cardPoolManager;
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI amountText;
    [SerializeField] private TextMeshProUGUI addAmountText;
    private bool _isSelected;
    private Button _button;
    private Animator _animator;
    private int _currentAmount;

    public void Init(CardData cardData)
    {
        _button = GetComponent<Button>();
        _animator = GetComponent<Animator>();
        _button.onClick.AddListener(OnClick);
        CardTable = TableListContainer.Get<CardTableList>().GetCardTable(cardData.id);
        iconImage.sprite = ImageContainer.GetImage(cardData.iconKey);
        addAmountText.gameObject.SetActive(false);
        _cardPoolManager.CardDict
            .ObserveEveryValueChanged(dict => dict.ContainsKey(CardTable.id) ? dict[CardTable.id] : 0)
            .Subscribe(amount =>
            {
                if (_currentAmount > amount)
                {
                    Consume(amount);
                }
                else
                {
                    AddAmount(amount);
                }
            })
            .AddTo(this);
        SetSelected(false);
        Refresh(_cardPoolManager.GetCardAmount(CardTable.id));
    }

    private void OnClick()
    {
        _cardSelectionManager.SelectCard(this);
    }

    private void Refresh(int amount)
    {
        _currentAmount = amount;
        amountText.text = $"x{amount}";
    }

    private void AddAmount(int amount)
    {
        int addedAmount = amount - _currentAmount;
        addAmountText.text = $"+{addedAmount}";
        _currentAmount = amount;
        amountText.text = $"x{_currentAmount}";
        _animator.SetTrigger("Add");
        _button.interactable = true;
    }


    private void Consume(int amount)
    {
        _currentAmount = amount;
        amountText.text = $"x{amount}";
    }

    public void DisableCard()
    {
        _button.interactable = false;
        _animator.SetTrigger("Disable");
    }

    public void SetSelected(bool selected)
    {
        if (_isSelected == selected) return;
        _isSelected = selected;
        if (selected)
            _animator.SetTrigger("Select");
        else
            _animator.SetTrigger("Deselected");
    }
}