using UnityEngine;

[CreateAssetMenu(fileName = "NewStatusEffectData", menuName = "StatusEffectData")]
public class StatusEffectData : ScriptableObject
{
    public enum EffectType { Stun, Freeze, Burn };
    public EffectType effectType;

    public float chanceToApply;
    public float duration;
    public int damagePerSecond;
}