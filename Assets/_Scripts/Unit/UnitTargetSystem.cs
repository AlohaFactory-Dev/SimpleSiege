using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class UnitTargetSystem : MonoBehaviour
{
    private UnitController _unitController;
    private CircleCollider2D _collider;
    [SerializeField] private List<UnitController> _targetsInSight = new();

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
        var target = other.GetComponent<UnitController>();
        if (CheckTargetType(target))
        {
            if (!_targetsInSight.Contains(target))
                _targetsInSight.Add(target);
        }
    }

    private bool CheckTargetType(UnitController target)
    {
        if (_unitController.UnitTable.targetType == TargetType.Ally)
        {
            return target.UnitTable.teamType == _unitController.UnitTable.teamType;
        }
        else if (_unitController.UnitTable.targetType == TargetType.Enemy)
        {
            return target.UnitTable.teamType != _unitController.UnitTable.teamType;
        }
        else // TargetType.All
        {
            return target != _unitController; // 자기 자신은 제외
        }
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
        float minDist = float.MaxValue;
        foreach (var t in _targetsInSight)
        {
            if (t.IsDead || t.IsUntargetable) continue;
            float dist = Vector3.Distance(_unitController.transform.position, t.Transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = t;
            }
        }

        return closest;
    }
}