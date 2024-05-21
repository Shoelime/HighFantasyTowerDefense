using UnityEngine;

[CreateAssetMenu(fileName = "NewStatusEffectData", menuName = "StatusEffectData")]
public class StatusEffectData : ScriptableObject
{
    public string effectName;
    public float chanceToApply;
    public float duration;
    public float damagePerSecond;
}