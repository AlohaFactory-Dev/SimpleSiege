using System;
using System.Collections;
using UnityEngine;


public class UnitActionSystem : MonoBehaviour
{
    private Coroutine _actionCoroutine;
    private UnitController _unitController;
    private UnitAnimationSystem _unitAnimationSystem;
    private UnitTable _unitTable;
    private IUnitAction _unitAction;

    public void Init(UnitController unitController, UnitAnimationSystem unitAnimationSystem)
    {
        // UnitController에서 UnitTable을 가져와서 초기화
        _unitController = unitController;
        _unitAnimationSystem = unitAnimationSystem;
        _unitTable = unitController.UnitTable;


        InitAction();
    }

    private void InitAction()
    {
        if (_unitTable.effectType == EffectType.Melee)
        {
            _unitAction = new MeleeAttackAction();
        }
        else if (_unitTable.effectType == EffectType.Ranged)
        {
            _unitAction = new RangedAttackAction();
        }
    }

    public void StartAction(ITarget target)
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

    private IEnumerator ActionRoutine(ITarget target)
    {
        while (true)
        {
            if (target != null)
            {
                _unitAnimationSystem.SetOnAction(() => OnAction(target, _unitController));
                yield return new WaitForSeconds(_unitAnimationSystem.AcionDuration);
                _unitController.OnActionEnd();
                yield break;
            }

            // 타겟이 없으면 즉시 액션 종료
            _unitController.OnActionEnd();
            yield break;
        }
    }

    protected virtual void OnAction(ITarget target, ICaster caster)
    {
        _unitAction.Execute(target, caster);
    }
}