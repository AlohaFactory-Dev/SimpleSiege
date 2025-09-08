using UniRx;
using UnityEngine;

public interface ICaster
{
    public Transform Transform { get; }
    public string ProjectTileId { get; }
    public string EffectVfxId { get; }
    public TargetType TargetType { get; }
    public TeamType TeamType { get; }
    public AreaType AreaType { get; }
    public float EffectRange { get; }
    public IReadOnlyReactiveProperty<float> EffectActionSpeed { get; }
}