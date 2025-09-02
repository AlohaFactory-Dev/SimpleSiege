using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class UnitTargetSystem : MonoBehaviour
{
    private UnitController _unitController;
    private CircleCollider2D _collider;
    private readonly List<ITarget> _targetsInSight = new();
    private IDisposable _updateSightRangeDisposable;

    public void Init(UnitController unitController)
    {
        _targetsInSight.Clear();
        _unitController = unitController;
        _collider = GetComponent<CircleCollider2D>();
        _collider.isTrigger = true;

        if (_updateSightRangeDisposable != null)
        {
            _updateSightRangeDisposable.Dispose();
            _updateSightRangeDisposable = null;
        }

        _updateSightRangeDisposable = unitController.SightRange.Subscribe(SetSiegeRange).AddTo(this);
        SetSiegeRange(unitController.SightRange.Value);
    }

    private void SetSiegeRange(float range)
    {
        _collider.radius = range;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var target = other.GetComponent<ITarget>();
        if (target == null || target.IsUntargetable) return;
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

        if (_unitController.TargetType == TargetType.Ally)
        {
            return target.TeamType == _unitController.UnitTable.teamType;
        }

        if (_unitController.TargetType == TargetType.Enemy)
        {
            return target.TeamType != _unitController.UnitTable.teamType;
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
                float dist = GetEdgeDistance(_unitController.transform.position, t);
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
                if (t.MaxHp.Value > maxHp)
                {
                    maxHp = t.MaxHp.Value;
                    closest = t;
                }
            }
        }

        return closest;
    }

    public float GetEdgeDistance(Vector3 unitPos, ITarget target)
    {
        var targetCollider2D = target.Collider2D;
        if (!targetCollider2D)
            return float.MaxValue;

        if (targetCollider2D is BoxCollider2D box)
        {
            return Vector3.Distance(unitPos, box.bounds.ClosestPoint(unitPos));
        }
        else
        {
            float targetRadius = targetCollider2D.bounds.extents.magnitude;
            return Mathf.Max(0f, Vector3.Distance(unitPos, target.Transform.position) - targetRadius);
        }
    }
}