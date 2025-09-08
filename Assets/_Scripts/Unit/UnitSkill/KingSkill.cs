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

    private void Start()
    {
        _unitController = GetComponent<UnitController>();
    }

    private void Update()
    {
        if (_unitController.IsBarrackUnit) return;
        _skillTimer += Time.deltaTime;
        if (_skillTimer >= skillInterval)
        {
            ActivateSkill();
            _skillTimer = 0f;
        }
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