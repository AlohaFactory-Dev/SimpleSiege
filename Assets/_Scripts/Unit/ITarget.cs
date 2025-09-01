using UniRx;
using UnityEngine;

public interface ITarget
{
    public Transform Transform { get; }
    public TeamType TeamType { get; }
    public TargetGroup Group { get; }
    public Collider2D Collider2D { get; }
    public bool IsUntargetable { get; }
    public IReadOnlyReactiveProperty<int> MaxHp { get; }
    public void TakeDamage(ICaster caster, int damage);
}