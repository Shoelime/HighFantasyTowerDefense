using System;
using System.Collections;
using UnityEngine;

public class Projectile : PooledMonoBehaviour
{
    [SerializeField] private DamageData damageData;
    [SerializeField] private GameObject visualObject;
    [SerializeField] private ParticleSystem hitEffects;
    [SerializeField] private AudioClip hitSound;

    [SerializeField] private float destroyDelay;

    private AudioSource audioSource;

    private void OnEnable()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            audioSource.clip = hitSound;
        }

        visualObject.SetActive(true);
    }

    public void CallCoroutine(Vector3 initialPosition, Vector3 targetPosition, float duration, EnemyCharacter targetEnemy = default)
    {
        StartCoroutine(MoveToPosition(initialPosition, targetPosition, duration, targetEnemy));
    }

    public IEnumerator MoveToPosition(Vector3 initialPosition, Vector3 targetPosition, float duration, EnemyCharacter targetEnemy = default)
    {
        float startTime = Time.time;
        Vector3 direction = (targetPosition - initialPosition).normalized;
        float distance = Vector3.Distance(initialPosition, targetPosition);

        while (Time.time - startTime < duration)
        {
            float t = (Time.time - startTime) / duration;
            transform.position = initialPosition + direction * (distance * t);
            yield return null;
        }

        DealDamage(targetEnemy);
        PlayEffects();
    }

    void DealDamage(EnemyCharacter targetEnemy)
    {
        if (damageData.DamageRadius == 0)
        {
            if (targetEnemy != null)
                targetEnemy.HealthComponent.TakeDamage(damageData);
        }
        else
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, damageData.DamageRadius, Vector3.up);

            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.TryGetComponent(out IHealth healthComponent))
                {
                    healthComponent.TakeDamage(damageData);
                }
            }
        }
    }

    private void PlayEffects()
    {
        audioSource.Play();
        hitEffects.Play();
        visualObject.SetActive(false);
        ReturnToPool(destroyDelay);
    }
}
