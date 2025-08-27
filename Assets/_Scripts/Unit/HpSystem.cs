using Aloha.CoconutMilk;
using UnityEngine;

public class HpSystem : MonoBehaviour
{
    private BarGauge _hpBarGauge;
    private int _currentHp;
    public bool IsDead => _currentHp <= 0;

    public void Init(int maxHp)
    {
        _hpBarGauge = GetComponentInChildren<BarGauge>(true);
        _hpBarGauge.Initialize(maxHp, null, maxHp);
        _hpBarGauge.Off();
        _currentHp = maxHp;
    }

    public bool TakeDamage(int damage)
    {
        _currentHp -= damage;
        if (_currentHp <= 0)
        {
            _currentHp = 0;
            _hpBarGauge.Off();
            return true; // 사망
        }

        _hpBarGauge.On();


        _hpBarGauge.SetValue(_currentHp);
        return false; // 생존
    }
}