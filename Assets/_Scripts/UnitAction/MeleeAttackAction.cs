using UnityEngine;

public class MeleeAttackAction : IUnitAction
{
    private ICaster _caster;


    public void Execute(ITarget target, ICaster caster)
    {
        target.TakeDamage(caster);
    }
}