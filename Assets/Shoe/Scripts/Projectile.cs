using System.Collections;
using UnityEngine;
using static StatusEffectData;

public class Projectile : PooledMonoBehaviour
{
    private DamageData damageData;
    [SerializeField] private GameObject visualObject;
    [SerializeField] private ParticleSystem hitEffects;
    [SerializeField] private AudioClip hitSound;

    [SerializeField] private float destroyDelay;

    private AudioSource audioSource;
    public LineRenderer LineRenderer { get; private set; }
    public void SetDamageData(DamageData damageData) { this.damageData = damageData; }

    private void OnEnable()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            audioSource.clip = hitSound;
        }

        if (LineRenderer == null)
        {
            LineRenderer = GetComponent<LineRenderer>();
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
    }

    public void DealDamage(EnemyCharacter targetEnemy)
    {
        if (damageData.DamageRadius == 0)
        {
            if (targetEnemy != null)
            {
                targetEnemy.HealthComponent.TakeDamage(damageData);
                ApplyStatusEffects(targetEnemy.HealthComponent);
            }
        }
        else
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, damageData.DamageRadius, Vector3.up);

            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.TryGetComponent(out IHealth healthComponent))
                {
                    healthComponent.TakeDamage(damageData);
                    ApplyStatusEffects(healthComponent);
                }
            }
        }

        PlayHitEffects();
    }

    public void ApplyStatusEffects(IHealth healthComponent)
    {

        if (damageData.StatusEffects.Count > 0)
        {
            int randomInt = Random.Range(0, 100);

            foreach (var status in damageData.StatusEffects)
            {
                if (randomInt < status.chanceToApply)
                {
                    IStatusEffect effect = null;
                    switch (status.effectType)
                    {
                        case EffectType.Burn:
                            effect = new BurningEffect(status.damagePerSecond, status.duration);
                            break;
                        case EffectType.Freeze:
                            effect = new FrozenEffect(status.duration);
                            break;
                        case EffectType.Stun:
                            effect = new StunnedEffect(status.duration);
                            break;
                    }
                    effect?.Apply(healthComponent);
                }
            }
        }
    }

    private void PlayHitEffects()
    {
        if (audioSource != null) audioSource.Play();
        if (hitEffects != null) hitEffects.Play();
        if (visualObject != null) visualObject.SetActive(false);

        ReturnToPool(destroyDelay);
    }
}
