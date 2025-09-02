using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedStageManager
{
    private StageTable _currentStage;
    public StageTable CurrentStage => _currentStage;

    public void SetStage(int stageNumber)
    {
        _currentStage = TableListContainer.Get<StageTableList>().GetStageTable(stageNumber);
    }
}