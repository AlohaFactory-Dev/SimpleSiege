using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;


public class UnitActionSystem : MonoBehaviour
{
    private Coroutine _actionCoroutine;
    private UnitController _unitController;
    private UnitAnimationSystem _unitAnimationSystem;
    private UnitTable _unitTable;
    private IUnitAction _unitAction;
    private int EffectValue => _unitController.EffectValue + _totalAddedEffectValue;
    private int _totalAddedEffectValue;
    private Dictionary<string, int> _effectValueModifiers = new Dictionary<string, int>();
    public IObservable<Unit> OnActionNotice => _onActionNotice;
    private ISubject<Unit> _onActionNotice = new Subject<Unit>();

    public void Init(UnitController unitController, UnitAnimationSystem unitAnimationSystem)
    {
        // UnitController에서 UnitTable을 가져와서 초기화
        _unitController = unitController;
        _unitAnimationSystem = unitAnimationSystem;
        _unitTable = unitController.UnitTable;
        _totalAddedEffectValue = 0;
        _effectValueModifiers.Clear();
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

    public void StartAction(ITarget target, bool isSiege)
    {
        if (_actionCoroutine != null)
        {
            StopCoroutine(_actionCoroutine);
            _actionCoroutine = null;
        }

        _actionCoroutine = StartCoroutine(ActionRoutine(target, isSiege));
    }

    public void StopAction()
    {
        if (_actionCoroutine != null)
        {
            StopCoroutine(_actionCoroutine);
            _actionCoroutine = null;
        }
    }

    private IEnumerator ActionRoutine(ITarget target, bool isSiege)
    {
        while (true)
        {
            if (target != null)
            {
                _unitAnimationSystem.SetOnAction(() => OnAction(target, _unitController));
                yield return new WaitForSeconds(_unitAnimationSystem.AcionDuration);
                if (target.IsUntargetable)
                {
                    if (isSiege)
                    {
                        _unitController.ChangeState(UnitState.Siege);
                    }
                    else
                    {
                        _unitController.ChangeState(UnitState.Move);
                    }

                    yield break;
                }

                // 액션 간격 대기
                _unitAnimationSystem.PlayIdle();
                yield return new WaitForSeconds(_unitTable.actionInterval);
            }

            // 타겟이 없으면 즉시 액션 종료
            if (isSiege)
            {
                _unitController.ChangeState(UnitState.Siege);
            }
            else
            {
                _unitController.ChangeState(UnitState.Move);
            }

            yield break;
        }
    }

    private void OnAction(ITarget target, ICaster caster)
    {
        _unitAction.Execute(target, caster, EffectValue);
        _onActionNotice.OnNext(Unit.Default);
    }

    public void SetAddedEffectValue(string id, int value)
    {
        if (!_effectValueModifiers.TryAdd(id, value))
        {
            _effectValueModifiers[id] += value;
        }

        _totalAddedEffectValue = 0;
        foreach (var mod in _effectValueModifiers.Values)
        {
            _totalAddedEffectValue += mod;
        }
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (_unitTable == null) return;
        if (_unitTable.teamType == TeamType.Player)
        {
            Gizmos.color = Color.blue;
        }
        else
        {
            Gizmos.color = Color.red;
        }

        Gizmos.DrawWireSphere(transform.position, _unitTable.effectAbleRange);
    }
#endif
}