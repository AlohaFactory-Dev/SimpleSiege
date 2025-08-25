using System;
using UnityEngine;


public class UnitAnimationSystem : MonoBehaviour
{
    private Animator _animator;
    private UnitTable _unitTable;

    public void Init(Action onDieAction)
    {
        _animator = GetComponentInChildren<Animator>();
        _animator.GetComponentInChildren<UnitDieEventHandler>().Init(onDieAction);
    }

    public void PlayMove()
    {
        _animator.SetTrigger("Move");
    }

    public void PlayAttack()
    {
        _animator.SetTrigger("Attack");
    }

    public void PlayIdle()
    {
        _animator.SetTrigger("Idle");
    }

    public void PlayDead()
    {
        _animator.SetTrigger("Dead");
    }
}