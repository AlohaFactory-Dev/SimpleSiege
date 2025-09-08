using System;
using UnityEngine;

public class UnitAnimationEventHandler : MonoBehaviour
{
    private Action _onDieAction;
    private Action _onAction;

    [SerializeField] private Color hitColor = Color.white;
    private MeshRenderer[] _renderers;
    private MaterialPropertyBlock _block;
    private readonly int _id = Shader.PropertyToID("_Black");

    public void Init(Action onDieAction)
    {
        _renderers = GetComponents<MeshRenderer>();
        _block = new MaterialPropertyBlock();
        _onDieAction = onDieAction;
    }

    public void SetOnAction(Action onAction)
    {
        _onAction = onAction;
    }

    // Animation Event
    public void OnDie()
    {
        _onDieAction?.Invoke();
    }

    public void OnHitEffectEvent()
    {
        _block.SetColor(_id, hitColor);
        foreach (var renderer in _renderers)
        {
            renderer.SetPropertyBlock(_block);
        }
    }

    public void OffHitEffectEvent()
    {
        _block.SetColor(_id, Color.black);
        foreach (var renderer in _renderers)
        {
            renderer.SetPropertyBlock(_block);
        }
    }

    public void OnAction()
    {
        _onAction?.Invoke();
    }
}