using System;
using UniRx;
using UnityEngine;

public class CavalrySkill : MonoBehaviour
{
    private string _skillId = "CavalrySkill";
    private float _skillActiveDistance;
    private float _boostSpeed;
    private float _effectValueMultiplier = 2f;
    private UnitController _unitController;
    private bool _onSkill = false;
    private Vector2 _detectionBoxSize = new Vector2(5f, 5f);
    private CircleCollider2D _collider2D;
    private float skillOnYPosition;
    private float coolDownTime = 2f;
    private float coolDownTimer = 0f;

    public void Init(UnitController unitController)
    {
        _collider2D = GetComponent<CircleCollider2D>();
        _collider2D.isTrigger = true;
        _unitController = unitController;
        _skillActiveDistance = _unitController.UnitTable.values[0];
        _boostSpeed = _unitController.UnitTable.values[1];
        _effectValueMultiplier = _unitController.UnitTable.values[2];
        _detectionBoxSize = new Vector2(_unitController.UnitTable.values[3], _unitController.UnitTable.values[4]);
        coolDownTime = _unitController.UnitTable.values[5];
        coolDownTimer = coolDownTime;
        _collider2D.enabled = false;
        _collider2D.radius = _unitController.UnitTable.values[6];
        _onSkill = false;
        _unitController.Collider2D.isTrigger = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var target = other.GetComponent<ITarget>();
        if (target != null && target.TeamType != _unitController.TeamType && !target.IsUntargetable)
        {
            target.TakeDamage(_unitController, Mathf.CeilToInt(_unitController.EffectValue * _effectValueMultiplier));
            if (target.Group == TargetGroup.Building)
            {
                UnActivateSkill();
            }
        }
    }

    private void Update()
    {
        if (_unitController.State == UnitState.Spawn) return;
        if (!_onSkill)
        {
            if (coolDownTimer < coolDownTime)
            {
                coolDownTimer += Time.deltaTime;
                return;
            }

            FindTargetAndActivate();
        }
        else
        {
            if (Mathf.Abs(transform.position.y - skillOnYPosition) >= _skillActiveDistance)
            {
                UnActivateSkill();
            }
        }
    }

    private void FindTargetAndActivate()
    {
        float direction = _unitController.TeamType == TeamType.Player ? 1 : -1;
        Vector2 center = (Vector2)_unitController.transform.position + new Vector2(0, direction * _detectionBoxSize.y * 0.5f);
        Vector2 size = _detectionBoxSize;
        Collider2D[] targets = Physics2D.OverlapBoxAll(center, size, 0f);

        foreach (var collider in targets)
        {
            var target = collider.GetComponent<ITarget>();
            if (target != null && target.TeamType != _unitController.TeamType && !target.IsUntargetable)
            {
                ActivateSkill();
                break;
            }
        }
    }


    private void ActivateSkill()
    {
        if (_onSkill) return;
        coolDownTimer = 0;
        skillOnYPosition = transform.position.y;
        _collider2D.enabled = true;
        _unitController.StatusSystem.AnimationSystem.PlaySkill();
        _onSkill = true;
        _unitController.UnitUpgradeController.ApplyUpgrade(_skillId, UpgradeType.MoveSpeedUp, new UpgradeValue(UpgradeValueType.Additive, _boostSpeed));
        _unitController.StatusSystem.MoveSystem.OnOnlyMoveYAxis = true;
        _unitController.Collider2D.isTrigger = true;
    }

    private void UnActivateSkill()
    {
        if (!_onSkill) return;
        coolDownTimer = 0;
        _collider2D.enabled = false;
        _onSkill = false;
        _unitController.UnitUpgradeController.ApplyUpgrade(_skillId, UpgradeType.MoveSpeedUp, new UpgradeValue(UpgradeValueType.Additive, -_boostSpeed));
        _unitController.StatusSystem.MoveSystem.OnOnlyMoveYAxis = false;
        _unitController.Collider2D.isTrigger = false;
    }
}