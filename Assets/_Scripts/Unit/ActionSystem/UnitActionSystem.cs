using System;
using System.Collections;
using UnityEngine;


public abstract class UnitActionSystem : MonoBehaviour
{
    private UnitTable _unitTable;
    private Coroutine _actionCoroutine;
    private UnitController _unitController;
    private UnitAnimationSystem _unitAnimationSystem;
    protected float EffectValue;

    public void Init(UnitController unitController, UnitAnimationSystem unitAnimationSystem)
    {
        // UnitController에서 UnitTable을 가져와서 초기화
        _unitController = unitController;
        _unitAnimationSystem = unitAnimationSystem;
        _unitTable = unitController.UnitTable;
        if (_unitTable.teamType == TeamType.Enemy)
        {
            var attackPowerLevel = StageConainer.Get<StageManager>().CurrentStageTable.enemyAttackPowerLevel;
            EffectValue = Mathf.CeilToInt(_unitTable.effectValue * Mathf.Pow(1 + _unitTable.effectGrowth, Math.Max(attackPowerLevel - 1, 0)));
        }
        else
        {
            EffectValue = _unitTable.effectValue;
        }
    }

    public void StartAction(Transform target)
    {
        StopAction();
        _actionCoroutine = StartCoroutine(ActionRoutine(target));
    }

    public void StopAction()
    {
        if (_actionCoroutine != null)
        {
            StopCoroutine(_actionCoroutine);
            _actionCoroutine = null;
        }
    }

    private IEnumerator ActionRoutine(Transform target)
    {
        while (true)
        {
            if (target)
            {
                _unitAnimationSystem.SetOnAction(() => OnAction(target));
                yield return new WaitForSeconds(_unitAnimationSystem.AcionDuration);
                _unitController.OnActionEnd();
                yield break;
            }
            else
            {
                // 타겟이 없으면 즉시 액션 종료
                _unitController.OnActionEnd();
                yield break;
            }

            yield return null;
        }
    }

    public abstract void OnAction(Transform target);
}