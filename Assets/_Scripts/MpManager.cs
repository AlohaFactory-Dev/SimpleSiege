using UniRx;
using UnityEngine;

public class MpManager
{
    public IReadOnlyReactiveProperty<int> CurrentMp => _currentMp;
    private readonly ReactiveProperty<int> _currentMp;

    public MpManager()
    {
        var initialMp = (int)TableListContainer.Get<EtcTableList>().GetEtcTable("initial").values[0];
        _currentMp = new ReactiveProperty<int>(initialMp);
    }


    public bool ConsumeMp(int amount)
    {
        if (_currentMp.Value >= amount)
        {
            _currentMp.Value -= amount;
            return true;
        }

        return false;
    }

    public void AddMp(int amount)
    {
        _currentMp.Value += amount;
    }
}