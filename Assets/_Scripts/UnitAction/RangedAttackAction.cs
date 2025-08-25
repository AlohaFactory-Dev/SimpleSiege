using FactorySystem;
using UnityEngine;

public class RangedAttackAction : IUnitAction
{
    private readonly ICaster _caster;

    public RangedAttackAction(ICaster caster)
    {
        _caster = caster;
    }


    public void Execute(ITarget target, ICaster caster)
    {
        var projectTile = StageConainer.Get<FactoryManager>().AttackObjectFactory.GetAttackObject(caster.UnitTable.projectTileId);
        var projectileTable = TableListContainer.Get<AttackObjectTableList>().GetAttackObjectTable(caster.UnitTable.projectTileId);
        projectTile.Init(_caster.Transform.position, EffectAction, projectileTable, target.Transform.position);
    }

    private void EffectAction()
    {
    }
}