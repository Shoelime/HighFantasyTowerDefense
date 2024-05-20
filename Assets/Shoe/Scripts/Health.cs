using System;
using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour, IHealth
{
    private int currentHealth = 0;
    private int damageTickCount = 0;

    public Action HealthReachedZero;

    public void SetHealth(int health)
    {
        currentHealth = health;
        damageTickCount = 0;
    }
    public void TakeDamage(DamageData damageData)
    {
        ReduceHealth(damageData.DamagePerHit);
        if (damageData.DamagePerSecondDuration > 0)
            StartCoroutine(DamageTick(damageData));
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
}
