using System;
using UniRx;
using UnityEngine;


public class UnitAnimationSystem : MonoBehaviour
{
    private Animator _animator;
    private UnitAnimationEventHandler _animationEventHandler;
    public float AcionDuration { get; private set; }
    private IDisposable _speedChangeSubscription;

    public void Init(Action onDieAction, UnitController unitController)
    {
        GetComponents();
        _animationEventHandler.Init(onDieAction);
        // Animator에 연결된 모든 AnimationClip 가져오기
        var clips = _animator.runtimeAnimatorController.animationClips;
        foreach (var clip in clips)
        {
            if (clip.name == "action")
            {
                AcionDuration = clip.length;
                break;
            }
        }

        if (_speedChangeSubscription != null)
        {
            _speedChangeSubscription.Dispose();
            _speedChangeSubscription = null;
        }

        _speedChangeSubscription = unitController.EffectActionSpeed.Subscribe(SetActionSpeed).AddTo(this);
        SetActionSpeed(unitController.EffectActionSpeed.Value);
    }

    private void GetComponents()
    {
        if (_animator != null) return;
        _animator = GetComponentInChildren<Animator>();
        _animationEventHandler = _animator.GetComponentInChildren<UnitAnimationEventHandler>();
    }

    private void SetActionSpeed(float speed)
    {
        _animator.SetFloat("EffectActionSpeed", speed);
    }

    public void SetOnAction(Action onAction)
    {
        _animationEventHandler.SetOnAction(onAction);
    }

    public void PlaySpawn()
    {
        _animator.SetTrigger("Spawn");
    }

    public void PlayMove()
    {
        _animator.SetTrigger("Move");
    }

    public void PlayAction()
    {
        _animator.SetTrigger("Action");
    }

    public void PlayIdle()
    {
        _animator.SetTrigger("Idle");
    }

    public void PlayDead()
    {
        _animator.SetTrigger("Dead");
    }

    public void PlaySkill()
    {
        _animator.SetTrigger("Skill");
    }

    public void PlayHit()
    {
        _animator.SetTrigger("Hit");
    }
}