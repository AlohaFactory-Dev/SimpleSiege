using UnityEngine;

public interface ITarget
{
    public Transform Transform { get; }
    public TeamType TeamType { get; }
    public TargetGroup Group { get; }
    public bool IsDead { get; }
    public bool IsUntargetable { get; }
    public int MaxHp { get; }
    public void TakeDamage(ICaster caster);
}