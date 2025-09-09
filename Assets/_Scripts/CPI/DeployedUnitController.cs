using System.Collections;
using UnityEngine;
using Zenject;

public class DeployedUnitController : MonoBehaviour
{
    private UnitController[] _unitControllers;
    [Inject] private StageManager _stageManager;
    [Inject] private UnitManager _unitManager;

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => _stageManager.isInit);
        _unitControllers = GetComponentsInChildren<UnitController>();
        _unitManager.SetDeployedUnit(_unitControllers);
    }

    public void StartBattle()
    {
        foreach (var unit in _unitControllers)
        {
            unit.Collider2D.enabled = true;
            unit.ChangeState(UnitState.Move);
        }
    }
}