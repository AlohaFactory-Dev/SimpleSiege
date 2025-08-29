using System;
using UnityEngine;


public class UnitAnimationSystem : MonoBehaviour
{
    private Animator _animator;
    private UnitAnimationEventHandler _animationEventHandler;
    private UnitTable _unitTable;
    public float AcionDuration { get; private set; }

    public void Init(Action onDieAction)
    {
        _animator = GetComponentInChildren<Animator>();
        _animationEventHandler = _animator.GetComponentInChildren<UnitAnimationEventHandler>();
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
}