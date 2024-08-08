public class StunnedEffect : IStatusEffect
{
    public StatusEffectData EffectData { get; set; }

    public StunnedEffect(StatusEffectData data)
    {
        EffectData = data;
    }

    public void Apply(IHealth target)
    {
        if (target is Health healthComponent)
        {
            healthComponent.ApplyEffect(EffectData);
        }
    }
}
