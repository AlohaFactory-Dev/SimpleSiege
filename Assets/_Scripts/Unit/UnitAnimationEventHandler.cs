using System;
using UnityEngine;

public class UnitAnimationEventHandler : MonoBehaviour
{
    private Action _onDieAction;
    private Action _onAction;

    [SerializeField] private Color hitColor = Color.white;
    private MeshRenderer _renderer;
    private MaterialPropertyBlock _block;
    private readonly int _id = Shader.PropertyToID("_Black");

    public void Init(Action onDieAction)
    {
        _renderer = GetComponent<MeshRenderer>();
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
        _renderer.SetPropertyBlock(_block);
    }

    public void OffHitEffectEvent()
    {
        _block.SetColor(_id, Color.black);
        _renderer.SetPropertyBlock(_block);
    }

    public void OnAction()
    {
        _onAction?.Invoke();
    }
}