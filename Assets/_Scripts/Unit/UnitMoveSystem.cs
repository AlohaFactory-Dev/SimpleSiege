using System.Collections;
using UnityEngine;


public class UnitMoveSystem : MonoBehaviour
{
    private UnitTable _unitTable;
    private Coroutine _moveCoroutine;
    private UnitController _unitController;

    public void Init(UnitController unit)
    {
        _unitController = unit;
        _unitTable = unit.UnitTable;
    }

    public void StartMove(Transform target)
    {
        StopMove();
        _moveCoroutine = StartCoroutine(MoveRoutine(target));
    }

    public void StopMove()
    {
        if (_moveCoroutine != null)
        {
            StopCoroutine(_moveCoroutine);
            _moveCoroutine = null;
        }
    }

    private IEnumerator MoveRoutine(Transform target)
    {
        while (true)
        {
            if (target != null)
            {
                float distance = Vector3.Distance(_unitController.transform.position, target.position);
                if (distance > _unitController.UnitTable.attackAbleRange)
                {
                    Vector3 dir = (target.position - _unitController.transform.position).normalized;
                    _unitController.transform.position += dir * (_unitController.UnitTable.moveSpeed * Time.deltaTime);
                }
            }
            else
            {
                Vector3 dir = _unitController.UnitTable.teamType == TeamType.Player ? Vector2.up : Vector2.down;
                _unitController.transform.position += dir * (_unitController.UnitTable.moveSpeed * Time.deltaTime);
            }

            yield return null;
        }
    }
}