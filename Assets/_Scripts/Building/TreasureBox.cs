using Zenject;

public class TreasureBox : Building
{
    [Inject] private StageManager _stageManager;

    protected override void DestroyBuilding()
    {
        base.DestroyBuilding();
        _stageManager.OpenPopup(StagePopupConfig.PassiveSelectionViewConfig);
    }
}