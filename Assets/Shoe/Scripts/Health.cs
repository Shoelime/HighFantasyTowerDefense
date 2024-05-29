using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StatusEffectData;

public class Health : MonoBehaviour, IHealth
{
    private int currentHealth = 0;
    private int damageTickCount = 0;

    public event Action HealthReachedZero;
    public event Action<int> HealthReduced;
    public event Action<StatusEffectData> EffectApplied;

    private HashSet<EffectType> currentEffects = new HashSet<EffectType>();

    public void SetStartingHealth(int health)
    {
        currentHealth = health;
        damageTickCount = 0;
    }

    public void TakeDamage(DamageData damageData)
    {
        ReduceHealth(damageData.DamagePerHit);
    }

    private void ReduceHealth(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            HealthReachedZero?.Invoke();
            StopCoroutine(nameof(DamageTick));
        }
        else HealthReduced?.Invoke(currentHealth);
    }

    private IEnumerator DamageTick(int damagePerSecond, float duration)
    {
        damageTickCount = (int)duration;

        while (damageTickCount > 0)
        {
            yield return new WaitForSeconds(1);
            damageTickCount--;
            ReduceHealth(damagePerSecond);
        }
    }

    private void ApplyBurn(int damagePerSecond, float duration)
    {
        StartCoroutine(DamageTick(damagePerSecond, duration));
    }

    internal void ApplyEffect(StatusEffectData status)
    {
        if (currentEffects.Contains(status.effectType))
            return;

        EffectApplied?.Invoke(status);
        currentEffects.Add(status.effectType);

        switch (status.effectType)
        {
            case EffectType.Freeze:            
                break;
            case EffectType.Burn:
                ApplyBurn(status.damagePerSecond, status.duration);
                break;
            case EffectType.Stun:
                break;
        }
    }

    void RemoveEffect(EffectType effectType)
    {
        currentEffects.Remove(effectType);
    }
}
