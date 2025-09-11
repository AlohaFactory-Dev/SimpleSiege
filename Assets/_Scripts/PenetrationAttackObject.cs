using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenetrationAttackObject : AttackObject
{
    private Action<ITarget> _onHitTarget;

    public void Set(ICaster caster, Action<ITarget> onHitTarget)
    {
        gameObject.layer = TargetLayerController.GetLayerMaskByTargetType(caster.TeamType, caster.TargetType, caster.TargetGroup);
        _onHitTarget = onHitTarget;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var target = other.GetComponent<ITarget>();
        if (target == null) return;
        if (target.IsUntargetable) return;
        _onHitTarget?.Invoke(target);
    }
}