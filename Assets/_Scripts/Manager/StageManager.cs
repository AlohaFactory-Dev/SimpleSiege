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
    PrisonSelectionViewConfig,
    StageResultViewConfig
}

public class StageManager
{
    [Inject] private CoconutCanvas _coconutCanvas;
    [Inject] private DeckSelectionManager _deckSelectionManager;
    [Inject] private CardPoolManager _cardPoolManager;
    [Inject] private StageUI _stageUI;
    [Inject] private BuildingSpawnController _buildingSpawnController;
    [Inject] private UnitManager _unitManager;
    public StageTable CurrentStageTable { get; private set; }

    public StageManager()
    {
        CurrentStageTable = TableListContainer.Get<StageTableList>().GetStageTable(1);
    }

    public void Init()
    {
        OpenPopup(StagePopupConfig.DeckSelectionViewConfig);
        _buildingSpawnController.Init();
        _unitManager.Init();
    }

    public void OpenPopup(StagePopupConfig config, UIOpenArgs args = null)
    {
        Time.timeScale = 0f;
        _coconutCanvas.Open(config.ToString(), args);
    }

    public void StartStage()
    {
        // 스테이지 시작 로직 구현
        Time.timeScale = 1f;
        _cardPoolManager.SetCardPool();
        _stageUI.Init();
    }

    public void EndStage(bool isWin)
    {
        Time.timeScale = 0f;
        var args = new StageResultPopup.Args()
        {
            IsWin = isWin
        };
        _coconutCanvas.Open(nameof(StagePopupConfig.StageResultViewConfig), args);
    }
}