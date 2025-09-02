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
    public IObservable<Unit> OnActionNotice => _onActionNotice;
    private ISubject<Unit> _onActionNotice = new Subject<Unit>();
    private UnitTargetSystem _targetSystem;
    private UnitRotationSystem _rotationSystem;

    public void Init(UnitController unitController, UnitAnimationSystem unitAnimationSystem, UnitTargetSystem targetSystem, UnitRotationSystem rotationSystem)
    {
        _rotationSystem = rotationSystem;
        _targetSystem = targetSystem;
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
        if (_actionCoroutine != null)
        {
            StopCoroutine(_actionCoroutine);
            _actionCoroutine = null;
        }

        _actionCoroutine = StartCoroutine(ActionRoutine(target));
    }

    public void StartSiegeAction()
    {
        if (_actionCoroutine != null)
        {
            StopCoroutine(_actionCoroutine);
            _actionCoroutine = null;
        }

        _actionCoroutine = StartCoroutine(SiegeAction());
    }

    private IEnumerator SiegeAction()
    {
        float lastYRotation = 0f;
        while (true)
        {
            ITarget target = _targetSystem.FindTarget();
            if (target != null)
            {
                Vector3 currentPos = _unitController.Rigidbody2D.position;
                float edgeDistance = _targetSystem.GetEdgeDistance(currentPos, target);

                if (edgeDistance <= _unitController.EffectAbleRange)
                {
                    Vector3 targetPos = target.Transform.position;
                    Vector3 dir = (targetPos - currentPos).normalized;
                    _rotationSystem.Rotate(dir, ref lastYRotation); // 변경
                    _unitAnimationSystem.SetOnAction(() => OnAction(target, _unitController, targetPos));
                    _unitAnimationSystem.PlayAction();
                    yield return new WaitForSeconds(_unitAnimationSystem.AcionDuration);
                    // 액션 간격 대기
                    _unitAnimationSystem.PlayIdle();
                    yield return new WaitForSeconds(_unitTable.actionInterval);
                }
                else
                {
                    yield return null;
                }
            }
            else
            {
                yield return null;
            }
        }
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
                Vector2 targetPos = target.Transform.position;
                _unitAnimationSystem.SetOnAction(() => OnAction(target, _unitController, targetPos));
                yield return new WaitForSeconds(_unitAnimationSystem.AcionDuration);
                if (target.IsUntargetable)
                {
                    _unitController.ChangeState(UnitState.Move);
                    yield break;
                }

                // 액션 간격 대기
                _unitAnimationSystem.PlayIdle();
                yield return new WaitForSeconds(_unitTable.actionInterval);
            }


            _unitController.ChangeState(UnitState.Move);
            yield break;
        }
    }

    private void OnAction(ITarget target, ICaster caster, Vector2 targetPos)
    {
        _unitAction.Execute(target, caster, _unitController.EffectValue, targetPos);
        _onActionNotice.OnNext(Unit.Default);
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

        Gizmos.DrawWireSphere(transform.position, _unitController.EffectAbleRange);
    }
#endif
}