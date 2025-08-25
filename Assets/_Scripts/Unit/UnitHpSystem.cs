using Aloha.CoconutMilk;
using UnityEngine;

public class UnitHpSystem : MonoBehaviour
{
    private BarGauge _hpBarGauge;
    private int _currentHp;

    public void Init(UnitTable unitTable)
    {
        _hpBarGauge = GetComponentInChildren<BarGauge>(true);
        _hpBarGauge.Initialize(unitTable.hp, null, unitTable.hp);
        _currentHp = unitTable.hp;
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

        _hpBarGauge.SetValue(_currentHp);
        return false; // 생존
    }
}