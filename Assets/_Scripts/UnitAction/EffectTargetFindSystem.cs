using System.Collections.Generic;
using Sirenix.Utilities;
using UnityEngine;

public class EffectTargetFindSystem
{
    public ITarget[] FindEffectTargets(ICaster caster, ITarget target, Vector3 position)
    {
        if (caster.AreaType == AreaType.Single)
        {
            return new[] { target };
        }

        var effectTargets = new List<ITarget>();
        Physics2D.OverlapCircleAll(position, caster.EffectRange).ForEach(collider =>
        {
            var effectTarget = collider.GetComponent<ITarget>();
            if (effectTarget != null && !effectTarget.IsUntargetable && CheckTargetType(caster, effectTarget))
            {
                effectTargets.Add(effectTarget);
            }
        });
        return effectTargets.ToArray();
    }

    private bool CheckTargetType(ICaster caster, ITarget target)
    {
        if (caster.TargetType == TargetType.Ally)
        {
            return target.TeamType == caster.TeamType;
        }

        return target.TeamType != caster.TeamType;
    }
}