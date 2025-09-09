using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class KingSkill : MonoBehaviour
{
    [Serializable]
    public struct SkillData
    {
        public string spwanUnitId;
        public int amount;
        public int probability;
    }

    [SerializeField] private float skillInterval = 10f;
    [SerializeField] private SkillData[] spawnUnits;
    [Inject] private UnitManager _unitManager;
    private float _skillTimer = 0f;
    private UnitController _unitController;
    private bool _isInGroup = false;

    private void Start()
    {
        _unitController = GetComponent<UnitController>();
    }

    private void Update()
    {
        if (_unitController.IsBarrackUnit || _isInGroup) return;
        _skillTimer += Time.deltaTime;
        if (_skillTimer >= skillInterval)
        {
            ActivateSkill();
            _skillTimer = 0f;
        }
    }

    public void SetGroup()
    {
        _isInGroup = true;
    }


    public void GroupSpawnSkill(string id)
    {
        _unitManager.SpawnUnit((Vector2)transform.position + Vector2.up, id, 1, true, false);
    }


    private void ActivateSkill()
    {
        int totalProbability = spawnUnits.Sum(skill => skill.probability);
        int randomValue = UnityEngine.Random.Range(0, totalProbability + 1);
        int cumulativeProbability = 0;
        foreach (var skill in spawnUnits)
        {
            cumulativeProbability += skill.probability;
            if (randomValue <= cumulativeProbability)
            {
                _unitManager.SpawnUnit(transform.position, skill.spwanUnitId, skill.amount);

                break;
            }
        }
    }
}