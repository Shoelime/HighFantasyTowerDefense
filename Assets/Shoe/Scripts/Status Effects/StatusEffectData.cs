using UnityEngine;

[CreateAssetMenu(fileName = "NewStatusEffectData", menuName = "StatusEffectData")]
public class StatusEffectData : ScriptableObject
{
    public string effectId;
    public float chanceToApply;
    public float duration;
    public float speedReductionPercentage;
    public int damagePerSecond;
}