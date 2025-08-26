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
                float distance = Vector3.Distance(currentPos, targetPos);

                if (distance > _unitController.UnitTable.effectAbleRange)
                {
                    Vector3 dir = (targetPos - currentPos).normalized;
                    nextPosition += dir * (_unitController.UnitTable.moveSpeed * Time.deltaTime);

                    // 좌우 회전 처리 (Y축 0 또는 180도)
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
                else
                {
                    float moveDirX = (targetPos - currentPos).x;
                    if (moveDirX != 0)
                    {
                        float yRotation = moveDirX > 0 ? 0f : 180f;
                        if (!Mathf.Approximately(lastYRotation, yRotation))
                        {
                            obj.rotation = Quaternion.Euler(0f, yRotation, 0f);
                        }
                    }

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