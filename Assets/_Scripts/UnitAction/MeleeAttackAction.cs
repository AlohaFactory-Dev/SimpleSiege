using UnityEngine;

public class MeleeAttackAction : IUnitAction
{
    private ICaster _caster;

    public MeleeAttackAction(ICaster caster)
    {
        _caster = caster;
    }

    public void Execute(ITarget target, ICaster caster)
    {
        target.TakeDamage(caster);
    }
}