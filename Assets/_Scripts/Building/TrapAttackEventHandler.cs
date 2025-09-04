using System;
using UnityEngine;

public class TrapAttackEventHandler : MonoBehaviour
{
    private Action OnAttack;

    public void Init(Action onAttack)
    {
        OnAttack = onAttack;
    }

    public void Attack()
    {
        OnAttack?.Invoke();
    }
}