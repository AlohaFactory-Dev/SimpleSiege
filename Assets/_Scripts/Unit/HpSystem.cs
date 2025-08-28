using Aloha.CoconutMilk;
using UnityEngine;

public class HpSystem : MonoBehaviour
{
    [SerializeField] private BarGauge enemyHpBar;
    [SerializeField] private BarGauge playerHpBar;
    private BarGauge _selectedHpBar;
    private int _currentHp;
    public bool IsDead => _currentHp <= 0;

    public void Init(int maxHp, TeamType team)
    {
        if (team == TeamType.Player)
        {
            enemyHpBar.Off();
            _selectedHpBar = playerHpBar;
        }
        else
        {
            playerHpBar.Off();
            _selectedHpBar = enemyHpBar;
        }

        _selectedHpBar.Initialize(maxHp, null, maxHp);
        _selectedHpBar.Off();
        _currentHp = maxHp;
    }

    public bool TakeDamage(int damage)
    {
        _currentHp -= damage;
        if (_currentHp <= 0)
        {
            _currentHp = 0;
            _selectedHpBar.Off();
            return true; // 사망
        }

        _selectedHpBar.On();


        _selectedHpBar.SetValue(_currentHp);
        return false; // 생존
    }
}