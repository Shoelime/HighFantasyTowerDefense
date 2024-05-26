public interface IStatusEffect
{
    void Apply(IHealth target);
}

public class StunnedEffect : IStatusEffect
{
    public float Duration { get; private set; }

    public StunnedEffect(float duration)
    {
        Duration = duration;
    }

    public void Apply(IHealth target)
    {
        if (target is Health healthComponent)
        {
            healthComponent.ApplyStun(Duration);
        }
    }
}

public class FrozenEffect : IStatusEffect
{
    public float Duration { get; private set; }

    public FrozenEffect(float duration)
    {
        Duration = duration;
    }

    public void Apply(IHealth target)
    {
        if (target is Health healthComponent)
        {
            healthComponent.ApplyFreeze(Duration);
        }
    }
}

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
}