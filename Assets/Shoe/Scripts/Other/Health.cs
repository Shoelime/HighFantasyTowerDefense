using System;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour, IHealth
{
    private int currentHealth = 0;

    public event Action HealthReachedZero;
    public event Action<int> HealthReduced;
    public event Action<IStatusEffect> EffectApplied;
    public event Action<IStatusEffect> EffectRemoved;

    private readonly HashSet<IStatusEffect> currentEffects = new();
    private readonly Dictionary<IStatusEffect, Coroutine> runningCoroutines = new();

    public void SetStartingHealth(int health)
    {
        currentHealth = health;
    }

    public void TakeDamage(DamageData damageData)
    {
        ReduceHealth(damageData.DamagePerHit);
    }

    public void ReduceHealth(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            HealthReachedZero?.Invoke();
            StopAllCoroutines();
            RemoveAllEffects();
        }
        else
        {
            HealthReduced?.Invoke(currentHealth);
        }
    }

    public void ApplyEffect(IStatusEffect effect)
    {
        if (currentEffects.Contains(effect))
        {
            RestartEffect(effect);
            return;
        }

        EffectApplied?.Invoke(effect);
        currentEffects.Add(effect);
        Coroutine routine = StartCoroutine(effect.Tick(this));
        runningCoroutines[effect] = routine;
    }

    public void RemoveEffect(IStatusEffect effect)
    {
        if (!currentEffects.Contains(effect)) return;

        currentEffects.Remove(effect);

        if (runningCoroutines.TryGetValue(effect, out var coroutine))
        {
            StopCoroutine(coroutine);
            runningCoroutines.Remove(effect);
        }

        EffectRemoved?.Invoke(effect);
    }

    private void RestartEffect(IStatusEffect effect)
    {
        RemoveEffect(effect);
        ApplyEffect(effect);
    }

    private void RemoveAllEffects()
    {
        foreach (var effect in currentEffects)
        {
            EffectRemoved?.Invoke(effect);
        }

        foreach (var coroutine in runningCoroutines.Values)
        {
            StopCoroutine(coroutine);
        }

        currentEffects.Clear();
        runningCoroutines.Clear();
    }

    private void OnDisable()
    {
        RemoveAllEffects();
    }
}
