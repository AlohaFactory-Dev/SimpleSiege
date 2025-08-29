using System;
using Aloha.Coconut;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PassiveCard : MonoBehaviour
{
    [Inject] private PassiveManager _passiveManager;
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    private Animator _animator;
    private Button _button;
    private PassiveTable _passiveTable;
    private Action _onClickClose;
    private Action _onBlockObject;

    public void Init(Action onClickClose, Action onBlockObject)
    {
        gameObject.SetActive(true);
        _onClickClose = onClickClose;
        _onBlockObject = onBlockObject;
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnClick);
        _animator = GetComponent<Animator>();
    }

    // 애니메이션 이밴트
    public void OnClose()
    {
        _onClickClose?.Invoke();
    }

    private void OnClick()
    {
        _passiveManager.SelectPassive(_passiveTable);
        _animator.SetTrigger("Select");
        _onBlockObject?.Invoke();
    }

    public void Refresh(PassiveTable passiveTable)
    {
        _animator.SetTrigger("Refresh");
        _passiveTable = passiveTable;
        iconImage.sprite = ImageContainer.GetImage(passiveTable.iconKey);
        nameText.text = TextTableV2.Get(passiveTable.nameKey);
        descriptionText.text = TextTableV2.Get(passiveTable.descriptionKey);
    }
}