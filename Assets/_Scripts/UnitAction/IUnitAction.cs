using UnityEngine;

public interface IUnitAction
{
    public void Execute(ITarget target, ICaster caster, int effectValue, Vector2 targetPos);
}