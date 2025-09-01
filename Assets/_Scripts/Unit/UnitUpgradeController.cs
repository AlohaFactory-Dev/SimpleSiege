using System.Collections.Generic;
using UniRx;
using UnityEngine;

public enum UpgradeValueType { Additive, Multiplicative }

public class UpgradeValue
{
    public readonly UpgradeValueType Type;
    public readonly float Value;

    public UpgradeValue(UpgradeValueType type, float value)
    {
        Type = type;
        Value = value;
    }
}

public class UnitUpgradeController
{
    // 기본값
    private readonly int _baseEffectValue;
    private readonly int _baseMaxHp;
    private readonly float _baseMoveSpeed;
    private readonly float _baseEffectAbleRange;
    private readonly float _baseSightRange;
    private readonly float _baseEffectRange;
    private readonly float _baseAttackSpeed;


    // 업그레이드 관리
    private readonly Dictionary<string, UpgradeValue> _effectAbleRangeUpgrades = new();
    private readonly Dictionary<string, UpgradeValue> _effectValueUpgrades = new();
    private readonly Dictionary<string, UpgradeValue> _speedUpgrades = new();
    private readonly Dictionary<string, UpgradeValue> _maxHpUpgrades = new();
    private readonly Dictionary<string, UpgradeValue> _effectRangeUpgrades = new();
    private readonly Dictionary<string, UpgradeValue> _attackSpeedUpgrades = new();

    // 프로퍼티
    public float EffectAbleRange { get; private set; }
    public int EffectValue { get; private set; }
    public ReactiveProperty<int> MaxHp { get; private set; }
    public float MoveSpeed { get; private set; }
    public ReactiveProperty<float> SightRange { get; private set; }
    public float EffectRange { get; private set; }
    public ReactiveProperty<float> EffectActionSpeed { get; private set; }

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
        _baseSightRange = unitTable.sightRange;
        _baseEffectRange = unitTable.effectRange;
        _baseAttackSpeed = 1f;

        ResetUpgrades();

        foreach (var passive in StageConainer.Get<PassiveManager>().ActivePassives)
            ApplyUpgrade(passive.id, passive.passiveType, new UpgradeValue(passive.upgradeValueType, passive.effectValue));
    }

    private void ResetUpgrades()
    {
        _effectAbleRangeUpgrades.Clear();
        _effectValueUpgrades.Clear();
        _speedUpgrades.Clear();

        RecalculateAll();
    }


    public void ApplyUpgrade(string id, UpgradeType upgradeType, UpgradeValue skillUpgrade)
    {
        switch (upgradeType)
        {
            case UpgradeType.EffectAbleRange:
                AddUpgrade(_effectAbleRangeUpgrades, id, skillUpgrade.Value, skillUpgrade.Type);
                break;
            case UpgradeType.EffectValue:
                AddUpgrade(_effectValueUpgrades, id, skillUpgrade.Value, skillUpgrade.Type);
                break;
            case UpgradeType.MoveSpeed:
                AddUpgrade(_speedUpgrades, id, skillUpgrade.Value, skillUpgrade.Type);
                break;
            case UpgradeType.MaxHp:
                AddUpgrade(_maxHpUpgrades, id, skillUpgrade.Value, skillUpgrade.Type);
                break;
            case UpgradeType.EffectRange:
                AddUpgrade(_effectRangeUpgrades, id, skillUpgrade.Value, skillUpgrade.Type);
                break;
            case UpgradeType.EffectActionSpeed:
                AddUpgrade(_attackSpeedUpgrades, id, skillUpgrade.Value, skillUpgrade.Type);
                break;
        }
    }

    private void AddUpgrade(Dictionary<string, UpgradeValue> dict, string id, float value, UpgradeValueType type)
    {
        if (value < 0f) dict.Remove(id);
        else dict[id] = new UpgradeValue(type, value);

        RecalculateAll();
    }

    private void RecalculateAll()
    {
        EffectAbleRange = CalculateUpgrade(_baseEffectAbleRange, _effectAbleRangeUpgrades);
        EffectValue = Mathf.CeilToInt(CalculateUpgrade(_baseEffectValue, _effectValueUpgrades));
        MoveSpeed = CalculateUpgrade(_baseMoveSpeed, _speedUpgrades);
        SightRange.Value = CalculateUpgrade(_baseSightRange, _effectAbleRangeUpgrades);
        MaxHp.Value = Mathf.CeilToInt(CalculateUpgrade(_baseMaxHp, _maxHpUpgrades));
        EffectRange = CalculateUpgrade(_baseEffectRange, _effectRangeUpgrades);
        EffectActionSpeed.Value = CalculateUpgrade(_baseAttackSpeed, _attackSpeedUpgrades);
    }

    private float CalculateUpgrade(float baseValue, Dictionary<string, UpgradeValue> dict)
    {
        float add = 0f;
        float mul = 0f;
        foreach (var upgrade in dict.Values)
        {
            if (upgrade.Type == UpgradeValueType.Additive) add += upgrade.Value;
            else mul += upgrade.Value;
        }

        return baseValue * (1 + mul) + add;
    }
}