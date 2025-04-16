using UnityEngine;

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
        if (originalColors == null)
        {
            originalColors = new Color[meshRenderers.Length];
            for (int i = 0; i < originalColors.Length; i++)
            {
                originalColors[i] = meshRenderers[i].material.color;
            }
        }

        if (enemyCharacter == null)
            enemyCharacter = GetComponentInParent<EnemyCharacter>();

        enemyCharacter.HealthComponent.EffectApplied += Activate;
        enemyCharacter.HealthComponent.EffectRemoved += Restore;

        ResetAllEffects();
    }

    void Activate(IStatusEffect effect)
    {
        switch (effect)
        {
            case BurningEffect:
                fireObject.SetActive(true);
                break;
            case FrozenEffect:
                for (int i = 0; i < meshRenderers.Length; i++)
                {
                    meshRenderers[i].material.color = frozenColor;
                }
                break;
            case StunnedEffect:
                break;
        }

        hasEffect = true;
    }

    void Restore(IStatusEffect effect)
    {
        switch (effect)
        {
            case BurningEffect:
                fireObject.SetActive(false);
                break;
            case FrozenEffect:
                for (int i = 0; i < meshRenderers.Length; i++)
                {
                    meshRenderers[i].material.color = originalColors[i];
                }
                break;
            case StunnedEffect:
                break;
        }

        hasEffect = false;
    }

    void ResetAllEffects()
    {
        fireObject.SetActive(false);

        for (int i = 0; i < meshRenderers.Length; i++)
        {
            meshRenderers[i].material.color = originalColors[i];
        }

        hasEffect = false;
    }

    private void OnDisable()
    {
        enemyCharacter.HealthComponent.EffectApplied -= Activate;
        enemyCharacter.HealthComponent.EffectRemoved -= Restore;
    }
}
