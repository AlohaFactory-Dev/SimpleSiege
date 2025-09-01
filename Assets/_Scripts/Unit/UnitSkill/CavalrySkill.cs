using System;
using UniRx;
using UnityEngine;

public class CavalrySkill : MonoBehaviour
{
    private string _skillId = "CavalrySkill";
    private float _skillActiveTime;
    private float _boostSpeed;
    private float _effectValueMultiplier = 2f;
    private UnitController _unitController;
    private float _skillActiveTimer;
    private bool _onSkill = false;

    private void Start()
    {
        _unitController = GetComponent<UnitController>();
        _skillActiveTime = _unitController.UnitTable.values[0];
        _boostSpeed = _unitController.UnitTable.values[1];
        _effectValueMultiplier = _unitController.UnitTable.values[2];
        _unitController.StatusSystem.ActionSystem.OnActionNotice.Subscribe(_ => UnActivateSkill()).AddTo(this);
    }

    private void OnDisable()
    {
        _skillActiveTimer = 0f;
    }

    private void Update()
    {
        if (!_onSkill && !_unitController.IsUntargetable)
        {
            _skillActiveTimer += Time.deltaTime;
            if (_skillActiveTimer >= _skillActiveTime)
            {
                ActivateSkill();
                _skillActiveTimer = 0;
            }
        }
    }

    private void ActivateSkill()
    {
        if (_onSkill) return;
        _unitController.StatusSystem.AnimationSystem.PlaySkill();
        _onSkill = true;
        _skillActiveTimer = 0f;
        _unitController.UnitUpgradeController.ApplyUpgrade(_skillId, UpgradeType.MoveSpeed, new UpgradeValue(UpgradeValueType.Additive, _boostSpeed));
        _unitController.UnitUpgradeController.ApplyUpgrade(_skillId, UpgradeType.EffectValue, new UpgradeValue(UpgradeValueType.Multiplicative, _effectValueMultiplier));
    }

    private void UnActivateSkill()
    {
        if (!_onSkill) return;
        _onSkill = false;
        _unitController.UnitUpgradeController.ApplyUpgrade(_skillId, UpgradeType.MoveSpeed, new UpgradeValue(UpgradeValueType.Additive, -_boostSpeed));
        _unitController.UnitUpgradeController.ApplyUpgrade(_skillId, UpgradeType.EffectValue, new UpgradeValue(UpgradeValueType.Multiplicative, -_effectValueMultiplier));
    }
}