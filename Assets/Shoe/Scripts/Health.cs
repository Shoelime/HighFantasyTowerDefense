using System;
using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour, IHealth
{
    private int currentHealth = 0;
    private int damageTickCount = 0;

    public event Action HealthReachedZero;
    public event Action<int> HealthReduced;
    public event Action<float> BurnApplied;
    public event Action<float> FreezeApplied;
    public event Action<float> StunApplied;

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

    internal void ApplyBurn(int damagePerSecond, float duration)
    {
        BurnApplied?.Invoke(duration);
        StartCoroutine(DamageTick(damagePerSecond, duration));
    }

    internal void ApplyFreeze(float duration)
    {
        FreezeApplied?.Invoke(duration);
    }

    internal void ApplyStun(float duration)
    {
        StunApplied?.Invoke(duration);
    }
}
