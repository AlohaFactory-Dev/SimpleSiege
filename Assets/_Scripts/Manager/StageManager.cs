using System;
using System.Threading.Tasks;
using Aloha.Coconut;
using Aloha.Coconut.UI;
using FactorySystem;
using UnityEngine;
using Zenject;

public enum StagePopupConfig
{
    DeckSelectionViewConfig,
}

public class StageManager
{
    [Inject] private CoconutCanvas _coconutCanvas;
    [Inject] private DeckSelectionManager _deckSelectionManager;
    [Inject] private CardPoolManager _cardPoolManager;
    public StageTable CurrentStageTable { get; private set; }

    public StageManager()
    {
        CurrentStageTable = TableListContainer.Get<StageTableList>().GetStageTable(1);
        OpenPopup(StagePopupConfig.DeckSelectionViewConfig);
    }

    public void OpenPopup(StagePopupConfig config, UIOpenArgs args = null)
    {
        Time.timeScale = 0f;
        _coconutCanvas.Open(config.ToString(), args);
    }

    public void StartStage()
    {
        // 스테이지 시작 로직 구현
        Debug.Log($"Starting Stage: {CurrentStageTable.stageNameKey}");
        Time.timeScale = 1f;
        foreach (var card in _deckSelectionManager.SelectedCards)
        {
            _cardPoolManager.SetCardPool(card);
        }
    }
}