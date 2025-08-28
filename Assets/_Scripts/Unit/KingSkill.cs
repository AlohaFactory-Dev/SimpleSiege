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
    private float skillTimer = 0f;

    private void Update()
    {
        skillTimer += Time.deltaTime;
        if (skillTimer >= skillInterval)
        {
            ActivateSkill();
            skillTimer = 0f;
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
                for (int i = 0; i < skill.amount; i++)
                {
                    _unitManager.SpawnUnit(transform.position, skill.spwanUnitId, skill.amount);
                }

                break;
            }
        }
    }
}