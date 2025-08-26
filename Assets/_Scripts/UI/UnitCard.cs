using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class UnitCard : MonoBehaviour
{
    public CardTable CardTable { get; private set; }
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI amountText;
    private bool _isSelected;
    private Button _button;
    private Animator _animator;

    [Inject] private CardSelectionManager _cardSelectionManager;
    [Inject] private CardPoolManager _cardPoolManager;

    public void Init(CardTable table)
    {
        _button = GetComponent<Button>();
        _animator = GetComponent<Animator>();
        _button.onClick.AddListener(OnClick);
        CardTable = table;
        iconImage.sprite = ImageContainer.GetImage(table.iconKey);
        _cardPoolManager.CardDict
            .ObserveEveryValueChanged(dict => dict.ContainsKey(CardTable.id) ? dict[CardTable.id] : 0)
            .Subscribe(Refresh)
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
        amountText.text = $"x{amount}";
        if (CardTable.cardAmount == amount) return;
        _animator.SetTrigger("Action");
    }

    public void DisableCard()
    {
        _button.interactable = false;
        _animator.SetTrigger("Disable");
    }


    public void SetSelected(bool selected)
    {
        _animator.SetBool("IsSelected", selected);
    }
}