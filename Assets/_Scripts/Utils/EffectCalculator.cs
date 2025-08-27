using System;
using UnityEngine;

public static class EffectCalculator
{
    /// <summary>
    /// 효과값 계산 공식
    /// </summary>
    public static int CalculateEffectValue(float baseValue, float growth, int level)
    {
        return Mathf.CeilToInt(baseValue * Mathf.Pow(1 + growth, Math.Max(level - 1, 0)));
    }
}

