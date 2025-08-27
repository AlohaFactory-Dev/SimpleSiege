using System.Collections;
using UnityEngine;

public class UnitMoveSystem : MonoBehaviour
{
    private UnitTable _unitTable;
    private Coroutine _moveCoroutine;
    private UnitController _unitController;
    [SerializeField] private Transform obj;
    [SerializeField] private UnitTargetSystem _targetSystem;
    private ITarget _target;

    public void Init(UnitController unit)
    {
        _unitController = unit;
        _unitTable = unit.UnitTable;
        _targetSystem.Init(unit);
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

    public void StarSiege()
    {
        StopMove();
        _moveCoroutine = StartCoroutine(SiegeRoutine());
    }

    // 경계선 거리 계산 메서드
    private float GetEdgeDistance(Vector3 unitPos, ITarget target)
    {
        var targetCollider2D = target.Collider2D;
        if (targetCollider2D is BoxCollider2D box)
        {
            return Vector3.Distance(unitPos, box.bounds.ClosestPoint(unitPos));
        }
        else
        {
            float targetRadius = targetCollider2D.bounds.extents.magnitude;
            return Mathf.Max(0f, Vector3.Distance(unitPos, target.Transform.position) - targetRadius);
        }
    }

    private void RotateObject(Vector3 dir, ref float lastYRotation)
    {
        if (dir.x != 0)
        {
            float yRotation = dir.x > 0 ? 0f : 180f;
            if (!Mathf.Approximately(lastYRotation, yRotation))
            {
                obj.rotation = Quaternion.Euler(0f, yRotation, 0f);
                lastYRotation = yRotation;
            }
        }
    }

    private IEnumerator SiegeRoutine()
    {
        float lastYRotation = 0f;
        while (true)
        {
            _target = _targetSystem.FindTarget();
            if (_target != null)
            {
                Vector3 targetPos = _target.Transform.position;
                Vector3 currentPos = _unitController.Rigidbody2D.position;
                Vector3 dir = (targetPos - currentPos).normalized;

                float edgeDistance = GetEdgeDistance(currentPos, _target);

                if (edgeDistance <= _unitController.UnitTable.effectAbleRange)
                {
                    RotateObject(dir, ref lastYRotation);
                    _unitController.ChangeState(UnitState.Action, _target, true);
                    yield break;
                }
            }

            yield return null;
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

                float edgeDistance = GetEdgeDistance(currentPos, _target);

                if (edgeDistance > _unitController.UnitTable.effectAbleRange)
                {
                    nextPosition += dir * (_unitController.UnitTable.moveSpeed * Time.deltaTime);
                    RotateObject(dir, ref lastYRotation);
                }
                else
                {
                    RotateObject(targetPos - currentPos, ref lastYRotation);
                    _unitController.ChangeState(UnitState.Action, _target);
                    yield break;
                }
            }
            else
            {
                Vector3 dir = _unitController.UnitTable.teamType == TeamType.Player ? Vector2.up : Vector2.down;
                nextPosition += dir * (_unitController.UnitTable.moveSpeed * Time.deltaTime);
            }

            _unitController.Rigidbody2D.MovePosition(nextPosition);

            yield return new WaitForFixedUpdate();
        }
    }
}