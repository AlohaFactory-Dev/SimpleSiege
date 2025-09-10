using System;
using System.Collections;
using System.Threading.Tasks;
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
    [Inject] private UnitManager _unitManager;
    public StageTable CurrentStageTable { get; private set; }

    public CameraController CameraController { get; private set; }
    [SerializeField] private float stageEndDelay = 0.5f;
    private float startTime;
    public float EndTime => Time.time - startTime;
    public bool isInit { get; private set; }

    public void OpenPopup(StagePopupConfig config, UIOpenArgs args = null)
    {
        Time.timeScale = 0f;
        _coconutCanvas.Open(config.ToString(), args);
    }

    public void StartStage()
    {
        // 스테이지 시작 로직 구현
        Time.timeScale = 1f;
        startTime = Time.time;
        _cardPoolManager.SetCardPool();
        _stageUI.Init();
    }

    private void EndStage(TeamType team)
    {
        StartCoroutine(EndCoroutine(team));
    }

    private IEnumerator EndCoroutine(TeamType team)
    {
        yield return new WaitForSeconds(stageEndDelay);
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

        var map = StageConainer.Container.InstantiatePrefab(mapGO, transform);
        // BuildingManager를 직접 할당
        var buildingManager = map.GetComponentInChildren<BuildingManager>();
        CameraController = map.GetComponentInChildren<CameraController>();
        OpenPopup(StagePopupConfig.DeckSelectionViewConfig);
        CameraController.Init();
        _inputManager.Init();
        buildingManager.Init();
        buildingManager.OnStageResult.Subscribe(EndStage).AddTo(this);
        _passiveManager.Init();
        isInit = true;
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