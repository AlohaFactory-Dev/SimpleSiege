using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMoveSystem : MonoBehaviour
{
    private UnitTable _unitTable;
    private Coroutine _moveCoroutine;
    private UnitController _unitController;
    private ITarget _target;
    private UnitTargetSystem _targetSystem;
    private UnitRotationSystem _rotationSystem; // 추가

    public void Init(UnitController unit, UnitTargetSystem targetSystem, UnitRotationSystem rotationSystem) // 파라미터 추가
    {
        _unitController = unit;
        _unitTable = unit.UnitTable;
        _targetSystem = targetSystem;
        _rotationSystem = rotationSystem; // 초기화
    }

    public void StartMove()
    {
        StopMove();
        _moveCoroutine = StartCoroutine(MoveRoutine());
    }

    public void StopMove()
    {
        if (_moveCoroutine != null)
        {
            StopCoroutine(_moveCoroutine);
            _moveCoroutine = null;
        }
    }


    private IEnumerator MoveRoutine()
    {
        float lastYRotation = 0f;
        while (true)
        {
            Vector3 nextPosition = _unitController.Rigidbody2D.position;
            _target = _targetSystem.FindTarget();

            if (_target != null)
            {
                Vector3 targetPos = _target.Transform.position;
                Vector3 currentPos = _unitController.Rigidbody2D.position;
                Vector3 dir = (targetPos - currentPos).normalized;

                float edgeDistance = _targetSystem.GetEdgeDistance(currentPos, _target);

                if (edgeDistance > _unitController.EffectAbleRange)
                {
                    nextPosition += dir * (_unitController.MoveSpeed * Time.deltaTime);
                    _rotationSystem.Rotate(dir, ref lastYRotation); // 변경
                }
                else
                {
                    _rotationSystem.Rotate(targetPos - currentPos, ref lastYRotation); // 변경
                    _unitController.ChangeState(UnitState.Action, _target);
                    yield break;
                }
            }
            else
            {
                Vector3 dir = _unitTable.teamType == TeamType.Player ? Vector2.up : Vector2.down;
                nextPosition += dir * (_unitController.MoveSpeed * Time.deltaTime);
            }

            _unitController.Rigidbody2D.MovePosition(nextPosition);

            yield return new WaitForFixedUpdate();
        }
    }
}