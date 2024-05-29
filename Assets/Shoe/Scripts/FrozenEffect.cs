public class FrozenEffect : IStatusEffect
{
    public StatusEffectData EffectData { get; set; }

    public FrozenEffect(StatusEffectData data)
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
