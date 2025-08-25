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
    public StageTable CurrentStageTable { get; private set; }

    public StageManager()
    {
        CurrentStageTable = TableListContainer.Get<StageTableList>().GetStageTable(1);
        OpenPopup(StagePopupConfig.DeckSelectionViewConfig);
    }

    public void OpenPopup(StagePopupConfig config, UIOpenArgs args = null)
    {
        _coconutCanvas.Open(config.ToString(), args);
    }
}