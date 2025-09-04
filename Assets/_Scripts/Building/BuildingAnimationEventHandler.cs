using System;
using UnityEngine;

public class BuildingAnimationEventHandler : MonoBehaviour
{
    private Action _onDestroyAction;
    [SerializeField] private Color hitColor = new Color32(193, 193, 193, 255);
    private SpriteRenderer[] _renderers;
    private MaterialPropertyBlock _block;
    private readonly int _id = Shader.PropertyToID("_Black");

    public void Init(Action onDestroyAction)
    {
        _onDestroyAction = onDestroyAction;
        _renderers = GetComponentsInChildren<SpriteRenderer>();
        _block = new MaterialPropertyBlock();
    }

    public void OnHitEffectEvent()
    {
        _block.SetColor(_id, hitColor);
        foreach (var renderer in _renderers)
        {
            renderer.material.SetColor(_id, hitColor);
        }
    }

    public void OffHitEffectEvent()
    {
        _block.SetColor(_id, Color.black);
        foreach (var renderer in _renderers)
        {
            renderer.material.SetColor(_id, Color.black);
        }
    }

    // Animation Event
    public void OnDestroyBuilding()
    {
        _onDestroyAction?.Invoke();
    }
}