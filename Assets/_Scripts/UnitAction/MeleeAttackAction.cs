using Sirenix.Utilities;
using UnityEngine;

public class MeleeAttackAction : IUnitAction
{
    private readonly EffectTargetFindSystem _targetFindSystem;

    public MeleeAttackAction()
    {
        _targetFindSystem = new EffectTargetFindSystem();
    }

    public void Execute(ITarget target, ICaster caster, int effectValue)
    {
        _targetFindSystem.FindEffectTargets(caster, target).ForEach(effectTarget => { effectTarget.TakeDamage(caster, effectValue); });
    }
}