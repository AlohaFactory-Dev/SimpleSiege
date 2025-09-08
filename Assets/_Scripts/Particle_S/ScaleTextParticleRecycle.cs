using System;
using Aloha.Particle;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class ScaleTextParticleRecycle : RecycleParticle
{
    [SerializeField] private float scaleDuration;
    [SerializeField] private float fadeDuration;
    [SerializeField] private float scale;
    [SerializeField] private AnimationCurve upEase;
    [SerializeField] private AnimationCurve fadeOutEase;
    private Vector3 initialScale;
    private Action<RecycleObject> restoreAction;
    private TextMeshPro textMesh;
    public override ParticleType particleType => ParticleType.Floating;

    public override void InitializeByFactory(Action<RecycleObject> restoreAction)
    {
        this.restoreAction = restoreAction;
        textMesh = GetComponentInChildren<TextMeshPro>();
        initialScale = textMesh.transform.localScale;
    }

    public override void Play()
    {
        textMesh.transform.localScale = initialScale;
        textMesh.alpha = 1f;
        var a = DOTween.Sequence();
        a.Append(transform.DOScale(scale, scaleDuration).SetEase(upEase));
        a.Append(textMesh.DOFade(0, fadeDuration).SetEase(fadeOutEase))
            .OnComplete(() => { restoreAction.Invoke(this); });
    }
}