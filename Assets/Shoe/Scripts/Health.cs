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
    public event Action<StatusEffectData> EffectRemoved;

    private HashSet<EffectType> currentEffects = new HashSet<EffectType>();
    private Dictionary<EffectType, Coroutine> runningCoroutines = new Dictionary<EffectType, Coroutine>();

    public bool HasEffect;


    void Update()
    {
         HasEffect = currentEffects.Count > 0;
    }

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

    public void ApplyEffect(StatusEffectData status)
    {
        if (currentEffects.Contains(status.effectType))
        {
            // Reset the effect's timer
            ApplyEffectWithTimer(status);
            return;
        }

        EffectApplied?.Invoke(status);
        currentEffects.Add(status.effectType);
        ApplyEffectWithTimer(status);
    }

    private IEnumerator EffectTick(StatusEffectData status)
    {
        float timer = status.duration;

        while (timer > 0)
        {
            yield return new WaitForSeconds(1);
            timer -= 1;

            if (status.effectType == EffectType.Burn)
            {
                ReduceHealth(status.damagePerSecond);
            }
        }

        RemoveEffect(status);
    }

    private void ApplyEffectWithTimer(StatusEffectData status)
    {
        // Stop any existing coroutine for this effect type
        if (runningCoroutines.ContainsKey(status.effectType))
        {
            StopCoroutine(runningCoroutines[status.effectType]);
            runningCoroutines.Remove(status.effectType);
        }

        // Start a new coroutine and store its reference
        Coroutine coroutine = StartCoroutine(EffectTick(status));
        runningCoroutines[status.effectType] = coroutine;
    }

    void RemoveEffect(StatusEffectData status)
    {
        currentEffects.Remove(status.effectType);
        runningCoroutines.Remove(status.effectType);
        EffectRemoved?.Invoke(status);
    }

    void OnDisable()
    {
        currentEffects.Clear();
        runningCoroutines.Clear();
        StopAllCoroutines();
    }
}
