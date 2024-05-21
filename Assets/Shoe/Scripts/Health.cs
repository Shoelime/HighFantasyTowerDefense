using System;
using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour, IHealth
{
    private int currentHealth = 0;
    private int damageTickCount = 0;

    public event Action HealthReachedZero;
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
    }

    private IEnumerator DamageTick(DamageData damageData)
    {
        damageTickCount = damageData.DamagePerSecondDuration;

        while (damageTickCount > 0)
        {
            yield return new WaitForSeconds(1);
            damageTickCount--;
            ReduceHealth(damageData.DamagePerSecond);
        }
    }

    internal void ApplyBurn(float damagePerSecond, float duration)
    {
        throw new NotImplementedException();
    }

    internal void ApplyFreeze(float duration)
    {
        throw new NotImplementedException();
    }

    internal void ApplyStun(float duration)
    {
        throw new NotImplementedException();
    }
}
