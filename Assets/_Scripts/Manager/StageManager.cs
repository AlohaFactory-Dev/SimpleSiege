using System;
using System.Threading.Tasks;
using _Scripts.Map;
using Aloha.Coconut;
using Aloha.Coconut.UI;
using FactorySystem;
using UniRx;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
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
    [Inject] private PassiveManager _passiveManager;
    [Inject] private InputManager _inputManager;
    [Inject] private CameraController _cameraController;
    [Inject] private UnitManager _unitManager;
    public StageTable CurrentStageTable { get; private set; }

    private BuildingManager _buildingManager; // 직접 관리

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


    public async Task Init()
    {
        CurrentStageTable = GlobalConainer.Get<SelectedStageManager>().CurrentStage;
        var mapGO = await LoadMapByIdAsync(CurrentStageTable.mapId);

        StageConainer.Container.InstantiatePrefab(mapGO, transform);

        // BuildingManager를 직접 할당
        _buildingManager = GetComponentInChildren<BuildingManager>();
        if (_buildingManager == null)
        {
            Debug.LogError("BuildingManager를 찾을 수 없습니다.");
            return;
        }

        GetComponentInChildren<SpawnableZoneController>().Init();
        OpenPopup(StagePopupConfig.DeckSelectionViewConfig);
        _cameraController.Init();
        _inputManager.Init();
        _buildingManager.Init();
        _buildingManager.OnStageResult.Subscribe(EndStage).AddTo(this);
        _passiveManager.Init();
    }

    private async Task<GameObject> LoadMapByIdAsync(string mapId)
    {
        var handle = Addressables.LoadAssetAsync<GameObject>(mapId);
        await handle.Task;
        if (handle.Status == AsyncOperationStatus.Succeeded)
            return handle.Result;
        else
            Debug.LogError($"Map({mapId}) Addressable Load 실패");
        return null;
    }
}