using System.Collections;
using UnityEngine;


public class UnitMoveSystem : MonoBehaviour
{
    private UnitTable _unitTable;
    private Coroutine _moveCoroutine;
    private UnitController _unitController;
    [SerializeField] private new Rigidbody2D rigidbody;

    public void Init(UnitController unit)
    {
        _unitController = unit;
        _unitTable = unit.UnitTable;
    }

    public void StartMove(ITarget target)
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

    private IEnumerator MoveRoutine(ITarget target)
    {
        while (true)
        {
            Vector3 nextPosition = _unitController.transform.position;
            if (target != null)
            {
                float distance = Vector3.Distance(_unitController.transform.position, target.Transform.position);
                if (distance > _unitController.UnitTable.effectAbleRange)
                {
                    Vector3 dir = (target.Transform.position - _unitController.transform.position).normalized;
                    nextPosition += dir * (_unitController.UnitTable.moveSpeed * Time.deltaTime);
                }
            }
            else
            {
                Vector3 dir = _unitController.UnitTable.teamType == TeamType.Player ? Vector2.up : Vector2.down;
                nextPosition += dir * (_unitController.UnitTable.moveSpeed * Time.deltaTime);
            }

            rigidbody.MovePosition(nextPosition);

            yield return new WaitForFixedUpdate();
        }
    }
}