using UnityEngine;

public interface ITarget
{
    public Transform Transform { get; }
    public bool IsDead { get; }
    public bool IsUntargetable { get; }
    public void TakeDamage(ICaster caster);
}