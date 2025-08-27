using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class UnitTargetSystem : MonoBehaviour
{
    private UnitController _unitController;
    private CircleCollider2D _collider;
    private readonly List<ITarget> _targetsInSight = new();

    public void Init(UnitController unitController)
    {
        _targetsInSight.Clear();
        _unitController = unitController;
        _collider = GetComponent<CircleCollider2D>();
        _collider.isTrigger = true;
        _collider.radius = unitController.UnitTable.sightRange;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var target = other.GetComponent<ITarget>();
        if (CheckAddAble(target))
        {
            if (!_targetsInSight.Contains(target))
                _targetsInSight.Add(target);
        }
    }

    private bool CheckAddAble(ITarget target)
    {
        if (target.Group != _unitController.UnitTable.targetGroup && _unitController.UnitTable.targetGroup != TargetGroup.All)
        {
            return false;
        }

        if (_unitController.UnitTable.targetType == TargetType.Ally)
        {
            return target.TeamType == _unitController.UnitTable.teamType;
        }

        if (_unitController.UnitTable.targetType == TargetType.Enemy)
        {
            return target.TeamType != _unitController.UnitTable.teamType;
        }

        if (_unitController.UnitTable.targetType == TargetType.All)
        {
            return target != _unitController; // 자기 자신은 제외
        }

        return false;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var enemy = other.GetComponent<UnitController>();
        if (_targetsInSight.Contains(enemy))
        {
            _targetsInSight.Remove(enemy);
        }
    }


    public ITarget FindTarget()
    {
        ITarget closest = null;
        if (_unitController.UnitTable.targetSelectionType == TargetSelectionType.Nearest)
        {
            float minDist = float.MaxValue;
            foreach (var t in _targetsInSight)
            {
                if (t.IsUntargetable) continue;
                float dist = Vector3.Distance(_unitController.transform.position, t.Transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    closest = t;
                }
            }
        }
        else if (_unitController.UnitTable.targetSelectionType == TargetSelectionType.HighestHp)
        {
            int maxHp = int.MinValue;
            foreach (var t in _targetsInSight)
            {
                if (t.IsUntargetable) continue;
                if (t.MaxHp > maxHp)
                {
                    maxHp = t.MaxHp;
                    closest = t;
                }
            }
        }

        return closest;
    }
}