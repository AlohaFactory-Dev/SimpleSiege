using UnityEngine;

public interface ITarget
{
    public Transform Transform { get; }
    public void TakeDamage(ICaster caster);
}