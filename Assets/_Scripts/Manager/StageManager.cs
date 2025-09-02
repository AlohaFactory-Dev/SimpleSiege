using System;
using System.Threading.Tasks;
using _Scripts.Map;
using Aloha.Coconut;
using Aloha.Coconut.UI;
using FactorySystem;
using UniRx;
using UnityEngine;
using Zenject;

public enum StagePopupConfig
{
    DeckSelectionViewConfig,
    PrisonSelectionViewConfig,
    StageResultViewConfig,
    PassiveSelectionViewConfig
}

public class StageManager : MonoBehaviour
{
    [Inject] private CoconutCanvas _coconutCanvas;
    [Inject] private DeckSelectionManager _deckSelectionManager;
    [Inject] private CardPoolManager _cardPoolManager;
    [Inject] private StageUI _stageUI;
    [Inject] private BuildingManager _buildingManager;
    [Inject] private PassiveManager _passiveManager;
    [Inject] private InputManager _inputManager;
    [Inject] private CameraController _cameraController;
    [Inject] private UnitManager _unitManager;
    public StageTable CurrentStageTable { get; private set; }

    public void Init()
    {
        CurrentStageTable = TableListContainer.Get<StageTableList>().GetStageTable(1);
        GetComponentInChildren<SpawnableZoneController>().Init();
        OpenPopup(StagePopupConfig.DeckSelectionViewConfig);
        _cameraController.Init();
        _inputManager.Init();
        _buildingManager.Init();
        _buildingManager.OnStageResult.Subscribe(EndStage).AddTo(this);
        _passiveManager.Init();
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

    private void EndStage(TeamType team)
    {
        Time.timeScale = 0f;
        var args = new StageResultPopup.Args()
        {
            winner = team
        };
        _coconutCanvas.Open(nameof(StagePopupConfig.StageResultViewConfig), args);
    }
}