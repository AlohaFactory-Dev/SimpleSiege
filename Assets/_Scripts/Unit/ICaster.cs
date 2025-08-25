using UnityEngine;

public interface ICaster
{
    public Transform Transform { get; }
    public UnitTable UnitTable { get; }
    public int EffectValue { get; }
}