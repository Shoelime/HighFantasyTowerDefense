using System;
using UnityEngine;
using static StatusEffectData;

public class StatusEffectActivator : MonoBehaviour
{
    [SerializeField] private MeshRenderer[] meshRenderers;
    [SerializeField] private Color frozenColor;
    [SerializeField] private GameObject fireObject;

    private Color[] originalColors;
    private EnemyCharacter enemyCharacter;

    public bool hasEffect;

    private void OnEnable()
    {
        originalColors = new Color[meshRenderers.Length];
        for (int i = 0; i < originalColors.Length; i++)
        {
            originalColors[i] = meshRenderers[i].material.color;
        }

        if (enemyCharacter == null)
            enemyCharacter = GetComponentInParent<EnemyCharacter>();

        enemyCharacter.HealthComponent.EffectApplied += Activate;
        enemyCharacter.HealthComponent.EffectRemoved += Restore;

        StatusEffectData status = new StatusEffectData();
        foreach (EffectType effect in Enum.GetValues(typeof(EffectType)))
        {
            Restore(effect);
        }
    }

    void Activate(StatusEffectData status)
    {
        switch (status.effectType)
        {
            case EffectType.Freeze:
                for (int i = 0; i < meshRenderers.Length; i++)
                {
                    meshRenderers[i].material.color = frozenColor;
                }
                break;
            case EffectType.Burn:
                fireObject.SetActive(true);
                break;
            case EffectType.Stun:
                break;
        }

        hasEffect = true;
    }

    void Restore(StatusEffectData status)
    {
        switch (status.effectType)
        {
            case EffectType.Freeze:
                for (int i = 0; i < meshRenderers.Length; i++)
                {
                    meshRenderers[i].material.color = originalColors[i];
                }
                break;
            case EffectType.Burn:
                fireObject.SetActive(false);
                break;
            case EffectType.Stun:
                break;
        }

        hasEffect = false;
    }

    void Restore(EffectType effectType)
    {
        switch (effectType)
        {
            case EffectType.Freeze:
                for (int i = 0; i < meshRenderers.Length; i++)
                {
                    meshRenderers[i].material.color = originalColors[i];
                }
                break;
            case EffectType.Burn:
                fireObject.SetActive(false);
                break;
            case EffectType.Stun:
                break;
        }

        hasEffect = false;
    }

    private void OnDisable()
    {
        enemyCharacter.HealthComponent.EffectApplied -= Activate;
        enemyCharacter.HealthComponent.EffectRemoved -= Restore;
    }
}
