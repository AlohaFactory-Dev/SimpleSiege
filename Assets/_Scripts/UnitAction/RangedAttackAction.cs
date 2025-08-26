using FactorySystem;
using UnityEngine;

public class RangedAttackAction : IUnitAction
{
    public void Execute(ITarget target, ICaster caster)
    {
        var projectTile = StageConainer.Get<FactoryManager>().AttackObjectFactory.GetAttackObject(caster.UnitTable.projectTileId);
        var projectileTable = TableListContainer.Get<AttackObjectTableList>().GetAttackObjectTable(caster.UnitTable.projectTileId);
        projectTile.Init(caster.Transform.position, Attack, projectileTable, target.Transform.position);
    }

    private void Attack()
    {
        // 공격 이펙트 처리
    }
}