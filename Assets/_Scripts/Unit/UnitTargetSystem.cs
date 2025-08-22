using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Unit
{
    public class UnitTargetSystem : MonoBehaviour
    {
        private UnitController _unitController;
        private readonly List<Transform> _targetsInSight = new List<Transform>();

        public void Init(UnitController unitController)
        {
            _unitController = unitController;
        }

        private void OnTriggerEnter(Collider other)
        {
            var enemy = other.GetComponent<UnitController>();
            if (enemy != null && enemy != _unitController && enemy.UnitTable.teamType != _unitController.UnitTable.teamType)
            {
                if (!_targetsInSight.Contains(enemy.transform))
                    _targetsInSight.Add(enemy.transform);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            var enemy = other.GetComponent<UnitController>();
            if (enemy != null && _targetsInSight.Contains(enemy.transform))
            {
                _targetsInSight.Remove(enemy.transform);
            }
        }

        public Transform FindTarget()
        {
            // null 또는 비활성화된 오브젝트 정리
            _targetsInSight.RemoveAll(t => !t.gameObject.activeSelf);

            // 시야 내에서 가장 가까운 적을 선택
            Transform closest = null;
            float minDist = float.MaxValue;
            foreach (var t in _targetsInSight)
            {
                float dist = Vector3.Distance(_unitController.transform.position, t.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    closest = t;
                }
            }

            return closest;
        }
    }
}