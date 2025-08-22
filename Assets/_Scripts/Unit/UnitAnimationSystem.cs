using UnityEngine;

namespace _Scripts.Unit
{
    public class UnitAnimationSystem : MonoBehaviour
    {
        private Animator _animator;
        private UnitTable _unitTable;

        public void Init()
        {
            _animator = GetComponentInChildren<Animator>();
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
    }
}