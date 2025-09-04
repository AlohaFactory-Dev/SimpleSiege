using System;
using System.Threading.Tasks;
using Aloha.Coconut;
using Aloha.Coconut.Launcher;
using Aloha.Coconut.UI;
using Aloha.CoconutMilk;
using CoconutMilk.Equipments;
using Cysharp.Threading.Tasks;
using ProtoTypeUI;
using UnityEngine;
using Zenject;

public class GameManager
{
    private GameSceneManager _gameSceneManager;
    private CoconutCanvas _coconutCanvas;
    private DiContainer _container;

    public GameManager(GameSceneManager gameSceneManager, CoconutCanvas coconutCanvas, DiContainer container)
    {
        _gameSceneManager = gameSceneManager;
        _coconutCanvas = coconutCanvas;
        _container = container;
    }

    //  TableListContainer가 완료되고 Bind해야 될 객체을 해줌
    public async UniTask Init()
    {
        await TableListContainer.InitAllTables();
        await ImageContainer.InitializeAsync();
        _container.Bind<PropertyIconPool>().AsSingle().NonLazy();

        await _gameSceneManager.LoadSceneAsync("Lobby");
    }


    public void ReLoadLobby()
    {
        UnloadStage();
        LoadLobby();
    }

    private async UniTask LoadLobby()
    {
        LobbyConainer.Get<LobbyUI>().ActiveUI(true);
    }

    public void LoadStage(bool isFirstLogin = false)
    {
        LobbyConainer.Get<LobbyUI>().ActiveUI(false);
        _coconutCanvas.Open(SceneTransitionViewer.ConfigName, new SceneTransitionViewer.SceneTransitionOpenArgs(Load, Pause));

        async UniTask Load()
        {
            Pause();
            await _gameSceneManager.LoadSceneAsync("Stage");
        }
    }

    public void ReLoadStage()
    {
        _coconutCanvas.Open(SceneTransitionViewer.ConfigName, new SceneTransitionViewer.SceneTransitionOpenArgs(ReLoad, Pause));

        async UniTask ReLoad()
        {
            Pause();
            await _gameSceneManager.ReloadSceneAsync("Stage");
            LobbyConainer.Get<LobbyUI>().ActiveUI(false);
        }
    }

    public void UnloadStage()
    {
        _coconutCanvas.Open(SceneTransitionViewer.ConfigName, new SceneTransitionViewer.SceneTransitionOpenArgs(Unload, Resume));


        async UniTask Unload()
        {
            Pause();
            await _gameSceneManager.UnloadSceneAsync("Stage");
            // LobbyConainer.Get<LobbyUI>().Show();
        }
    }

    public static void Pause()
    {
        Time.timeScale = 0;
    }

    public static void Resume()
    {
        Time.timeScale = 1;
    }
}