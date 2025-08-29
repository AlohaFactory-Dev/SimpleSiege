using System.Collections.Generic;

public class UnitUpgradeController
{
    // 기본값
    private readonly int _baseEffectValue;
    private readonly int _baseMaxHp;
    private readonly float _baseMoveSpeed;
    private readonly float _baseEffectAbleRange;

    // 누적값 및 수정자
    private float _totalAddedEffectAbleRange;
    private int _totalAddedEffectValue;
    private float _totalBoostSpeed;

    private readonly Dictionary<string, float> _effectAbleRangeModifiers = new();
    private readonly Dictionary<string, int> _effectValueModifiers = new();
    private readonly Dictionary<string, float> _speedModifiers = new();

    // 프로퍼티
    public float EffectAbleRange => _baseEffectAbleRange + _totalAddedEffectAbleRange;
    public int EffectValue => _baseEffectValue + _totalAddedEffectValue;
    public int MaxHp => _baseMaxHp;
    public float MoveSpeed => _baseMoveSpeed + _totalBoostSpeed;

    public UnitUpgradeController(UnitTable unitTable)
    {
        if (unitTable.teamType == TeamType.Enemy)
        {
            var stageTable = StageConainer.Get<StageManager>().CurrentStageTable;
            _baseEffectValue = EffectCalculator.CalculateEffectValue(unitTable.effectValue, unitTable.effectGrowth, stageTable.enemyAttackPowerLevel);
            _baseMaxHp = EffectCalculator.CalculateEffectValue(unitTable.maxHp, unitTable.maxHpGrowth, stageTable.enemyHpLevel);
        }
        else
        {
            _baseEffectValue = unitTable.effectValue;
            _baseMaxHp = unitTable.maxHp;
        }

        _baseEffectAbleRange = unitTable.effectAbleRange;
        _baseMoveSpeed = unitTable.moveSpeed;
        var PassiveManager = StageConainer.Get<PassiveManager>();
        foreach (var passive in PassiveManager.ActivePassives)
        {
        }
    }

    public void ApplyPassive(PassiveTable passiveTable)
    {
        switch (passiveTable.passiveType)
        {
            case PassiveType.EffectAbleRangeUp:
                SetAddedEffectAbleRange(passiveTable.id, passiveTable.effectValue);
                break;
            case PassiveType.AttackPowerUp:
                SetAddedEffectValue(passiveTable.id, (int)passiveTable.effectValue);
                break;
            case PassiveType.MoveSpeedUp:
                SetBoostSpeed(passiveTable.id, passiveTable.effectValue);
                break;
        }
    }

    public void SetAddedEffectAbleRange(string id, float range)
    {
        if (!_effectAbleRangeModifiers.TryAdd(id, range))
            _effectAbleRangeModifiers[id] += range;

        _totalAddedEffectAbleRange = 0f;
        foreach (var mod in _effectAbleRangeModifiers.Values)
            _totalAddedEffectAbleRange += mod;
    }

    public void SetAddedEffectValue(string id, int value)
    {
        if (!_effectValueModifiers.TryAdd(id, value))
            _effectValueModifiers[id] += value;

        _totalAddedEffectValue = 0;
        foreach (var mod in _effectValueModifiers.Values)
            _totalAddedEffectValue += mod;
    }

    public void SetBoostSpeed(string id, float speed)
    {
        if (!_speedModifiers.TryAdd(id, speed))
            _speedModifiers[id] += speed;

        _totalBoostSpeed = 0f;
        foreach (var mod in _speedModifiers.Values)
            _totalBoostSpeed += mod;
    }
}