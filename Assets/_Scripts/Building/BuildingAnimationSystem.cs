using UnityEngine;

public class BuildingAnimationSystem : MonoBehaviour
{
    private Animator _animator;

    private readonly int _destroyTrigger = Animator.StringToHash("Destroy");
    private readonly int _hitTrigger = Animator.StringToHash("Hit");

    public void Init()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    public void PlayDestroyAnimation()
    {
        _animator.SetTrigger(_destroyTrigger);
    }

    public void TakeDamage()
    {
        _animator.SetTrigger(_hitTrigger);
    }
}