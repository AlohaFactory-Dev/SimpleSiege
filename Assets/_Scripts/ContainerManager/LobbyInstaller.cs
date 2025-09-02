using Aloha.Coconut.UI;
using ProtoTypeUI;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

public class LobbyInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        LobbyConainer.Initialize(Container);

        var coconutCanvas = GetComponentInChildren<CoconutCanvas>();
        var lobbyUI = GetComponentInChildren<LobbyUI>();


        Container.Bind<CoconutCanvas>().FromInstance(coconutCanvas).AsSingle().NonLazy();
        Container.Bind<LobbyUI>().FromInstance(lobbyUI).AsSingle().NonLazy();
        // Container.Bind<EquipmentInventoryFilterManager>().AsSingle().NonLazy();
    }
}