using FactorySystem;
using Sirenix.Utilities;
using UnityEngine;

public class RangedAttackAction : IUnitAction
{
    private int _effectValue;
    private readonly EffectTargetFindSystem _targetFindSystem;

    public RangedAttackAction()
    {
        _targetFindSystem = new EffectTargetFindSystem();
    }

    public void Execute(ITarget target, ICaster caster, int effectValue)
    {
        _effectValue = effectValue;
        var projectTile = StageConainer.Get<FactoryManager>().AttackObjectFactory.GetAttackObject(caster.ProjectTileId);
        var projectileTable = TableListContainer.Get<AttackObjectTableList>().GetAttackObjectTable(caster.ProjectTileId);
        projectTile.Init(caster.Transform.position, () => Attack(caster, target, target.Transform.position), projectileTable, target.Transform.position);
    }

    private void Attack(ICaster caster, ITarget target, Vector3 position)
    {
        _targetFindSystem.FindEffectTargets(caster, target, position).ForEach(effectTarget => { effectTarget.TakeDamage(caster, _effectValue); });
    }
}