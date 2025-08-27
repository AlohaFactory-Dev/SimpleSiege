using UnityEngine;

public interface ICaster
{
    public Transform Transform { get; }
    public int EffectValue { get; }
    public string ProjectTileId { get; }
    public string EffectVfxId { get; }
}