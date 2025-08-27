using Zenject;

public class PrisonBuilding : Building
{
    [Inject] private StageManager _stageManager;

    protected override void DestroyBuilding()
    {
        base.DestroyBuilding();
        _stageManager.OpenPopup(StagePopupConfig.PrisonSelectionPopup);
    }
}