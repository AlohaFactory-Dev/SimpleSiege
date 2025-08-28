using System.Collections.Generic;
using Sirenix.Utilities;
using UnityEngine;

public class EffectTargetFindSystem
{
    public ITarget[] FindEffectTargets(ICaster caster, ITarget target)
    {
        if (caster.AreaType == AreaType.Single)
        {
            return new[] { target };
        }


        var effectTargets = new List<ITarget>();
        Physics2D.OverlapCircleAll(caster.Transform.position, caster.EffectRange).ForEach(collider =>
        {
            var effectTarget = collider.GetComponent<ITarget>();
            if (effectTarget != null && !effectTarget.IsUntargetable && effectTarget.TeamType != target.TeamType)
            {
                effectTargets.Add(effectTarget);
            }
        });

        return effectTargets.ToArray();
    }
}