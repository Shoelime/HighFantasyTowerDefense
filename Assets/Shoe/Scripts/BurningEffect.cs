public class BurningEffect : IStatusEffect
{
    public int DamagePerSecond { get; private set; }
    public float Duration { get; private set; }

    public BurningEffect(int damagePerSecond, float duration)
    {
        DamagePerSecond = damagePerSecond;
        Duration = duration;
    }

    public void Apply(IHealth target)
    {
        if (target is Health healthComponent)
        {
            healthComponent.ApplyBurn(DamagePerSecond, Duration);
        }
    }

    public void Remove(IHealth target)
    {
        throw new System.NotImplementedException();
    }
}