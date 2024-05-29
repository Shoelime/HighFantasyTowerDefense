public interface IStatusEffect
{
    public StatusEffectData EffectData { get; set; }
    void Apply(IHealth target);
}