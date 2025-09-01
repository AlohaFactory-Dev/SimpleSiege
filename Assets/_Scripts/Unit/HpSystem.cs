using System;
using Aloha.CoconutMilk;
using UniRx;
using UnityEngine;

public class HpSystem : MonoBehaviour
{
    [SerializeField] private BarGauge enemyHpBar;
    [SerializeField] private BarGauge playerHpBar;
    private int _baseMaxHp;
    private BarGauge _selectedHpBar;
    private int _currentHp;
    public bool IsDead => _currentHp <= 0;
    private IDisposable _maxHpChangeSubscription;

    public void Init(IReadOnlyReactiveProperty<int> maxHp, TeamType teamType, int baseMaxHp)
    {
        if (teamType == TeamType.Player)
        {
            enemyHpBar.Off();
            _selectedHpBar = playerHpBar;
        }
        else
        {
            playerHpBar.Off();
            _selectedHpBar = enemyHpBar;
        }

        _selectedHpBar.Initialize(maxHp.Value, null, maxHp.Value);
        _selectedHpBar.Off();
        _currentHp = maxHp.Value;
        _baseMaxHp = baseMaxHp;

        if (_maxHpChangeSubscription != null)
        {
            _maxHpChangeSubscription.Dispose();
            _maxHpChangeSubscription = null;
        }

        _maxHpChangeSubscription = maxHp.Pairwise().Subscribe(pair =>
        {
            var preMaxHp = pair.Previous;
            var newMaxHp = pair.Current;
            if (newMaxHp < _baseMaxHp)
                newMaxHp = _baseMaxHp;
            _currentHp += newMaxHp - preMaxHp;
            if (_currentHp > newMaxHp)
                _currentHp = newMaxHp;

            _selectedHpBar.SetMaxValue(newMaxHp);
            _selectedHpBar.SetValue(_currentHp);
        }).AddTo(this);
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