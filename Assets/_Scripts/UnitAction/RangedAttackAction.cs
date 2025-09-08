using Aloha.Coconut;
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

    public void Execute(ITarget target, ICaster caster, int effectValue, Vector2 targetPos)
    {
        _effectValue = effectValue;
        var projectTile = StageConainer.Get<FactoryManager>().AttackObjectFactory.GetAttackObject(caster.ProjectTileId);
        var projectileTable = TableListContainer.Get<AttackObjectTableList>().GetAttackObjectTable(caster.ProjectTileId);
        projectTile.Init(
            caster.Transform.position,
            () => Attack(caster, target, targetPos),
            projectileTable,
            targetPos
        );
    }

    private void Attack(ICaster caster, ITarget target, Vector3 position)
    {
        if (caster.AreaType == AreaType.Circle)
        {
            PlayParticle(caster.EffectVfxId, position);
        }

        if (target == null || target.IsUntargetable)
        {
            return;
        }


        var targets = _targetFindSystem.FindEffectTargets(caster, target, position);


        if (caster.AreaType == AreaType.Single)
        {
            PlayParticle(caster.EffectVfxId, targets[0].DamageEffectPoint.position);
        }


        targets.ForEach(t => { t.TakeDamage(caster, _effectValue); });
    }

    private void PlayParticle(string effectVfxId, Vector3 position)
    {
        if (!TableManager.IsMagicNumber(effectVfxId))
        {
            var particle = StageConainer.Get<FactoryManager>().ParticleFactory.GetParticle(effectVfxId);
            particle.Init(position);
            particle.Play();
        }
    }
}