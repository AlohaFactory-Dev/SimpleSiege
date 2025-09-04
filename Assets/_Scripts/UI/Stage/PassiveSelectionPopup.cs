using Aloha.Coconut.UI;
using UnityEngine;
using Zenject;

public class PassiveSelectionPopup : UISlice
{
    [SerializeField] private GameObject blockObject;
    private bool _isInitialized;
    [Inject] private PassiveManager _passiveManager;
    private PassiveCard[] _passiveCards;

    protected override void Open(UIOpenArgs openArgs)
    {
        StageConainer.Get<StageUI>().CardContainer.OnFirst();
        GameManager.Pause();
        Init();
        Refresh();
        base.Open(openArgs);
    }

    private void Init()
    {
        if (_isInitialized) return;
        _isInitialized = true;
        _passiveCards = GetComponentsInChildren<PassiveCard>(true);
        for (int i = 0; i < _passiveCards.Length; i++)
        {
            _passiveCards[i].Init(CloseView, OnBlockObject);
        }
    }

    private void OnBlockObject()
    {
        blockObject.SetActive(true);
    }

    private void Refresh()
    {
        blockObject.SetActive(false);
        var passives = _passiveManager.PickRandomPassives();
        for (int i = 0; i < _passiveCards.Length; i++)
        {
            _passiveCards[i].Refresh(passives[i]);
        }
    }

    protected override void OnClose()
    {
        StageConainer.Get<StageUI>().CardContainer.OnLast();
        GameManager.Resume();
        base.OnClose();
    }
}