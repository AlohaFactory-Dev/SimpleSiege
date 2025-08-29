using System.Collections.Generic;
using Aloha.Coconut.UI;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PrisonSelectionPopup : UISlice
{
    [Inject] private PrisonUnitSelectionManager _prisonUnitSelectionManager;
    [SerializeField] private GameObject blockObject;
    private PrisonCard[] _prisonCards;
    private bool _isInitialized = false;

    protected override void Open(UIOpenArgs openArgs)
    {
        GameManager.Pause();
        Init();
        blockObject.SetActive(false);
        Refresh();

        base.Open(openArgs);
    }

    private void Init()
    {
        if (_isInitialized) return;
        _isInitialized = true;
        _prisonCards = GetComponentsInChildren<PrisonCard>(true);
        for (int i = 0; i < _prisonCards.Length; i++)
        {
            _prisonCards[i].Init(OnClose, OnBlockObject);
        }
    }

    private void OnBlockObject()
    {
        blockObject.SetActive(true);
    }

    private void Refresh()
    {
        var selectedCard = _prisonUnitSelectionManager.GetPrisonUnits();
        for (int i = 0; i < _prisonCards.Length; i++)
        {
            if (!_prisonCards[i].gameObject.activeSelf)
            {
                _prisonCards[i].Refresh(selectedCard[i]);
                return;
            }
        }
    }

    protected override void OnClose()
    {
        GameManager.Resume();
        base.OnClose();
    }
}