using UnityEngine;

public enum SpellEffectType { Damage, Heal, Buff, Debuff }

[CreateAssetMenu(menuName = "SimpleSiege/SpellData")]
public class SpellData : ScriptableObject
{
    public SpellEffectType effectType;
    public float effectValue;
    public bool canUseAnywhere;
    public string description;
}

