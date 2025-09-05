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
    private UnitRotationSystem _rotationSystem;
    [HideInInspector] public bool onOnlyMoveYAxis = false;

    public void Init(UnitController unit, UnitTargetSystem targetSystem, UnitRotationSystem rotationSystem)
    {
        _unitController = unit;
        _unitTable = unit.UnitTable;
        _targetSystem = targetSystem;
        _rotationSystem = rotationSystem;
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
            Vector3 currentPosition = _unitController.Transform.position;
            Vector2 targetVelocity = Vector2.zero;
            _target = _targetSystem.FindTarget();

            if (_target != null && !onOnlyMoveYAxis)
            {
                Vector3 targetPos = _target.Transform.position;
                Vector3 dir = (targetPos - currentPosition).normalized;

                float edgeDistance = _targetSystem.GetEdgeDistance(currentPosition, _target);

                if (edgeDistance > _unitController.EffectAbleRange)
                {
                    targetVelocity = new Vector2(dir.x, dir.y) * _unitController.MoveSpeed;
                    _rotationSystem.Rotate(dir, ref lastYRotation);
                }
                else
                {
                    _rotationSystem.Rotate(targetPos - currentPosition, ref lastYRotation);
                    _unitController.ChangeState(UnitState.Action, _target);
                    yield break;
                }
            }
            else
            {
                Vector2 dir = _unitTable.teamType == TeamType.Player ? Vector2.up : Vector2.down;
                targetVelocity = dir * _unitController.MoveSpeed;
            }

            _unitController.Rigidbody2D.velocity = targetVelocity;

            yield return new WaitForFixedUpdate();
        }
    }
}