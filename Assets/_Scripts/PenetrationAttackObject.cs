using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PenetrationAttackObject : AttackObject
{
    private Action<ITarget> _onHitTarget;
    [SerializeField] private float distance;


    public void Set(ICaster caster, Action<ITarget> onHitTarget)
    {
        gameObject.layer = TargetLayerController.GetLayerMaskByTargetType(caster.TeamType, caster.TargetType, caster.TargetGroup);
        _onHitTarget = onHitTarget;
        MoveToTargetDistance(Table.speedType, Table.speedRandomMax);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var target = other.GetComponent<ITarget>();
        if (target == null) return;
        if (target.IsUntargetable) return;
        _onHitTarget?.Invoke(target);
    }

    // speedType에 따라 TargetPosition 방향으로 distance만큼 이동
    void MoveToTargetDistance(AttackObjectSpeedType speedType, float speedOrTime)
    {
        Vector2 startPos = transform.position;
        Vector2 dir = (TargetPosition - startPos).normalized;
        Vector2 targetPos = startPos + dir * distance;
        float duration = speedType == AttackObjectSpeedType.Speed
            ? distance / speedOrTime
            : speedOrTime;
        transform.DOMove(targetPos, duration)
            .SetEase(normalCurve)
            .OnComplete(() =>
            {
                _onHit?.Invoke();
                StartCoroutine(ReleaseDelay());
            });
        ;
    }
}