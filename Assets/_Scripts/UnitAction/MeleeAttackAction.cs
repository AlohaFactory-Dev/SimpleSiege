using Aloha.Coconut;
using FactorySystem;
using Sirenix.Utilities;
using UnityEngine;

public class MeleeAttackAction : IUnitAction
{
    private readonly EffectTargetFindSystem _targetFindSystem;
    private readonly FactoryManager _factoryManager;

    public MeleeAttackAction()
    {
        _targetFindSystem = new EffectTargetFindSystem();
        _factoryManager = StageConainer.Get<FactoryManager>();
    }

    public void Execute(ITarget target, ICaster caster, int effectValue, Vector2 targetPos)
    {
        if (target == null || target.IsUntargetable) return;
        var targets = _targetFindSystem.FindEffectTargets(caster, target, target.Transform.position);
        targets.ForEach(t =>
        {
            if (caster.AreaType == AreaType.Single)
            {
                if (!TableManager.IsMagicNumber(caster.EffectVfxId))
                {
                    var particle = _factoryManager.ParticleFactory.GetParticle(caster.EffectVfxId);
                    particle.Init(t.DamageEffectPoint.position);
                    particle.Play();
                }
            }

            t.TakeDamage(caster, effectValue);
        });
    }
}