public class BurningEffect : IStatusEffect
{
    public StatusEffectData EffectData { get; set; }

    public BurningEffect(StatusEffectData data)
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