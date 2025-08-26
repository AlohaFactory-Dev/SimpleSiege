using System;
using _Scripts;
using Aloha.Coconut.UI;
using FactorySystem;
using UnityEngine;
using Zenject;

public class StageInstaller : MonoInstaller
{
    private FactoryManager _factoryManager;
    private CoconutCanvas _coconutCanvas;
    private InputManager _inputManager;
    private CameraController _cameraController;
    private StageUI _stageUI;

    public override void InstallBindings()
    {
        StageConainer.Initialize(Container);

        // Bind the necessary components for the stage

        // 필요한 컴포넌트들을 가져옵니다.
        _coconutCanvas = GetComponentInChildren<CoconutCanvas>();
        _factoryManager = GetComponentInChildren<FactoryManager>();
        _inputManager = GetComponentInChildren<InputManager>();
        _cameraController = GetComponentInChildren<CameraController>();
        _stageUI = GetComponentInChildren<StageUI>();


        Container.Bind<CoconutCanvas>().FromInstance(_coconutCanvas).AsSingle().NonLazy();
        Container.Bind<FactoryManager>().FromInstance(_factoryManager).AsSingle().NonLazy();
        Container.Bind<InputManager>().FromInstance(_inputManager).AsSingle().NonLazy();
        Container.Bind<CameraController>().FromInstance(_cameraController).AsSingle().NonLazy();
        Container.Bind<StageUI>().FromInstance(_stageUI).AsSingle().NonLazy();


        Container.Bind<CardSelectionManager>().AsSingle().NonLazy();
        Container.Bind<UnitManager>().AsSingle().NonLazy();
        Container.Bind<SpellController>().AsSingle().NonLazy();
        Container.Bind<CardPoolManager>().AsSingle().NonLazy();
        Container.Bind<DeckSelectionManager>().AsSingle().NonLazy();
        Container.Bind<StageManager>().AsSingle().NonLazy();

        Init();
    }

    private async void Init()
    {
        try
        {
            _cameraController.Init();
            await _factoryManager.Init(Container);
            _inputManager.Init();
            var stageManager = Container.Resolve<StageManager>();
            stageManager.Init();
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }
}