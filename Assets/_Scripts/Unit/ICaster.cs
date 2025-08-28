using UnityEngine;

public interface ICaster
{
    public Transform Transform { get; }
    public string ProjectTileId { get; }
    public string EffectVfxId { get; }
    public AreaType AreaType { get; }
    public float EffectRange { get; }
}