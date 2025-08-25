using System.Collections;
using UnityEngine;


public class UnitActionSystem : MonoBehaviour
{
    private UnitTable _unitTable;
    private Coroutine _actionCoroutine;
    private UnitController _unitController;

    public void Init(UnitController unitController)
    {
        // UnitController에서 UnitTable을 가져와서 초기화
        _unitController = unitController;
        _unitTable = unitController.UnitTable;
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
            if (target != null)
            {
                // 공격 타이밍 체크 및 실제 공격 로직 구현
                // 예시: target.GetComponent<Health>()?.TakeDamage(unit.UnitTable.attackPower);
                // 액션이 끝났다고 가정할 때(예시로 1회 공격 후 종료)
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
}