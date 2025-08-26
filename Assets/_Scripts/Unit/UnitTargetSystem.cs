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
        _unitController = unitController;
        _collider = GetComponent<CircleCollider2D>();
        _collider.isTrigger = true;
        _collider.radius = unitController.UnitTable.sightRange;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var enemy = other.GetComponent<UnitController>();
        if (enemy != null && enemy != _unitController && enemy.UnitTable.teamType != _unitController.UnitTable.teamType)
        {
            if (!_targetsInSight.Contains(enemy))
                _targetsInSight.Add(enemy);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var enemy = other.GetComponent<UnitController>();
        if (enemy != null && _targetsInSight.Contains(enemy))
        {
            _targetsInSight.Remove(enemy);
        }
    }


    public ITarget FindTarget()
    {
        // null 또는 비활성화된 오브젝트 정리
        _targetsInSight.RemoveAll(t => !t.Transform.gameObject.activeSelf);

        // 시야 내에서 가장 가까운 적을 선택
        ITarget closest = null;
        float minDist = float.MaxValue;
        foreach (var t in _targetsInSight)
        {
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